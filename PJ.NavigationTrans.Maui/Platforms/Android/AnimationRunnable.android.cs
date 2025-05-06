using AndroidX.Fragment.App;

namespace PJ.NavigationTrans.Maui;

sealed class AnimationRunnable : Java.Lang.Object, Java.Lang.IRunnable
{
	readonly WeakReference<Fragment> fragmentWrapper;
	readonly WeakReference<AAnimation> animationWrapper;
	public Action<Fragment>? Finished { get; set; }

	public AnimationRunnable(Fragment fragment, AAnimation animation)
	{
		fragmentWrapper = new(fragment);
		animationWrapper = new(animation);
	}

	public void Run()
	{
		var animation = animationWrapper.GetTargetOrDefault();

		if (animation is null)
			return;

		var fragment = fragmentWrapper.GetTargetOrDefault();

		if (fragment is null)
			return;

		fragment.View?.StartAnimation(animation);

		Finished?.Invoke(fragment);
	}
}

