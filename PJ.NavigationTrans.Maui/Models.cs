#if IOS
using UIKit; 
#endif

namespace PJ.NavigationTrans.Maui;

public enum TransitionType
{
	Default,
	FadeIn,
	FadeOut,
	LeftIn, LeftOut,
	RightIn,
	RightOut,
	TopIn,
	TopOut,
	BottomIn,
	BottomOut,
	Custom
}

#if ANDROID
readonly record struct AnimationInfo(int AnimationId, AAnimation Animation);
record AndroidCustomAnimation(double Duration, int AnimationIn, int AnimationOut) : BaseCustomAnimation(Duration);
#elif IOS
record IosCustomAnimation(Action<UIView> AnimationIn, Action<UIView>? ConfigurationIn, Action<UIView> AnimationOut, Action<UIView>? ConfigurationOut, double Duration) : BaseCustomAnimation(Duration)
{
	public new double Duration => base.Duration / 1000;
}
#endif

record struct TransInfo(double Duration, TransitionType AnimationIn, TransitionType AnimationOut);


public abstract record BaseCustomAnimation(double Duration);
