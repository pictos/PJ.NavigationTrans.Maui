namespace PJ.NavigationTrans.Maui;

public static class AppBuilderExtensions
{
	public static MauiAppBuilder UseCustomTransitions(this MauiAppBuilder builder)
	{
#if ANDROID || IOS
		builder.ConfigureMauiHandlers(h =>
		{
			h.AddHandler(typeof(Shell), typeof(ShellTransRenderer));

#if IOS
			h.AddHandler(typeof(NavigationPage), typeof(NavigationTransRenderer));
#endif
		});
#endif

		return builder;
	}
}
