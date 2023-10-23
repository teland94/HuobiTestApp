using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
using Huobi.Net.Clients;
using Huobi.Net.Interfaces.Clients;
using Microsoft.Extensions.Configuration;
using HuobiTestApp.BLL.Configuration;
using HuobiTestApp.BLL.Services;
using HuobiTestApp.BLL.Interfaces;

namespace HuobiTestApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream($"{nameof(HuobiTestApp)}.appsettings.json");

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        
        builder.Services.AddMudServices(config =>
        {
			config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
        });

        builder.Services.AddHttpClient<IHuobiService, HuobiService>(
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>(nameof(AppSettings.BaseAddress)));
                httpClient.Timeout = TimeSpan.FromSeconds(60);

                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(builder.Configuration.GetValue<string>(nameof(AppSettings.UserAgent)));
            });

        builder.Services.AddTransient<IHuobiRestClient, HuobiRestClient>(_ =>
        {
            return new HuobiRestClient(options =>
            {
                options.RequestTimeout = TimeSpan.FromSeconds(60);
            });
        });

        builder.Services.AddTransient<IHuobiSocketClient, HuobiSocketClient>(_ =>
        {
            return new HuobiSocketClient(options =>
            {
                options.RequestTimeout = TimeSpan.FromSeconds(60);
            });
        });

        builder.Services.Configure<AppSettings>(builder.Configuration);

		return builder.Build();
	}
}
