#if IOS
using PJ.NavigationTrans.Sample.Platforms.iOS; 
#endif

namespace PJ.NavigationTrans.Sample;

public partial class NewPage2 : ContentPage
{
	public NewPage2()
	{
		InitializeComponent();

#if ANDROID
		Maui.NavigationTrans.SetAndroidTransitions(this, Resource.Animation.scale_in, Resource.Animation.flip_out, 1500);
#elif IOS
		Maui.NavigationTrans.SetIosTransitions(this, MyAnimations.FlipAnimation, MyAnimations.ConfigFlipAnimation, MyAnimations.ScaleAnimation, MyAnimations.ConfigScaleAnimation, 2000);
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