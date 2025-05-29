using System.Runtime.CompilerServices;

namespace PJ.NavigationTrans.Maui;

static partial class AnimationHelpers
{
	public static readonly Action EmptyAction = () => { };

	public static TransInfo GetInfo(BindableObject bindable)
	{
		var duration = NavigationTrans.GetDuration(bindable);

#if IOS
		duration /= 1_000;
#endif

		var animationIn = NavigationTrans.GetTransitionIn(bindable);
		var animationOut = NavigationTrans.GetTransitionOut(bindable);

		return new(duration, animationIn, animationOut);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsBuiltIn(this TransitionType type) => type != TransitionType.Custom;
}