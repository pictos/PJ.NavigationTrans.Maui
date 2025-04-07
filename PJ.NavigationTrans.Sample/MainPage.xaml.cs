namespace PJ.NavigationTrans.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	void Button_Clicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//" + nameof(NewPage1));
	}

	void Button_Clicked_1(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//" + nameof(NewPage2));
	}
}

