using Huobi.Net;
using HuobiTestApp.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
using Huobi.Net.Clients;
using Huobi.Net.Interfaces.Clients;
using HuobiTestApp.Services;
using Microsoft.Extensions.Configuration;

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

       IHuobiRestClient huobiRestClient = new HuobiRestClient(options =>
       {
           options.RequestTimeout = TimeSpan.FromSeconds(60);
       });

       IHuobiSocketClient huobiSocketClient = new HuobiSocketClient(options =>
       {
           options.RequestTimeout = TimeSpan.FromSeconds(60);
       });

        builder.Services.AddSingleton(huobiRestClient);
        builder.Services.AddSingleton(huobiSocketClient);

        builder.Services.AddTransient<IHuobiService, HuobiService>();

        builder.Services.Configure<AppSettings>(builder.Configuration);

		return builder.Build();
	}
}
