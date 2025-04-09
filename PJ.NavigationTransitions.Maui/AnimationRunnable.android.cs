using AndroidX.Fragment.App;

namespace PJ.NavigationTransitions.Maui;


sealed class AnimationRunnable : Java.Lang.Object, Java.Lang.IRunnable
{
	readonly WeakWrapper<Fragment> fragmentWrapper;
	readonly WeakWrapper<AAnimation> animationWrapper;

	public AnimationRunnable(Fragment fragment, AAnimation animation)
	{
		fragmentWrapper = new(fragment);
		animationWrapper = new(animation);
	}

	public void Run()
	{
		var animation = animationWrapper.Target;

		if (animation is null)
			return;

		fragmentWrapper.Target?.View?.StartAnimation(animation);
	}
}

