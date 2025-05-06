using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTrans.Maui;

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

	protected override void SwitchFragment(FragmentManager manager, Android.Views.View targetView, ShellItem newItem, bool animate = true)
	{
		var previousView = __currentView;
		var shellContent = newItem.CurrentItem.CurrentItem!;
		var transitionIn = ShellTrans.GetTransitionIn(shellContent);

		if (!animate || transitionIn == TransitionType.Default)
		{
			base.SwitchFragment(manager, targetView, newItem, false);

			ref var refView = ref UnsafeAccessorClass.GetSetUnsafeCurrentView(this);
			__currentView = refView;

			return;
		}

		var info = AnimationHelpers.GetInfo(shellContent);

		var duration = info.Duration;

		var animationIn = info.AnimationIn.ToPlatform(duration);
		var animationOut = info.AnimationOut.ToPlatform(duration);

		var fragmentTransaction = manager.BeginTransaction();

		__currentView = CreateShellItemRenderer(newItem);
		__currentView.ShellItem = newItem;

		var fragment = __currentView.Fragment;
		var oldFragment = previousView?.Fragment;
		fragmentTransaction.Add(targetView.Id, fragment);
		var runnableIn = new AnimationRunnable(fragment, animationIn.Animation);

		if (oldFragment is not null)
		{
			var runnableOut = new AnimationRunnable(oldFragment, animationOut.Animation);
			fragmentTransaction.RunOnCommit(runnableOut);
			runnableOut.OnComplete = (f) =>
			{
				fragmentTransaction.RemoveEx(f);
			};
		}

		fragmentTransaction.RunOnCommit(runnableIn);

		fragmentTransaction.CommitAllowingStateLoss();

		UnsafeAccessorClass.GetSetUnsafeCurrentView(this) = __currentView;
	}
}
