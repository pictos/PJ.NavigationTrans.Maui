using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public partial class ShellTransRenderer : ShellRenderer
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

		var context = Platform.AppContext ?? throw new NullReferenceException();

		var loadedAnimation = Android.Views.Animations.AnimationUtils.LoadAnimation(context, aAnimation)!;
		loadedAnimation.Duration = duration;

		var currentView = CreateShellItemRenderer(newItem);
		currentView.ShellItem = newItem;

		fragmentTransaction.SetCustomAnimations(aAnimation, aAnimation);
		var fragment = currentView.Fragment;

		fragmentTransaction.Add(targetView.Id, fragment);
		var runnable = new AnimationRunnable(fragment, loadedAnimation);
		fragmentTransaction.RunOnCommit(runnable);

		fragmentTransaction.CommitAllowingStateLoss();
	}

	sealed class AnimationRunnable : Java.Lang.Object, Java.Lang.IRunnable
	{
		readonly WeakWrapper<Fragment> fragmentWrapper;
		readonly WeakWrapper<Android.Views.Animations.Animation> animationWrapper;

		public AnimationRunnable(Fragment fragment, Android.Views.Animations.Animation animation)
		{
			fragmentWrapper = new(fragment);
			animationWrapper = new(animation);
		}

		public void Run()
		{
			var animation = animationWrapper.Target;

			if (animation is null)
				return;

			fragmentWrapper.Target?.View?.StartAnimation(animation);
		}
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
}
