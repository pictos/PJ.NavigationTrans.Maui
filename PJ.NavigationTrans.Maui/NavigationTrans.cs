using System.Runtime.CompilerServices;
#if IOS
using UIKit; 
#endif

namespace PJ.NavigationTrans.Maui;

/// <summary>
/// Provides attached properties and methods for configuring navigation transitions in a <see cref="ShellContent"/>.
/// </summary>
/// <remarks>This class enables customization of navigation transitions, including duration and transition types, 
/// for views within a .NET MAUI application. It supports platform-specific transition configurations  for Android
/// and iOS, allowing developers to define custom animations.</remarks>
public static class NavigationTrans
{
	/// <summary>
	/// Identifies the attached property that specifies the duration, in milliseconds, for a given <see cref="ShellContent"/>.
	/// </summary>
	/// <remarks>
	///  This property is used to define the duration of an operation or animation associated with a <see cref="ShellContent"/>.
	///  The default value is 500 milliseconds.
	/// </remarks>
	public static readonly BindableProperty DurationProperty =
		BindableProperty.CreateAttached("Duration", typeof(double), typeof(ShellContent), 500d);

	/// <summary>
	/// Retrieves the duration value associated with the specified view.
	/// </summary>
	/// <param name="view">The <see cref="BindableObject"/> from which to retrieve the duration value.</param>
	/// <returns>The duration value as a <see cref="double"/>. Returns 0 if no value is set.</returns>
	public static double GetDuration(BindableObject view) => (double)view.GetValue(DurationProperty);

	/// <summary>
	/// Sets the duration value for the specified bindable object.
	/// </summary>
	/// <remarks>This method assigns the specified duration value to the bindable object's <see cref="DurationProperty"/>.
	/// Ensure that the <paramref name="value"/> is valid and appropriate for the context in
	/// which the bindable object is used.</remarks>
	/// <param name="view">The bindable object to which the duration value will be applied. Cannot be null.</param>
	/// <param name="value">The duration value to set, expressed as a double. Must be a non-negative value.</param>
	public static void SetDuration(BindableObject view, double value) => view.SetValue(DurationProperty, value);

	/// <summary>
	/// Identifies the attached property that specifies the transition type to be used when a  <see cref="ShellContent"/>
	/// is displayed.
	/// </summary>
	/// <remarks>This property allows developers to define custom transition effects for <see cref="ShellContent"/> 
	/// elements. The transition type can be set to one of the predefined values in the <see cref="TransitionType"/> 
	/// enumeration or a custom implementation.</remarks>
	public static readonly BindableProperty TransitionInProperty =
		BindableProperty.CreateAttached("TransitionIn", typeof(TransitionType), typeof(ShellContent), TransitionType.Default);

	/// <summary>
	/// Retrieves the transition type applied to the specified view when it enters the screen.
	/// </summary>
	/// <param name="view">The view for which the transition type is being retrieved. Cannot be null.</param>
	/// <returns>The <see cref="TransitionType"/> associated with the view's entry transition.</returns>
	public static TransitionType GetTransitionIn(BindableObject view) => (TransitionType)view.GetValue(TransitionInProperty);

	/// <summary>
	/// Sets the transition type to be applied when the specified view is displayed.
	/// </summary>
	/// <param name="view">The view to which the transition type will be applied. Must not be <see langword="null"/>.</param>
	/// <param name="value">The transition type to apply when the view is displayed.</param>
	public static void SetTransitionIn(BindableObject view, TransitionType value) => view.SetValue(TransitionInProperty, value);

	/// <summary>
	/// Identifies the attached property that specifies the transition type to be used when navigating away from a <see
	/// cref="ShellContent"/>.
	/// </summary>
	/// <remarks>This property allows developers to define custom transition effects for navigation when a <see
	/// cref="ShellContent"/> is exited. The default value is <see cref="TransitionType.Default"/>.</remarks>
	public static readonly BindableProperty TransitionOutProperty =
		BindableProperty.CreateAttached("TransitionOut", typeof(TransitionType), typeof(ShellContent), TransitionType.Default);

