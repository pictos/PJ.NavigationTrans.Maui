using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform.Compatibility;
using UIKit;

namespace PJ.NavigationTransitions.Maui;

interface INavigationAwareness
{
	Page? CurrentPage { get; }
	IMauiContext MauiContext { get; }
}

class ShellTransSectionRenderer : ShellSectionRenderer, INavigationAwareness
{
	internal static bool isPush;
	IShellContext context;
	public Page? CurrentPage { get; private set; }

	public IMauiContext MauiContext { get; private set; } = default!;

	public ShellTransSectionRenderer(IShellContext context) : base(context)
	{
		this.context = context;
	}


	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		base.Delegate = null!;
		MauiContext = context.Shell.Handler!.MauiContext ?? throw new NullReferenceException("panic at the disco!");
		base.Delegate = new AnimationNavigationControllerDelegate(this);
		//base.TransitioningDelegate = new AnimationNavigationControllerDelegate();
		// Implement some property to control this
		//InteractivePopGestureRecognizer.Enabled = 
	}

	protected override void OnDisplayedPageChanged(Page page)
	{
		base.OnDisplayedPageChanged(page);

		// This will get the page that I need to observe and get values
		CurrentPage = page;
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
