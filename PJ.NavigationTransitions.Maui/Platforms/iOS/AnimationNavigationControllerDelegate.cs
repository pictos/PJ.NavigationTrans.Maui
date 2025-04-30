using ObjCRuntime;
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

//sealed class AnimationNavigationControllerDelegate : UIViewControllerTransitioningDelegate
//{
//	static TransitionAnimator _animator = new TransitionAnimator();

//	public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
//	{
//		return _animator;
//	}

//	public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
//	{
//		return _animator;
//	}

//}
