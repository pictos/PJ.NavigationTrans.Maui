using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace PJ.NavigationTransitions.Maui;
class ShellTransSectionRenderer : ShellSectionRenderer
{
	internal static bool isPush;
	IShellContext context;
	Page? currentPage;

	public ShellTransSectionRenderer(IShellContext context) : base(context)
	{
		this.context = context;
	}


	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		base.Delegate = null!;
		
		base.Delegate = new AnimationNavigationControllerDelegate();
		//base.TransitioningDelegate = new AnimationNavigationControllerDelegate();
		// Implement some property to control this
		//InteractivePopGestureRecognizer.Enabled = 
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values
		currentPage = page;
	}
	protected override async void Dispose(bool disposing)
	{

		await Task.Delay(TimeSpan.FromSeconds(2));
		base.Dispose(disposing);
	}

	protected override void OnPopRequested(NavigationRequestedEventArgs e)
	{
		isPush = false;
		base.OnPopRequested(e);
	}

	protected override void OnPushRequested(NavigationRequestedEventArgs e)
	{
		isPush = true;
		base.OnPushRequested(e);

		//UnsafeAccessorClass.UnsafeRemoveViewController(this, ViewController);
	}
}
