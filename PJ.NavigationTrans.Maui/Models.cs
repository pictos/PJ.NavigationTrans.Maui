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
#endif

record struct TransInfo(double Duration, TransitionType AnimationIn, TransitionType AnimationOut);


public abstract record BaseCustomAnimation(double Duration);
