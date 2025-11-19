#nullable disable
using Microsoft.Maui.Platform;
using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;

sealed class NavigationViewFragmentTrans : NavigationViewFragment
{
	// TODO Try the OnCreateAnimator method instead.
	// https://github.com/dotnet/maui/issues/21187
	public override AAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
	{
		var navigationManager = (StackNavigationManagerTrans)UnsafeAccessorClass.GetStackNavigationManager(this);

		var animation = navigationManager.TransInfo;

		if (animation.AnimationIn == TransitionType.Default || animation.AnimationOut == TransitionType.Default)
		{
			return base.OnCreateAnimation(transit, enter, nextAnim);
		}

		return enter ? animation.AnimationIn.ToPlatform(animation.Duration).Animation : animation.AnimationOut.ToPlatform(animation.Duration).Animation;
	}
}