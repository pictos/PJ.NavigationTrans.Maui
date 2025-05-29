using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Controls;
using UIKit;

namespace PJ.NavigationTrans.Sample.Platforms.iOS;
static class MyAnimations
{
	public static void FadeAnimation(this UIView view, TaskCompletionSource? tcs, Action? complete, double duration = 1.0)
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

	public static void ConfigFlipAnimation(UIView view)
	{
		var m34 = (nfloat)(-1 * 0.001);
		var initialTransform = CATransform3D.Identity;
		initialTransform.M34 = m34;
		initialTransform = initialTransform.Rotate((nfloat)(1 * Math.PI * 0.5), 0.0f, 1.0f, 0.0f);

		view.Alpha = 0.0f;
		view.Layer.Transform = initialTransform;
	}

	public static void FlipAnimation(UIView view)
	{
		var m34 = (nfloat)(-1 * 0.001);
		view.Layer.AnchorPoint = new CGPoint((nfloat)0.5, 0.5f);
		var newTransform = CATransform3D.Identity;
		newTransform.M34 = m34;
		view.Layer.Transform = newTransform;
		view.Alpha = 1.0f;
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

	public static void ConfigScaleAnimation(UIView view)
	{
		view.Alpha = 0.0f;
		view.Transform = CGAffineTransform.MakeScale((nfloat)0.5, (nfloat)0.5);
	}

	public static void ScaleAnimation(UIView view)
	{
		view.Alpha = 1.0f;
		view.Transform = CGAffineTransform.MakeScale((nfloat)1.0, (nfloat)1.0);
	}

	public static void ScaleAnimation(this UIView view, TaskCompletionSource? tcs, Action? complete, double duration = 0.5)
	{
		view.Alpha = 0.0f;
		view.Transform = CGAffineTransform.MakeScale((nfloat)0.5, (nfloat)0.5);

		_ = MainThread.IsMainThread;

		UIView.Animate(duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			() =>
			{

				_ = MainThread.IsMainThread;
				view.Alpha = 1.0f;
				view.Transform = CGAffineTransform.MakeScale((nfloat)1.0, (nfloat)1.0);
			},
			() =>
			{

				_ = MainThread.IsMainThread;
				tcs?.TrySetResult();
				complete?.Invoke();
			}
		);
	}

}
