using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Radzen;
using Utensils_Client.Shared.Handlers;
using Utensils_Client.Shared.Services;

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

		// Add a hhtpclient factory, with a named client, set the baseaddress and add
		// a delegation handler
		builder.Services.AddHttpClient("Auth", client => client.BaseAddress = new Uri("http://localhost:5000"));
            
        builder.Services.AddHttpClient("Data", client => client.BaseAddress = new Uri("http://localhost:5000"))
            .AddHttpMessageHandler(() => new TokenHandler());

        // Register needed elements for authentication
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());

		builder.Services.AddScoped<AuthService>();
		builder.Services.AddScoped<GroupService>();
		builder.Services.AddScoped<EventService>();
		builder.Services.AddScoped<UserService>();

		// radzen services
        builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();

        return builder.Build();
	}
}
