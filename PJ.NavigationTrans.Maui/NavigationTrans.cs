using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PJ.NavigationTrans.Maui;
public static class NavigationTrans
{
	public static readonly BindableProperty DurationProperty =
		BindableProperty.CreateAttached("Duration", typeof(double), typeof(ShellContent), 500d);

	public static double GetDuration(BindableObject view) => (double)view.GetValue(DurationProperty);

	public static void SetDuration(BindableObject view, double value) => view.SetValue(DurationProperty, value);

	public static readonly BindableProperty TransitionInProperty =
		BindableProperty.CreateAttached("TransitionIn", typeof(TransitionType), typeof(ShellContent), TransitionType.Default);

	public static TransitionType GetTransitionIn(BindableObject view) => (TransitionType)view.GetValue(TransitionInProperty);

	public static void SetTransitionIn(BindableObject view, TransitionType value) => view.SetValue(TransitionInProperty, value);

	public static readonly BindableProperty TransitionOutProperty =
		BindableProperty.CreateAttached("TransitionOut", typeof(TransitionType), typeof(ShellContent), TransitionType.Default);

	public static TransitionType GetTransitionOut(BindableObject view) => (TransitionType)view.GetValue(TransitionOutProperty);
	public static void SetTransitionOut(BindableObject view, TransitionType value) => view.SetValue(TransitionOutProperty, value);

#if ANDROID
	public static void SetAndroidTransitions(BindableObject view, int transitionIn, int transitionOut, double duration)
	{
		RegisterCustomTransitions(view);
		var value = new AndroidCustomAnimation(duration, transitionIn, transitionOut);
		PropertyManager.Add(view, value);
	}
#endif

	static void RegisterCustomTransitions(BindableObject view)
	{
		SetTransitionIn(view, TransitionType.Custom);
		SetTransitionOut(view, TransitionType.Custom);
	}

	public static BaseCustomAnimation? GetAndroidTransitions(BindableObject view) => PropertyManager.Get(view);

}

static class PropertyManager
{
	static readonly Dictionary<BindableObject, BaseCustomAnimation> properties = [];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Add(BindableObject key, BaseCustomAnimation value) => 
		properties[key] = value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BaseCustomAnimation? Get(BindableObject key) =>
		properties.TryGetValue(key, out var value) ? value : null;
}