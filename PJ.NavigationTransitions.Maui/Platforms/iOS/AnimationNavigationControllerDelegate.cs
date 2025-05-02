using ObjCRuntime;
using UIKit;

namespace PJ.NavigationTransitions.Maui;

sealed class AnimationNavigationControllerDelegate : UINavigationControllerDelegate
{
	internal static TransitionAnimator _animator = new TransitionAnimator();
	INavigationAwareness navInfo;

	public AnimationNavigationControllerDelegate(INavigationAwareness navInfo)
	{
		this.navInfo = navInfo;
	}

	public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
	{
		_animator.NavInfo = navInfo;
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
