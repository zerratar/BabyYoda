using System;

namespace BabyYodaBot.Core
{
    public interface ICommandListener : IDisposable
    {
        void Start();
        void Stop();
    }
}