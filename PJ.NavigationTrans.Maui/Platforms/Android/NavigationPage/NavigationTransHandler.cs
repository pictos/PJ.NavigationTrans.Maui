using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace PJ.NavigationTrans.Platforms.Android.NavigationPage;

class NavigationTransHandler : NavigationViewHandler
{
	public NavigationTransHandler()
	{
	}
}


class X : NavigationViewFragment
{
	public override AAnimation OnCreateAnimation(int transit, bool enter, int nextAnim)
	{
		return base.OnCreateAnimation(transit, enter, nextAnim);
	}
}
