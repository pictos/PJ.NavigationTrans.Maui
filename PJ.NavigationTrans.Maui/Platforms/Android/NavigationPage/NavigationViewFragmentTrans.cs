#nullable disable
using Microsoft.Maui.Platform;
using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;

sealed class NavigationViewFragmentTrans : NavigationViewFragment
{
	public override AAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
	{
		var navigationManager = UnsafeAccessorClass.GetStackNavigationManager(this);

		var page = (ContentPage)navigationManager.CurrentPage;

		var animation = AnimationHelpers.GetInfo(page);

		if (animation.AnimationIn == TransitionType.Default || animation.AnimationOut == TransitionType.Default)
		{
			return base.OnCreateAnimation(transit, enter, nextAnim);
		}

		return enter ? animation.AnimationIn.ToPlatform(animation.Duration).Animation : animation.AnimationOut.ToPlatform(animation.Duration).Animation;
	}
}
