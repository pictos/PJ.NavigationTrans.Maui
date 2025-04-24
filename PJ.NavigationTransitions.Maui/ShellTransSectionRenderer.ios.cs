using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace PJ.NavigationTransitions.Maui;
class ShellTransSectionRenderer : ShellSectionRenderer
{
	bool isPush;
	IShellContext context;
	Page? currentPage;

	public ShellTransSectionRenderer(IShellContext context) : base(context)
	{
		this.context = context;
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		// Implement some property to control this
		//InteractivePopGestureRecognizer.Enabled = 
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values
		currentPage = page;
	}

	public override UIViewController[] PopToRootViewController(bool animated)
	{
		var p = context.Shell.CurrentPage;
		Pop(animated);
		var r = base.PopToRootViewController(animated);

		p = context.Shell.CurrentPage;
		return r;

	}

	public override UIViewController PopViewController(bool animated)
	{
		Pop(animated);
		return base.PopViewController(!animated);
	}


	void Pop(bool animated)
	{
		if (!animated || View is null)
			return;


		var page = currentPage;
		var oldView = View.SnapshotView(false);
		var newView = View;

		Assert(oldView is not null);
		Assert(page is not null);

		newView.Layer.RemoveAllAnimations();

		newView.Superview?.AddSubview(oldView);

		var info = GetInfo(page);

		oldView.SelectAndRunAnimation(info.AnimationOut, info.Duration, () =>
		{
			oldView.Layer.RemoveAllAnimations();
			oldView.RemoveFromSuperview();
			oldView = null;
		});
		newView.SelectAndRunAnimation(info.AnimationIn, info.Duration);
	}

	static TransInfo GetInfo(BindableObject bindable)
	{
		var duration = ShellTrans.GetDuration(bindable);

#if IOS
		duration /= 1_000;
#endif

		var animationIn = ShellTrans.GetTransitionIn(bindable);
		var animationOut = ShellTrans.GetTransitionOut(bindable);

		return new(duration, animationIn, animationOut);
	}

	public override async void PushViewController(UIViewController viewController, bool animated)
	{
		if (!animated || View is null)
		{
			goto END;
		}

		var page = currentPage;
		var oldView = View.SnapshotView(false);
		var newView = viewController.View;

		Assert(oldView is not null);
		Assert(newView is not null);
		Assert(page is not null);

		View.Layer.RemoveAllAnimations();
		oldView.Layer.RemoveAllAnimations();
		newView.Layer.RemoveAllAnimations();

		View.AddSubview(newView);
		View.AddSubview(oldView);

		View.SendSubviewToBack(oldView);

		var info = GetInfo(page);

		var tcs = new TaskCompletionSource();

		newView.SelectAndRunAnimation(info.AnimationIn, info.Duration, tcs);
		oldView.SelectAndRunAnimation(info.AnimationOut, info.Duration, () =>
		{
			oldView.Layer.RemoveAllAnimations();
			oldView.RemoveFromSuperview();
			oldView = null;
		});

		await tcs.Task;

		END:
		base.PushViewController(viewController, false);

	}
}
