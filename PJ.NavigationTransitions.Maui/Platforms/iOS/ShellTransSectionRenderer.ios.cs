using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

class ShellTransSectionRenderer : ShellSectionRenderer
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

		var info = AnimationHelpers.GetInfo(currentPage);

		var view = ViewController.View;

		Assert(view is not null);

		view.Layer.RemoveAllAnimations();

		view.BuiltInAnimation(info.AnimationOut, null, null, info.Duration);
		e.Animated = false;

		END:
		base.OnPopRequested(e);
	}

	protected override void OnPushRequested(NavigationRequestedEventArgs e)
	{
		if (currentPage is null || !e.Animated)
		{
			goto END;
		}

		var info = AnimationHelpers.GetInfo(currentPage);

		var view = ViewController.View!;

		view.Layer.RemoveAllAnimations();

		view.FlipAnimation(null, null, 1.5);
		e.Animated = info.AnimationIn != TransitionType.Default;

		END:
		base.OnPushRequested(e);
	}
}
