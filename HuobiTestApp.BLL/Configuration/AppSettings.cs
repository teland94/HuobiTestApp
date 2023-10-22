namespace HuobiTestApp.BLL.Configuration
{
    public class AppSettings
    {
        public string UserAgent { get; set; }

        public string BaseAddress { get; set; }

        public IEnumerable<HuobiAccountSettings> Accounts { get; set; }
    }
}
