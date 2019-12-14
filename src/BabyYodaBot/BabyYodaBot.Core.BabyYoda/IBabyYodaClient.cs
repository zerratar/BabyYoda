using System.Threading.Tasks;
using BabyYodaBot.Core.BabyYoda.Models;

namespace BabyYodaBot.Core.BabyYoda
{
    public interface IBabyYodaClient
    {
        Task<bool> ProcessAsync(int serverPort);
        Task FeedAsync(Player player);
    }
}
