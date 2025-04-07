using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public class ShellTransItemRenderer : ShellItemRenderer
{
	public ShellTransItemRenderer(IShellContext context) : base(context)
	{
	}

	protected override void SetupAnimation(ShellNavigationSource navSource, FragmentTransaction t, Page page)
	{
		switch (navSource) 
		{
			case ShellNavigationSource.Push:
				//t.SetCustomAnimations(Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
				break;
			case ShellNavigationSource.Pop:
				//t.SetCustomAnimations(Resource.Animation.slide_in_left, Resource.Animation.slide_out_right);
				break;
			default:
				break;
		}
	}
}
