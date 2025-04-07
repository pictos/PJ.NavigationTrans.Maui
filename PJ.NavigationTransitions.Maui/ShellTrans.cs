namespace PJ.NavigationTransitions.Maui;
public static class ShellTrans
{
	public static readonly BindableProperty DurationProperty =
		BindableProperty.CreateAttached("Duration", typeof(int), typeof(ShellContent), 500);

	public static int GetDuration(ShellContent view) => (int)view.GetValue(DurationProperty);

	public static void SetDuration(ShellContent view, int value) => view.SetValue(DurationProperty, value);

	public static readonly BindableProperty TransitionProperty =
		BindableProperty.CreateAttached("Transition", typeof(TransitionType), typeof(ShellContent), TransitionType.Default);

	public static TransitionType GetTransition(ShellContent view) => (TransitionType)view.GetValue(TransitionProperty);

	public static void SetTransition(ShellContent view, TransitionType value) => view.SetValue(TransitionProperty, value);
}
