using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BabyYodaBot.Core.Net;
using BabyYodaBot.Core.BabyYoda.Models;

namespace BabyYodaBot.Core.BabyYoda
{
    public class UnityBabyYodaClient : IBabyYodaClient, IDisposable
    {
        private readonly ConcurrentQueue<string> requests = new ConcurrentQueue<string>();
        private readonly ILogger logger;
        private readonly IMessageBus messageBus;
        private readonly IGameClient client;

        public UnityBabyYodaClient(
            ILogger logger,
            IMessageBus messageBus,
            IGameClient2 client)
        {
            this.logger = logger;
            this.messageBus = messageBus;

            this.client = client;
            this.client.Connected += Client_OnConnect;
            this.client.Subscribe("message", SendResponseToTwitchChat);
        }

        private async void Client_OnConnect(object sender, EventArgs e)
        {
            while (requests.TryDequeue(out var request))
            {
                await this.client.SendAsync(request);
            }
        }
        public Task FeedAsync(Player user)
            => SendAsync("feed", user);

        private async Task SendAsync<T>(string name, T packet)
        {
            var request = name + ":" + JsonConvert.SerializeObject(packet);

            if (!this.client.IsConnected)
            {
                this.EnqueueRequest(request);
                return;
            }

            await this.client.SendAsync(request);
        }

        private void SendResponseToTwitchChat(IGameCommand obj)
        {
            if (string.IsNullOrEmpty(obj.Destination))
            {
                this.messageBus.Send(MessageBus.Broadcast, obj.Args.LastOrDefault());
            }
            else
            {
                this.messageBus.Send(MessageBus.Message, obj.Destination + ", " + obj.Args.LastOrDefault());
            }
        }

        private void EnqueueRequest(string request)
        {
            this.requests.Enqueue(request);
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.client.Connected -= Client_OnConnect;
        }

        public Task<bool> ProcessAsync(int serverPort)
        {
            return this.client.ProcessAsync(serverPort);
        }
    }
}
