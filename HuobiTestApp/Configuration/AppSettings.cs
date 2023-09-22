namespace HuobiTestApp.Configuration
{
    public class AppSettings
    {
        public string UserAgent { get; set; }

        public IEnumerable<HuobiAccountSettings> Accounts { get; set; }
    }
}
