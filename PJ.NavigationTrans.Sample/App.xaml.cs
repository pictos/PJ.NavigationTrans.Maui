namespace PJ.NavigationTrans.Sample;

public partial class App : Application
{
	internal static bool IsShell => Shell.Current is not null;

	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var navPage = new NavigationPage(new MainPage());

		navPage.HandlerChanged += (_, __) =>
		{
			if (navPage.Handler is null)
			{
				return;
			}


			// iOS Navigation Renderer
			// Android Navigation Handler
			_ = navPage.Handler;
		};

		if (IsShell)
		{
			return new Window(new AppShell());
		}

		return new Window(navPage);
	}
}