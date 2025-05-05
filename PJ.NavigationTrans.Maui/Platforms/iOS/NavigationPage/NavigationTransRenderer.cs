using Microsoft.Maui.Controls.Handlers.Compatibility;
using UIKit;

namespace PJ.NavigationTrans.Maui;

sealed class NavigationTransRenderer : NavigationRenderer
{
	public override void PushViewController(UIViewController viewController, bool animated)
	{
		if (UnsafeAccessorClass.GetUnsafeCurrentPageProperty(this) is Page currentPage)
		{
			var fromUIView = VisibleViewController.View;

			Assert(fromUIView is not null);

			animated = CreateAndApplyAnimation(currentPage, NavigationRequestType.Push, fromUIView);
		}

		base.PushViewController(viewController, animated);
	}

	//TODO: Implement animation for PopAsync
	public override UIViewController PopViewController(bool animated)
	{
		if (UnsafeAccessorClass.GetUnsafeCurrentPageProperty(this) is Page currentPage)
		{
			var fromUIView = VisibleViewController.View;

			Assert(fromUIView is not null);

			animated = CreateAndApplyAnimation(currentPage, NavigationRequestType.Pop, fromUIView);
		}

		return base.PopViewController(animated);
	}

	bool CreateAndApplyAnimation(Page currentPage, NavigationRequestType navigationRequest, UIView currentView)
	{
		var info = AnimationHelpers.GetInfo(currentPage);

		if (info.AnimationIn == TransitionType.Default)
		{
			return true;
		}

		var toAnimation = navigationRequest == NavigationRequestType.Push ? info.AnimationIn : info.AnimationOut;
		var fromAnimation = navigationRequest != NavigationRequestType.Push ? info.AnimationIn : info.AnimationOut;

		var view = ViewController.View;

		Assert(view is not null);
		Assert(currentView is not null);

		var window = view.Window;

		Assert(window is not null);

		// Add the currentView into the view in order to animate it.
		window.InsertSubview(currentView, 0);

		view.Layer.RemoveAllAnimations();
		currentView.Layer.RemoveAllAnimations();

		currentView.SelectAndRunAnimation(fromAnimation, info.Duration, currentView.RemoveFromSuperview);
		view.SelectAndRunAnimation(toAnimation, info.Duration);

		return false;
	}
}