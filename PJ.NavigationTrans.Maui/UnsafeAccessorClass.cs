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
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "RemoveViewController")]
	public static extern void UnsafeRemoveViewController(ShellSectionRenderer shellSectionRenderer, UIKit.UIViewController viewController);
#endif
}
