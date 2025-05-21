#nullable disable
using Microsoft.Maui.Platform;
using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;

sealed class NavigationViewFragmentTrans : NavigationViewFragment
{
	public override AAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
	{
		var navigationManager = UnsafeAccessorClass.GetStackNavigationManager(this);

		var isPopping = IKnowYouAreThere(navigationManager);

		if (isPopping is null)
		{
			return null;
		}

		var page = (ContentPage)navigationManager.CurrentPage;

		var animation = AnimationHelpers.GetInfo(page);

		// This shouldn't typically happen, but if a previous animation hasn't finished from a navigation that was interrupted
		// the opacity of the view will be set to 0. This will reset it to 1.
		page.Opacity = 1;

		return !enter ? animation.AnimationOut.ToPlatform(animation.Duration).Animation : animation.AnimationIn.ToPlatform(animation.Duration).Animation;
	}


	// For some reason  UnsafeAccessorClass.GetIUnsafeIsPopping(navigationManager); is throwing `MissingMethodException` and can't see the reason.
	static bool? IKnowYouAreThere(StackNavigationManager manager)
	{
		var propInfo = typeof(StackNavigationManager).GetProperty("IsPopping", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		Assert(propInfo is not null);

		return (bool?)propInfo.GetValue(manager);
	}
}
