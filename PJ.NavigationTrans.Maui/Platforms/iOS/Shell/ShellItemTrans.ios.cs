using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace PJ.NavigationTrans.Maui;
class ShellItemTrans : IShellItemTransition
{
	public Task Transition(IShellItemRenderer oldRenderer, IShellItemRenderer newRenderer)
	{
		var tcs = new TaskCompletionSource();
		var item = newRenderer.ShellItem;

		var section = item.CurrentItem;
		var content = section.CurrentItem;

		var oldView = oldRenderer.ViewController.View;
		var newView = newRenderer.ViewController.View;

		Assert(oldView is not null);
		Assert(newView is not null);

		var info = AnimationHelpers.GetInfo(content);

		if (info.AnimationIn == TransitionType.Default || info.AnimationOut == TransitionType.Default)
		{
			return DefaultImpl(oldRenderer, newRenderer);
		}

		oldView.Layer.RemoveAllAnimations();
		oldView.Superview!.InsertSubviewAbove(newView, oldView);

		var isInBuiltIn = info.AnimationIn.IsBuiltIn();
		var isOutBuiltIn = info.AnimationOut.IsBuiltIn();

		if (isInBuiltIn & isOutBuiltIn)
		{
			oldView.SelectAndRunAnimation(info.AnimationOut, info.Duration, tcs);
			newView.SelectAndRunAnimation(info.AnimationIn, info.Duration, tcs);
			goto END;
		}

		var iosCustomAnimation = content.ComputeCustomAnimation();

		if (isOutBuiltIn)
		{
			oldView.SelectAndRunAnimation(info.AnimationOut, info.Duration, tcs);
		}
		else
		{
			oldView.RunCustomAnimation(iosCustomAnimation, tcs, false);
		}

		if (isInBuiltIn)
		{
			newView.SelectAndRunAnimation(info.AnimationIn, info.Duration, tcs);
		}
		else
		{
			newView.RunCustomAnimation(iosCustomAnimation, tcs);
		}


		END:
		return tcs.Task;
	}


	static Task DefaultImpl(IShellItemRenderer oldRenderer, IShellItemRenderer newRenderer)
	{
		TaskCompletionSource task = new();
		var oldView = oldRenderer.ViewController.View;
		var newView = newRenderer.ViewController.View;

		Assert(newView is not null);

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