using System.Runtime.CompilerServices;

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsBuiltIn(this TransitionType type) => 
		!(type is TransitionType.ScaleIn or TransitionType.ScaleOut or TransitionType.FlipIn or TransitionType.FlipOut);
}
