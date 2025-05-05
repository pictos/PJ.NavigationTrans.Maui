namespace PJ.NavigationTrans.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	// FlyoutNavigation
	void Button_Clicked(object sender, EventArgs e)
	{
		if (App.IsShell)
			Shell.Current.GoToAsync("//" + nameof(NewPage1));

	}

	// FlyoutNavigation
	void Button_Clicked_1(object sender, EventArgs e)
	{
		if (App.IsShell)
			Shell.Current.GoToAsync("//" + nameof(NewPage2));

	}

	void Button_Clicked_2(object sender, EventArgs e)
	{
		if (App.IsShell)
			Shell.Current.GoToAsync(nameof(NewPage1));
		else
			Navigation.PushAsync(new NewPage1());

	}

	void Button_Clicked_3(object sender, EventArgs e)
	{
		if (App.IsShell)
			Shell.Current.GoToAsync(nameof(NewPage2));
		else
			Navigation.PushAsync(new NewPage2());

	}
}

