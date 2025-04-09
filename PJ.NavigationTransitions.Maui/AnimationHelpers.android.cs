using System.Diagnostics;
namespace PJ.NavigationTransitions.Maui;

static class AnimationHelpers
{
	public static int ToPlatform(this TransitionType transition) => transition switch
	{
		TransitionType.FadeIn => Resource.Animation.fade_in,
		TransitionType.FadeOut => Resource.Animation.fade_out,
		TransitionType.BottomIn => Resource.Animation.enter_bottom,
		TransitionType.BottomOut => Resource.Animation.exit_bottom,
		TransitionType.TopIn => Resource.Animation.enter_top,
		TransitionType.TopOut => Resource.Animation.exit_top,
		TransitionType.LeftIn => Resource.Animation.enter_left,
		TransitionType.LeftOut => Resource.Animation.exit_left,
		TransitionType.RightIn => Resource.Animation.enter_right,
		TransitionType.RightOut => Resource.Animation.exit_right,
		TransitionType.ScaleIn => Resource.Animation.scale_in,
		TransitionType.ScaleOut => Resource.Animation.scale_out,
		TransitionType.Default => Resource.Animation.none,
		_ => Resource.Animation.none,
	};

	public static AnimationInfo ToPlatform(this TransitionType transition, int duration)
	{
		var animation = transition.ToPlatform();
		var context = Platform.AppContext ?? throw new NullReferenceException();
		var loadedAnimation = AAnimationUtils.LoadAnimation(context, animation);
		Debug.Assert(loadedAnimation is not null);
		loadedAnimation.Duration = duration;
		return new (animation, loadedAnimation);
	}
}
