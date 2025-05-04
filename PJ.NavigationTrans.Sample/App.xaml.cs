namespace PJ.NavigationTrans.Sample;

public partial class App : Application
{
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

		return new Window(navPage);
	}
}