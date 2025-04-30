using Microsoft.Maui.Platform;
using UIKit;

namespace PJ.NavigationTransitions.Maui;

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