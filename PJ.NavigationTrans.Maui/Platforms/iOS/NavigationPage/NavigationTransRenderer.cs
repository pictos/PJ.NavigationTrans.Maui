using Microsoft.Maui.Controls.Handlers.Compatibility;
using UIKit;

namespace PJ.NavigationTrans.Platforms.iOS.NavigationPage;
sealed class NavigationTransRenderer : NavigationRenderer
{
	public NavigationTransRenderer()
	{
		
	}

	public override void PushViewController(UIViewController viewController, bool animated)
	{

		base.PushViewController(viewController, animated);
	}
}
