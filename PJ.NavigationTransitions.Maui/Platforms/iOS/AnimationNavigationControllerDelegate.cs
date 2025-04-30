using UIKit;

namespace PJ.NavigationTransitions.Maui;

sealed class AnimationNavigationControllerDelegate : UINavigationControllerDelegate
{
	static TransitionAnimator _animator = new TransitionAnimator();

	public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
	{
		return _animator;
	}
}
