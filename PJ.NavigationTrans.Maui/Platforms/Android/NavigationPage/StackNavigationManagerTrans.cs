using AndroidX.Navigation.Fragment;
using Microsoft.Maui.Platform;
using PJ.NavigationTrans.Maui;
using AView = Android.Views.View;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;
sealed class StackNavigationManagerTrans : StackNavigationManager
{
	public TransInfo TransInfo { get; private set; }

	IReadOnlyList<IView> oldStack = [];

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

	public override void RequestNavigation(NavigationRequest e)
	{
		var stack = e.NavigationStack;
		CalculateAnimationpage(stack);
		base.RequestNavigation(e);
	}

	// The way navigation on android works will cause issues with Pop animations,
	// So adding this method to make sure it will get the correct approach for each page
	void CalculateAnimationpage(IReadOnlyList<IView> views)
	{
		if (oldStack.Count is 0)
		{
			oldStack = views;
			var animationPage = (Page)oldStack[^1];
			TransInfo = AnimationHelpers.GetInfo(animationPage);
			return;
		}

		// Pop
		if (oldStack.Count > views.Count)
		{
			oldStack = views;
			return;
		}

		//Push
		if (oldStack.Count < views.Count)
		{
			oldStack = views;
			var animationPage = (Page)oldStack[^1];
			TransInfo = AnimationHelpers.GetInfo(animationPage);
		}
	}
}

