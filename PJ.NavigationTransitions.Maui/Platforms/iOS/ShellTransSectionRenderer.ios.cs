using Microsoft.Maui.Controls.Platform.Compatibility;

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
		Delegate = new AnimationNavigationControllerDelegate();
		// Implement some property to control this
		//InteractivePopGestureRecognizer.Enabled = 
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values
		currentPage = page;
	}
}
