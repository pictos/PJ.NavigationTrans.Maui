namespace PJ.NavigationTransitions.Maui;
public static class ShellTrans
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
}
