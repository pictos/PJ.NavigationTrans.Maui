using System.Runtime.CompilerServices;
using CoreAnimation;
using CoreGraphics;
using UIKit;

namespace PJ.NavigationTransitions.Maui;
static partial class AnimationHelpers
{
	[OverloadResolutionPriority(0)]
	public static void SelectAndRunAnimation(this UIView view, TransitionType animation, int duration, TaskCompletionSource? tcs = null)
	{
		SelectAndRunAnimation(view, animation, duration, null, tcs);
	}


	[OverloadResolutionPriority(1)]
	public static void SelectAndRunAnimation(this UIView view, TransitionType animation, int duration, Action? complete = null)
	{
		SelectAndRunAnimation(view, animation, duration, complete, null);
	}

	static void SelectAndRunAnimation(UIView view, TransitionType animation, int duration, Action? complete, TaskCompletionSource? tcs)
	{
		ArgumentNullException.ThrowIfNull(view);

		switch (animation)
		{
			case TransitionType.ScaleOut:
			case TransitionType.ScaleIn:
				view.ScaleAnimation(tcs, complete, duration);
				break;
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

	public static void BuiltInAnimation(this UIView view, TransitionType transition, TaskCompletionSource? tcs, Action? complete, double duration)
	{
		var trans = CATransition.CreateAnimation();
		trans.Duration = duration;
		trans.FadeInDuration = 0f;
		trans.FadeOutDuration = 0f;
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
			case TransitionType.BottomIn:
			case TransitionType.TopOut:
				trans.Subtype = CAAnimation.TransitionFromBottom;
				break;
			case TransitionType.TopIn:
			case TransitionType.BottomOut:
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

	public static void FadeAnimation(this UIView view, TaskCompletionSource? tcs, double duration = 1.0)
	{
		view.Alpha = 0.0f;
		view.Transform = CGAffineTransform.MakeIdentity();

		UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			() =>
			{
				view.Alpha = 1.0f;
			},
			() => tcs?.TrySetResult()
		);
	}

	public static void FlipAnimation(this UIView view, TaskCompletionSource? tcs, Action? complete, double duration = 0.5)
	{
		var m34 = (nfloat)(-1 * 0.001);
		var initialTransform = CATransform3D.Identity;
		initialTransform.M34 = m34;
		initialTransform = initialTransform.Rotate((nfloat)(1 * Math.PI * 0.5), 0.0f, 1.0f, 0.0f);

		view.Alpha = 0.0f;
		view.Layer.Transform = initialTransform;
		UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			() =>
			{
				view.Layer.AnchorPoint = new CGPoint((nfloat)0.5, 0.5f);
				var newTransform = CATransform3D.Identity;
				newTransform.M34 = m34;
				view.Layer.Transform = newTransform;
				view.Alpha = 1.0f;
			},
			() =>
			{
				tcs?.TrySetResult();
				complete?.Invoke();
			}
		);
	}

	public static void ScaleAnimation(this UIView view, TaskCompletionSource? tcs, Action? complete, double duration = 0.5)
	{
		view.Alpha = 0.0f;
		view.Transform = CGAffineTransform.MakeScale((nfloat)0.5, (nfloat)0.5);

		UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			() =>
			{
				view.Alpha = 1.0f;
				view.Transform = CGAffineTransform.MakeScale((nfloat)1.0, (nfloat)1.0);
			},
			() =>
			{
				tcs?.TrySetResult();
				complete?.Invoke();
			}
		);
	}
}
