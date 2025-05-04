using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTrans.Maui;
public class ShellTransRenderer : ShellRenderer
{
	protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
	{
		return new ShellTransSectionRenderer(this);
	}

	protected override IShellItemTransition CreateShellItemTransition()
	{
		return new ShellItemTrans();
	}
}
