using Microsoft.Extensions.Logging;
 using OtoKMaui.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OtoKMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        builder.Services.AddDbContext<DataContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


        builder
            .UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
		
#endif

		return builder.Build();
	}
}
