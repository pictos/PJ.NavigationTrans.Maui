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
}

#if ANDROID
readonly record struct AnimationInfo(int AnimationId, AAnimation Animation);
#endif

record struct TransInfo(double Duration, TransitionType AnimationIn, TransitionType AnimationOut);