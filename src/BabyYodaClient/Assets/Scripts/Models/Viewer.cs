namespace Assets.Scripts.Models
{
    public class Viewer
    {
        public Viewer(TwitchUser user)
        {
            User = user;
        }

        public TwitchUser User { get; set; }
    }
}
