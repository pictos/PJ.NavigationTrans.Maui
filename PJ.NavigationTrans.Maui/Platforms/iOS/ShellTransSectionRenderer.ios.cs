using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTrans.Maui;

sealed class ShellTransSectionRenderer : ShellSectionRenderer
{
	Page? currentPage;

	public ShellTransSectionRenderer(IShellContext context) : base(context)
	{
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values.
		// Shell.Current.CurrentPage doesn't give the correct page to access.
		currentPage = page;
	}

	protected override async void Dispose(bool disposing)
	{
		// Shell disposes this too early, when navigating between flyouts
		// so we add a delay to keep things around for more time.
		// Otherwise this will throw an exception
		await Task.Delay(TimeSpan.FromSeconds(2));
		base.Dispose(disposing);
	}

	protected override void OnPopRequested(NavigationRequestedEventArgs e)
	{
		if (currentPage is null || !e.Animated)
		{
			goto END;
		}

		CreateAndApplyAnimation(e);

		END:
		base.OnPopRequested(e);
	}

	void CreateAndApplyAnimation(NavigationRequestedEventArgs e)
	{
		Assert(currentPage is not null);
		var info = AnimationHelpers.GetInfo(currentPage);

		var animation = e.RequestType == NavigationRequestType.Push ? info.AnimationIn : info.AnimationOut;

		var view = ViewController.View;

		Assert(view is not null);

		view.Layer.RemoveAllAnimations();

		e.Animated = info.AnimationIn == TransitionType.Default;

		view.SelectAndRunAnimation(animation, info.Duration);
	}

	protected override void OnPushRequested(NavigationRequestedEventArgs e)
	{
		if (currentPage is null || !e.Animated)
		{
			goto END;
		}

		CreateAndApplyAnimation(e);

		END:
		base.OnPushRequested(e);
	}
}
