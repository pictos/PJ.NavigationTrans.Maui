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

		var oldView = oldRenderer.ViewController.View;
		var newView = newRenderer.ViewController.View;

		Debug.Assert(oldView is not null);
		Debug.Assert(newView is not null);

		var animIn = ShellTrans.GetTransitionIn(content);

		if (animIn == TransitionType.Default)
		{
			return DefaultImpl(oldRenderer, newRenderer);
		}

		oldView.Layer.RemoveAllAnimations();
		oldView.Superview!.InsertSubviewAbove(newView, oldView);

		var animOut = ShellTrans.GetTransitionOut(content);
		var duration = ShellTrans.GetDuration(content) / 1_000;

		oldView.SelectAndRunAnimation(animOut, duration, tcs);
		newView.SelectAndRunAnimation(animIn, duration, tcs);

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