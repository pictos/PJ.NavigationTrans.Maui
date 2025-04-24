using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;
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
