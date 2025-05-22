using AndroidX.Navigation.Fragment;
using Microsoft.Maui.Platform;
using PJ.NavigationTrans.Maui;
using AView = Android.Views.View;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;
sealed class StackNavigationManagerTrans : StackNavigationManager
{
	public StackNavigationManagerTrans(IMauiContext mauiContext) : base(mauiContext)
	{
	}

	// Past most of the code because they are internal, hopefuly will open on .NET 10.
	public override FragmentNavigator.Destination AddFragmentDestination()
	{
		var fragmentNavigator = UnsafeAccessorClass.GetUnsafeFragmentNavigator(this);
		var navGraph = UnsafeAccessorClass.GetUnsafeNavGraph(this);

		var destination = new FragmentNavigator.Destination(fragmentNavigator);
		var canonicalName = Java.Lang.Class.FromType(typeof(NavigationViewFragmentTrans)).CanonicalName;

		if (canonicalName is not null)
			destination.SetClassName(canonicalName);

		destination.Id = AView.GenerateViewId();
		navGraph.AddDestination(destination);
		return destination;
	}
}

