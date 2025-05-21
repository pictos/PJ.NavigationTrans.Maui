#if ANDROID
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using AndroidX.Fragment.App;
#endif
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace PJ.NavigationTrans.Maui;

class UnsafeAccessorClass
{
#if ANDROID
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_currentView")]
	public static extern ref IShellItemRenderer? GetSetUnsafeCurrentView(ShellRenderer shell);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_FragmentNavigator")]
	public static extern FragmentNavigator GetUnsafeFragmentNavigator(StackNavigationManager stackNavigationManager);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_NavGraph")]
	public static extern NavGraph GetUnsafeNavGraph(StackNavigationManager stackNavigationManager);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_IsPopping")]
	public static extern ref Nullable<bool> GetUnsafeIsPopping(StackNavigationManager stackNavigationManager);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_fragmentManager")]
	public static extern ref FragmentManager GetSetUnsafeFragmentManager(StackNavigationManager stackNavigationManager);
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_fragmentContainerView")]
	public static extern ref FragmentContainerView GetSetUnsafeFragmentContainerView(StackNavigationManager stackNavigationManager);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_stackNavigationManager")]
	public static extern ref StackNavigationManager GetSetStackNavigationManager(NavigationViewHandler handler);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_NavigationManager")]
	public static extern StackNavigationManager GetStackNavigationManager(NavigationViewFragment stackNavigationManager);
#elif IOS
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_Current")]
	public static extern Page GetUnsafeCurrentPageProperty(NavigationRenderer nav);
#endif
}
