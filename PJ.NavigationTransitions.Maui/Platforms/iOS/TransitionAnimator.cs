using Microsoft.Maui.Platform;
using UIKit;

namespace PJ.NavigationTransitions.Maui;

sealed class TransitionAnimator : UIViewControllerAnimatedTransitioning
{
	const double _duration = 1.5;

	public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
	{
		var containerView = transitionContext.ContainerView;

		var toVc = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
		var fromVc = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

		var toView = toVc.View;
		var fromView = fromVc.View;

		if (toView is null || fromView is null)
		{
			return;
		}

		containerView.ClearSubviews();
		containerView.Layer.RemoveAllAnimations();
		toView.Layer.RemoveAllAnimations();

		if (ShellTransSectionRenderer.isPush)
		{
			containerView.AddSubview(fromView);
			containerView.AddSubview(toView);


			fromView.BuiltInAnimation(TransitionType.TopOut, null, () => fromView.RemoveFromSuperview(), _duration);
			toView.BuiltInAnimation(TransitionType.BottomIn, null, () => transitionContext.CompleteTransition(true), _duration);
		}
		else
		{
			containerView.AddSubview(toView);
			containerView.InsertSubview(fromView, 0);

			fromView.BuiltInAnimation(TransitionType.BottomOut, null, () => transitionContext.CompleteTransition(true), _duration);
			toView.BuiltInAnimation(TransitionType.TopIn, null, () => { fromView.RemoveFromSuperview(); transitionContext.CompleteTransition(true); }, _duration);
		}
	}

	public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
	{
		return _duration * 2;
	}
}