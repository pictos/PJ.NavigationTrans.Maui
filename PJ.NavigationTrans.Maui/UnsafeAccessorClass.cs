using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTrans.Maui;

class UnsafeAccessorClass
{
#if ANDROID
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_currentView")]
	public static extern ref IShellItemRenderer? GetSetUnsafeCurrentView(ShellRenderer shell);
#elif IOS
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Current")]
	public static extern Page GetUnsafeCurrentPageProperty(NavigationRenderer nav);
#endif
}
