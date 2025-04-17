using System.Diagnostics;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace PJ.NavigationTransitions.Maui;
class ShellItemTrans : IShellItemTransition
{
	public Task Transition(IShellItemRenderer oldRenderer, IShellItemRenderer newRenderer)
	{
		var tcs = new TaskCompletionSource();
		var item = newRenderer.ShellItem;

		var section = item.CurrentItem;
		var content = section.CurrentItem;

		var oldView = oldRenderer.ViewController.View!;
		var newView = newRenderer.ViewController.View!;
		var animIn = ShellTrans.GetTransitionIn(content);
		var animOut = ShellTrans.GetTransitionOut(content);
		if (animIn == TransitionType.Default)
		{
			return DefaultImpl(oldRenderer, newRenderer);
		}

		oldView.Layer.RemoveAllAnimations();

		oldView.Superview!.InsertSubviewAbove(newView, oldView);

		var duration = ShellTrans.GetDuration(content) / 1_000;

		SelectAndRunAnimation(animOut, duration, tcs, oldView);
		SelectAndRunAnimation(animIn, duration, tcs, newView);
		return tcs.Task;
	}


	static void SelectAndRunAnimation(TransitionType animation, int duration, TaskCompletionSource? tcs, UIView view)
	{
		ArgumentNullException.ThrowIfNull(view);

		switch (animation)
		{
			case TransitionType.FadeIn:
			case TransitionType.FadeOut:
				view.FadeAnimation(tcs, duration);
				break;
			case TransitionType.ScaleIn:
			case TransitionType.ScaleOut:
				view.ScaleAnimation(tcs, duration);
				break;
			case TransitionType.LeftIn:
			case TransitionType.LeftOut:
			case TransitionType.RightIn:
			case TransitionType.RightOut:
			case TransitionType.TopIn:
			case TransitionType.TopOut:
			case TransitionType.BottomIn:
			case TransitionType.BottomOut:
				view.BuiltInAnimation(animation, tcs, duration);
				break;
		}
	}


	static Task DefaultImpl(IShellItemRenderer oldRenderer, IShellItemRenderer newRenderer)
	{
		TaskCompletionSource task = new();
		var oldView = oldRenderer.ViewController.View;
		var newView = newRenderer.ViewController.View;

		Debug.Assert(newView is not null);

		oldView?.Layer.RemoveAllAnimations();
		newView.Alpha = 0;

		oldView?.Superview?.InsertSubviewAbove(newView, oldView);

		UIView.Animate(0.5, 0, UIViewAnimationOptions.BeginFromCurrentState, () => newView.Alpha = 1, () =>
		{
			task.TrySetResult();
		});

		return task.Task;
	}
}


static class Animations
{
	public static void BuiltInAnimation(this UIView view, TransitionType transition, TaskCompletionSource? tcs, float duration)
	{
		var trans = CATransition.CreateAnimation();
		trans.Duration = duration;
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
		}

		trans.AnimationStopped += (_, __) =>
		{
			tcs?.TrySetResult();
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

	public static void FlipAnimation(this UIView view, TaskCompletionSource? tcs, double duration = 0.5)
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
			() => tcs?.TrySetResult()
		);
	}

	public static void ScaleAnimation(this UIView view, TaskCompletionSource? tcs, double duration = 0.5)
	{
		view.Alpha = 0.0f;
		view.Transform = CGAffineTransform.MakeScale((nfloat)0.5, (nfloat)0.5);

		UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			() =>
			{
				view.Alpha = 1.0f;
				view.Transform = CGAffineTransform.MakeScale((nfloat)1.0, (nfloat)1.0);
			},
			() => tcs?.TrySetResult()
		);
	}
}