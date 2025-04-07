using System.Diagnostics;
using Android.Animation;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public class ShellTransRenderer : ShellRenderer
{

	public ShellTransRenderer()
	{
		//Element.Navigating += OnElementNavigating;
	}

	void OnElementNavigating(object? sender, ShellNavigatingEventArgs e)
	{

	}

	protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
	{
		return new ShellTransItemRenderer(this);
	}

	protected override void SwitchFragment(FragmentManager manager, Android.Views.View targetView, ShellItem newItem, bool animate = true)
	{
		var shellContent = newItem.CurrentItem.CurrentItem!;
		var animation = ShellTrans.GetTransition(shellContent);

		if (!animate || animation == TransitionType.Default)
		{
			base.SwitchFragment(manager, targetView, newItem, false);
			return;
		}

		var duration = ShellTrans.GetDuration(shellContent);

		var fragmentTransaction = manager.BeginTransaction();
		var aAnimation = animation.ToPlatform();

		

		var currentView = CreateShellItemRenderer(newItem);
		currentView.ShellItem = newItem;

		fragmentTransaction.SetCustomAnimations(aAnimation, aAnimation);
		var fragment = currentView.Fragment;
		fragmentTransaction.Add(targetView.Id, fragment);

		Task.Run(async () =>
		{
			await Task.Delay(5_000);
			var transaction = manager.BeginTransaction();
			transaction.Replace(fragment.Id, fragment);
			transaction.CommitAllowingStateLoss();
		});

		fragmentTransaction.CommitAllowingStateLoss();
	}
}

static class AnimationHelpers
{
	public static int ToPlatform(this TransitionType transition) => transition switch
	{
		TransitionType.FadeIn => Resource.Animation.fade_in,
		TransitionType.FadeOut => Resource.Animation.fade_out,
		TransitionType.BottomIn => Resource.Animation.enter_bottom,
		TransitionType.BottomOut => Resource.Animation.exit_bottom,
		TransitionType.TopIn => Resource.Animation.enter_top,
		TransitionType.TopOut => Resource.Animation.exit_top,
		TransitionType.LeftIn => Resource.Animation.enter_left,
		TransitionType.LeftOut => Resource.Animation.exit_left,
		TransitionType.RightIn => Resource.Animation.enter_right,
		TransitionType.RightOut => Resource.Animation.exit_right,
		TransitionType.ScaleIn => Resource.Animation.scale_in,
		TransitionType.ScaleOut => Resource.Animation.scale_out,
		TransitionType.Default => Resource.Animation.none,
		_ => Resource.Animation.none,
	};

	public static ObjectAnimator CreateObjectAnimator(this Android.Views.View targetView, TransitionType transition, bool isEntering, int duration)
	{
		ObjectAnimator? animator = null;

		switch (transition)
		{
			case TransitionType.FadeIn:
			case TransitionType.FadeOut:
				animator = ObjectAnimator.OfFloat(targetView, "alpha", isEntering ? 0f : 1f, isEntering ? 1f : 0f);
				break;
			case TransitionType.ScaleIn:
			case TransitionType.ScaleOut:
				animator = ObjectAnimator.OfFloat(targetView, "scaleX", isEntering ? 0f : 1f, isEntering ? 1f : 0f)!;
				animator.SetDuration(duration);
				animator.Start();
				animator = ObjectAnimator.OfFloat(targetView, "scaleY", isEntering ? 0f : 1f, isEntering ? 1f : 0f);
				break;
				// Add other cases for different transitions if needed
		}

		Debug.Assert(animator is not null);

		animator.SetDuration(duration);
		animator.Start();

		return animator;
	}
}
