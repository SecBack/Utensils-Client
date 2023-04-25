using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Radzen;
using Utensils_Client.Data;
using Utensils_Client.Shared;

namespace Utensils_Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        //Register needed elements for authentication
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

        builder.Services.AddSingleton<WeatherForecastService>();

		// radzen services
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();

        return builder.Build();
	}
}
