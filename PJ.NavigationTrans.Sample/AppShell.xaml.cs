#if IOS
using PJ.NavigationTrans.Sample.Platforms.iOS; 
#endif

namespace PJ.NavigationTrans.Sample;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(NewPage1), typeof(NewPage1));
		Routing.RegisterRoute(nameof(NewPage2), typeof(NewPage2));

#if ANDROID
		Maui.NavigationTrans.SetAndroidTransitions(this.content, Resource.Animation.flip_in, Resource.Animation.scale_out, 1500);
#elif IOS
		Maui.NavigationTrans.SetIosTransitions(this.content, MyAnimations.FlipAnimation, MyAnimations.ConfigFlipAnimation, MyAnimations.ScaleAnimation, MyAnimations.ConfigScaleAnimation, 2000);
#endif
	}
}
