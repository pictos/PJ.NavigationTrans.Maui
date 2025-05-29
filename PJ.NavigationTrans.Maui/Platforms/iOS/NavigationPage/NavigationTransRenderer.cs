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

		if (info.AnimationIn == TransitionType.Default & info.AnimationOut == TransitionType.Default)
		{
			return true;
		}

		var isPush = navigationRequest == NavigationRequestType.Push;

		var toAnimation = isPush ? info.AnimationIn : info.AnimationOut;

		var view = ViewController.View;

		Assert(view is not null);
		Assert(currentView is not null);

		var window = view.Window;

		Assert(window is not null);

		view.Layer.RemoveAllAnimations();
		currentView.Layer.RemoveAllAnimations();

		window.BackgroundColor = UIColor.White;

		var toAnimationIsBuiltIn = toAnimation.IsBuiltIn();

		if (toAnimationIsBuiltIn)
		{
			view.SelectAndRunAnimation(toAnimation, info.Duration);
			goto END;
		}

		var customAnimation = currentPage.ComputeCustomAnimation();
		view.RunCustomAnimation(customAnimation, AnimationHelpers.EmptyAction, isPush);

		END:
		return false;
	}
}