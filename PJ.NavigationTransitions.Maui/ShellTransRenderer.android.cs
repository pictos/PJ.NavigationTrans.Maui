using System.Diagnostics;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public partial class ShellTransRenderer : ShellRenderer
{
	IShellItemRenderer? __currentView;
	public ShellTransRenderer()
	{
	}

	protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
	{
		return new ShellTransItemRenderer(this);
	}

	void SetFieldValue()
	{
		var fieldInfo = typeof(ShellRenderer).GetField("_currentView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		Debug.Assert(fieldInfo is not null);
		__currentView = (IShellItemRenderer)fieldInfo.GetValue(this)!;
	}

	protected override void SwitchFragment(FragmentManager manager, Android.Views.View targetView, ShellItem newItem, bool animate = true)
	{
		var previousView = __currentView;
		var shellContent = newItem.CurrentItem.CurrentItem!;
		var transitionIn = ShellTrans.GetTransitionIn(shellContent);

		if (!animate || transitionIn == TransitionType.Default)
		{
			base.SwitchFragment(manager, targetView, newItem, false);
			SetFieldValue();
			return;
		}
		var transitionOut = ShellTrans.GetTransitionOut(shellContent);
		var duration = ShellTrans.GetDuration(shellContent);

		var animationIn = transitionIn.ToPlatform(duration);
		var animationOut = transitionOut.ToPlatform(duration);

		var fragmentTransaction = manager.BeginTransaction();

		__currentView = CreateShellItemRenderer(newItem);
		__currentView.ShellItem = newItem;

		fragmentTransaction.SetCustomAnimations(animationIn.AnimationId, animationOut.AnimationId);
		var fragment = __currentView.Fragment;
		var oldFragment = previousView?.Fragment;
		fragmentTransaction.Add(targetView.Id, fragment);
		var runnableIn = new AnimationRunnable(fragment, animationIn.Animation);
		
		if (oldFragment is not null)
		{
			var runnableOut = new AnimationRunnable(oldFragment, animationOut.Animation);
			fragmentTransaction.RunOnCommit(runnableOut);
		}
		
		fragmentTransaction.RunOnCommit(runnableIn);

		fragmentTransaction.CommitAllowingStateLoss();
	}

	sealed class AnimationRunnable : Java.Lang.Object, Java.Lang.IRunnable
	{
		readonly WeakWrapper<Fragment> fragmentWrapper;
		readonly WeakWrapper<AAnimation> animationWrapper;

		public AnimationRunnable(Fragment fragment, AAnimation animation)
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
