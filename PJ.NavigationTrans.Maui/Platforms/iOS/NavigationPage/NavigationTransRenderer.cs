using Microsoft.Maui.Controls.Handlers.Compatibility;
using UIKit;

namespace PJ.NavigationTrans.Maui;

sealed class NavigationTransRenderer : NavigationRenderer
{
	// Today when the `PopViewCotoller` is called, the `OnPopViewAsync` is called as well causing issues with animation,
	// for now we just return earlier like the base implementation does. In a future release maybe this is better handled in the base class.
	bool ignorePopCall;

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

	public override UIViewController PopViewController(bool animated)
	{
		ignorePopCall = true;
		if (UnsafeAccessorClass.GetUnsafeCurrentPageProperty(this) is Page currentPage)
		{
			var fromUIView = VisibleViewController.View;

			Assert(fromUIView is not null);

			animated = CreateAndApplyAnimation(currentPage, NavigationRequestType.Pop, fromUIView);
		}

		return base.PopViewController(animated);
	}

	protected override Task<bool> OnPopViewAsync(Page page, bool animated)
	{
		if (ignorePopCall)
		{
			ignorePopCall = false;
			return Task.FromResult(true);
		}

		var fromUIView = VisibleViewController.View;

		Assert(fromUIView is not null);

		animated = CreateAndApplyAnimation(page, NavigationRequestType.Pop, fromUIView);

		return base.OnPopViewAsync(page, animated);
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

		view.Layer.RemoveAllAnimations();
		currentView.Layer.RemoveAllAnimations();

		// If we use the Built-In animations, we don't add the `currentView` to the `window`.
		if (toAnimation.IsBuiltIn() || fromAnimation.IsBuiltIn())
		{
			view.SelectAndRunAnimation(toAnimation, info.Duration);
			goto END;
		}

		return navigationRequest == NavigationRequestType.Pop ? HandlePop() : HandlePush();

		END:
		return false;


		bool HandlePush()
		{
			window.InsertSubview(currentView, 0);
			currentView.SelectAndRunAnimation(fromAnimation, info.Duration, () =>
			{
				currentView.RemoveFromSuperview();
			});
			view.SelectAndRunAnimation(toAnimation, info.Duration);
			return false;
		}


		// This causes a black background, not sure why it happens, but it is not a big issue.
		bool HandlePop()
		{
			window.InsertSubview(view, 0);

			view.SelectAndRunAnimation(fromAnimation, info.Duration);
			currentView.SelectAndRunAnimation(toAnimation, info.Duration);

			return false;
		}
	}
}
