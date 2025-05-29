using System.Runtime.CompilerServices;
using CoreAnimation;
using UIKit;

namespace PJ.NavigationTrans.Maui;
static partial class AnimationHelpers
{
	[OverloadResolutionPriority(0)]
	public static void SelectAndRunAnimation(this UIView view, TransitionType animation, double duration, TaskCompletionSource? tcs = null)
	{
		SelectAndRunAnimation(view, animation, duration, null, tcs);
	}

	[OverloadResolutionPriority(1)]
	public static void SelectAndRunAnimation(this UIView view, TransitionType animation, double duration, Action? complete = null)
	{
		SelectAndRunAnimation(view, animation, duration, complete, null);
	}

	static void SelectAndRunAnimation(UIView view, TransitionType animation, double duration, Action? complete, TaskCompletionSource? tcs)
	{
		ArgumentNullException.ThrowIfNull(view);

		switch (animation)
		{
			case TransitionType.FadeIn:
			case TransitionType.FadeOut:
			case TransitionType.LeftIn:
			case TransitionType.LeftOut:
			case TransitionType.RightIn:
			case TransitionType.RightOut:
			case TransitionType.TopIn:
			case TransitionType.TopOut:
			case TransitionType.BottomIn:
			case TransitionType.BottomOut:
				view.BuiltInAnimation(animation, tcs, complete, duration);
				break;
		}
	}

	public static void RunCustomAnimation(this UIView view, IosCustomAnimation customAnimation, TaskCompletionSource tcs, bool isIn = true)
	{
		var config = isIn ? customAnimation.ConfigurationIn : customAnimation.ConfigurationOut;
		var animation = isIn ? customAnimation.AnimationIn : customAnimation.AnimationOut;

		config?.Invoke(view);

		UIView.Animate(customAnimation.Duration, 0, UIViewAnimationOptions.CurveEaseInOut, () => animation(view), () => tcs.TrySetResult());
	}

	public static void RunCustomAnimation(this UIView view, IosCustomAnimation customAnimation, Action complete, bool isIn = true)
	{
		var config = isIn ? customAnimation.ConfigurationIn : customAnimation.ConfigurationOut;
		var animation = isIn ? customAnimation.AnimationIn : customAnimation.AnimationOut;

		config?.Invoke(view);

		UIView.Animate(customAnimation.Duration, 0, UIViewAnimationOptions.CurveEaseInOut, () => animation(view), complete);
	}

	public static void RunCustomAnimation(this UIView view, IosCustomAnimation customAnimation, bool isIn = true)
	{
		var config = isIn ? customAnimation.ConfigurationIn : customAnimation.ConfigurationOut;
		var animation = isIn ? customAnimation.AnimationIn : customAnimation.AnimationOut;

		config?.Invoke(view);
	}

	public static IosCustomAnimation ComputeCustomAnimation(this BindableObject bindable)
	{
		var animationIos = (IosCustomAnimation?)NavigationTrans.GetTransitions(bindable);

		Assert(animationIos is not null);

		return animationIos;
	}

	public static void BuiltInAnimation(this UIView view, TransitionType transition, TaskCompletionSource? tcs, Action? complete, double duration)
	{
		var trans = CATransition.CreateAnimation();

		trans.Duration = duration;
		trans.RemovedOnCompletion = true;
		trans.Type = CAAnimation.TransitionPush;

		switch (transition)
		{
			case TransitionType.RightIn:
			case TransitionType.LeftOut:
				trans.Subtype = CAAnimation.TransitionFromRight;
				break;
			case TransitionType.LeftIn:
			case TransitionType.RightOut:
				trans.Subtype = CAAnimation.TransitionFromLeft;
				break;
			case TransitionType.TopIn:
			case TransitionType.BottomOut:
				trans.Subtype = CAAnimation.TransitionFromBottom;
				break;
			case TransitionType.BottomIn:
			case TransitionType.TopOut:
				trans.Subtype = CAAnimation.TransitionFromTop;
				break;
			case TransitionType.FadeIn:
				trans.Subtype = CAAnimation.TransitionReveal;
				break;
			case TransitionType.FadeOut:
				trans.Subtype = CAAnimation.TransitionFade;
				break;
		}

		trans.AnimationStopped += (_, __) =>
		{
			tcs?.TrySetResult();
			complete?.Invoke();
		};

		view.Layer.AddAnimation(trans, null);
	}

}