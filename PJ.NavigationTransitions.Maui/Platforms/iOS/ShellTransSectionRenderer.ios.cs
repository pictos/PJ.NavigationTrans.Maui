using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using UIKit;

namespace PJ.NavigationTransitions.Maui;
class ShellTransSectionRenderer : ShellSectionRenderer
{
	bool isPush;
	IShellContext context;
	Page? currentPage;

	public ShellTransSectionRenderer(IShellContext context) : base(context)
	{
		this.context = context;
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		Delegate = new AnimationNavigationControllerDelegate();
		// Implement some property to control this
		//InteractivePopGestureRecognizer.Enabled = 
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values
		currentPage = page;
	}

	//public override UIViewController[] PopToRootViewController(bool animated)
	//{
	//	var p = context.Shell.CurrentPage;
	//	Pop(animated);
	//	var r = base.PopToRootViewController(animated);

	//	p = context.Shell.CurrentPage;
	//	return r;
	//}

	//public override UIViewController PopViewController(bool animated)
	//{
	//	Pop(animated);
	//	return base.PopViewController(!animated);
	//}

	void Pop(bool animated)
	{
		if (!animated || View is null)
			return;


		var page = currentPage;
		var oldView = View.SnapshotView(false);
		var newView = View;

		//Assert(oldView is not null);
		Assert(page is not null);

		newView.Layer.RemoveAllAnimations();

		if (oldView is not null)
			newView.Superview?.AddSubview(oldView);

		var info = AnimationHelpers.GetInfo(page);

		oldView?.SelectAndRunAnimation(info.AnimationOut, info.Duration + 1, () =>
		{
			oldView.Layer.RemoveAllAnimations();
			oldView.RemoveFromSuperview();
			oldView = null;
		});
		newView.SelectAndRunAnimation(info.AnimationIn, info.Duration);
	}
}

sealed class AnimationNavigationControllerDelegate : UINavigationControllerDelegate
{
	TransitionAnimator _animator = new TransitionAnimator();

	public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
	{
		return _animator;
	}
}

sealed class TransitionAnimator : UIViewControllerAnimatedTransitioning
{
	const double _duration = 3.5;

	public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
	{
		var containerView = transitionContext.ContainerView;
		var toView = transitionContext.GetViewFor(UITransitionContext.ToViewKey);
		var fromView = transitionContext.GetViewFor(UITransitionContext.FromViewKey);
		containerView.ClearSubviews();

		containerView.AddSubview(fromView);
		containerView.AddSubview(toView);

		fromView.BuiltInAnimation(TransitionType.TopOut, null, null, (float)_duration);
		toView.BuiltInAnimation(TransitionType.BottomIn, null, () => transitionContext.CompleteTransition(true), (float)_duration);

		//toView.Alpha = 0;
		//UIView.Animate(_duration, () =>
		//{
		//	toView.Alpha = 1;
		//}, () =>
		//{
		//	transitionContext.CompleteTransition(true);
		//});
	}

	public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
	{
		return _duration;
	}
}