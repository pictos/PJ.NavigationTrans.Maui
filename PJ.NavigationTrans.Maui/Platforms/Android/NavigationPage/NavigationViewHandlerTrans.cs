using Android.Runtime;
using Android.Views;
using AndroidX.Fragment.App;
using Microsoft.Maui.Handlers;
using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;

sealed class NavigationViewHandlerTrans
{
	// I don't want anyone to create an instance of this class
	// it exists just to hold the static method below
	NavigationViewHandlerTrans()
	{

	}

	public static void HackNavigationViewHandler()
	{
		NavigationViewHandler.PlatformViewFactory = (h) =>
		{
			var navigationManager = new StackNavigationManagerTrans(h.MauiContext!);
			UnsafeAccessorClass.GetSetStackNavigationManager((NavigationViewHandler)h) = navigationManager;

			LayoutInflater? li = navigationManager.MauiContext?.GetLayoutInflater() ?? throw new InvalidOperationException($"LayoutInflater cannot be null");
			var view = li.Inflate(Microsoft.Maui.Resource.Layout.fragment_backstack, null).JavaCast<FragmentContainerView>() ?? throw new InvalidOperationException($"Resource.Layout.navigationlayout view not found");

			return view;
		};
	}
}
