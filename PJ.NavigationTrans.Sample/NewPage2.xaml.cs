namespace PJ.NavigationTrans.Sample;

public partial class NewPage2 : ContentPage
{
	public NewPage2()
	{
		InitializeComponent();

#if ANDROID
		Maui.NavigationTrans.SetAndroidTransitions(this, Resource.Animation.scale_in, Resource.Animation.flip_out, 1500);
#endif

		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, __) =>
		{
			if (App.IsShell)
				Shell.Current.GoToAsync("..", true);
			else
				Navigation.PopAsync();
		};

		this.Content.GestureRecognizers.Add(tap);
	}
}