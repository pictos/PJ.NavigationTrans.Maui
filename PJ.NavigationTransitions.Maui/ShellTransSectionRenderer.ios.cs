using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace PJ.NavigationTransitions.Maui;
class ShellTransSectionRenderer : ShellSectionRenderer
{
	bool isPush;
	IShellContext context;
	Page? displayedPage;
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
		displayedPage = page;
	}

	public override UIViewController[] PopToRootViewController(bool animated)
	{
		Pop(animated);
		return base.PopToRootViewController(animated);

	}

	public override UIViewController PopViewController(bool animated)
	{
		Pop(animated);
		return base.PopViewController(animated);
	}


	void Pop(bool animated)
	{
		if (!animated || View is null)
			return;


		var oldView = View.SnapshotView(false);
		var newView = View;
		_ = displayedPage;
		var p = context.Shell.CurrentPage;
	}

	public override void PushViewController(UIViewController viewController, bool animated)
	{
		base.PushViewController(viewController, animated);

		if (!animated)
			return;

		_ = displayedPage;
		var p = context.Shell.CurrentPage;
	}
}
