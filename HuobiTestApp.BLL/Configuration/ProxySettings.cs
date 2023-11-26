namespace JobResponseApp.Worker.Configuration;

public class ProxySettings
{
    public bool Enabled { get; set; }

    public string Url { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }
}