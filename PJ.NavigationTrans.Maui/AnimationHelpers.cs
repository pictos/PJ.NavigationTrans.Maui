namespace PJ.NavigationTrans.Maui;

static partial class AnimationHelpers
{
	public static TransInfo GetInfo(BindableObject bindable)
	{
		var duration = ShellTrans.GetDuration(bindable);

#if IOS
		duration /= 1_000;
#endif

		var animationIn = ShellTrans.GetTransitionIn(bindable);
		var animationOut = ShellTrans.GetTransitionOut(bindable);

		return new(duration, animationIn, animationOut);
	}
}