	/// <summary>
	/// Retrieves the transition type that is applied when the specified view transitions out.
	/// </summary>
	/// <param name="view">The <see cref="BindableObject"/> from which to retrieve the transition type.</param>
	/// <returns>The <see cref="TransitionType"/> representing the transition applied when the view transitions out.</returns>
	public static TransitionType GetTransitionOut(BindableObject view) => (TransitionType)view.GetValue(TransitionOutProperty);

	/// <summary>
	/// Sets the transition type to be applied when the specified view transitions out.
	/// </summary>
	/// <param name="view">The view to which the transition type will be applied. Cannot be null.</param>
	/// <param name="value">The transition type to set for the view. Specifies how the view transitions out.</param>
	public static void SetTransitionOut(BindableObject view, TransitionType value) => view.SetValue(TransitionOutProperty, value);

#if ANDROID
	/// <summary>
	/// Sets custom transition animations for an Android view.
	/// </summary>
	/// <remarks>This method registers custom transition animations for the specified Android view. The animations
	/// are defined by the provided resource IDs and duration.</remarks>
	/// <param name="view">The <see cref="BindableObject"/> representing the view to apply the transitions to. Cannot be null.</param>
	/// <param name="transitionIn">The animation resource ID for the transition-in effect.</param>
	/// <param name="transitionOut">The animation resource ID for the transition-out effect.</param>
	/// <param name="duration">The duration of the transition animations, in milliseconds. Must be greater than or equal to 0.</param>
	public static void SetAndroidTransitions(BindableObject view, int transitionIn, int transitionOut, double duration)
	{
		RegisterCustomTransitions(view);
		var value = new AndroidCustomAnimation(duration, transitionIn, transitionOut);
		PropertyManager.Add(view, value);
	}
#elif IOS
	/// <summary>
	/// Sets custom transition animations for an iOS view.
	/// </summary>
	/// <param name="view">The <see cref="Page"/> or <see cref="ShellContent"/> that will be animated.</param>
	/// <param name="animationIn">Code that describes animation for entering the page.</param>
	/// <param name="configurationIn">Code that describes the configuration for the view that will animated.</param>
	/// <param name="animationOut">Code that describes animation for leaving the page.</param>
	/// <param name="configurationOut">Code that describes the configuration for the view that will animated.</param>
	/// <param name="duration">Animation duration in milliseconds.</param>
	public static void SetIosTransitions(BindableObject view, Action<UIView> animationIn, Action<UIView>? configurationIn, Action<UIView> animationOut, Action<UIView>? configurationOut, double duration)
	{
		RegisterCustomTransitions(view);
		ArgumentNullException.ThrowIfNull(animationIn, nameof(animationIn));
		ArgumentNullException.ThrowIfNull(animationOut, nameof(animationOut));

		var value = new IosCustomAnimation(animationIn, configurationIn, animationOut, configurationOut, duration);
		PropertyManager.Add(view, value);
	}
#endif

	static void RegisterCustomTransitions(BindableObject view)
	{
		SetTransitionIn(view, TransitionType.Custom);
		SetTransitionOut(view, TransitionType.Custom);
	}

	/// <summary>
	/// Retrieves the custom animation transitions associated with the specified view.
	/// </summary>
	/// <param name="view">The <see cref="BindableObject"/> for which to retrieve the animation transitions. Cannot be <see langword="null"/>.</param>
	/// <returns>The <see cref="BaseCustomAnimation"/> object representing the animation transitions for the specified view,  or
	/// <see langword="null"/> if no transitions are associated with the view.</returns>
	public static BaseCustomAnimation? GetTransitions(BindableObject view) => PropertyManager.Get(view);

}

static file class PropertyManager
{
	static readonly ConditionalWeakTable<BindableObject, BaseCustomAnimation> properties = [];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Add(BindableObject key, BaseCustomAnimation value) =>
		properties.AddOrUpdate(key, value);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BaseCustomAnimation? Get(BindableObject key) =>
		properties.TryGetValue(key, out var value) ? value : null;
}
