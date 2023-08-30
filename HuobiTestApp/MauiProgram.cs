using Huobi.Net;
using HuobiTestApp.Configuration;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
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

        builder.Services.AddTransient<IHuobiService, HuobiService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        
        builder.Services.AddMudServices(config =>
        {
			config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
        });

        builder.Services.AddHuobi(options =>
        {
			options.RequestTimeout = TimeSpan.FromSeconds(60);
        });

        var accounts = builder.Configuration.GetRequiredSection("Accounts").Get<IEnumerable<HuobiAccountSettings>>();

        builder.Services.Configure<AppSettings>(options => options.Accounts = accounts);

		return builder.Build();
	}
}
