using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

class UnsafeAccessorClass
{
#if ANDROID
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_currentView")]
	public static extern ref IShellItemRenderer? GetSetUnsafeCurrentView(ShellRenderer shell);
#endif
}
