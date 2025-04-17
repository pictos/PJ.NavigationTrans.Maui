using System.Diagnostics;
using AndroidX.Fragment.App;
using Android.Graphics.Drawables;
using AView = Android.Views.View;

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
		var loadedAnimation = Android.Views.Animations.AnimationUtils.LoadAnimation(context, animation);
		Debug.Assert(loadedAnimation is not null);
		loadedAnimation.Duration = duration;
		return new(animation, loadedAnimation);
	}
}

static class AndroidHelpers
{
	public static FragmentTransaction RemoveEx(this FragmentTransaction fragmentTransaction, Fragment fragment)
	{
		return fragmentTransaction.Remove(fragment);
	}

	public static FragmentTransaction AddEx(this FragmentTransaction fragmentTransaction, int containerViewId, Fragment fragment)
	{
		return fragmentTransaction.Add(containerViewId, fragment);
	}

	public static FragmentTransaction ReplaceEx(this FragmentTransaction fragmentTransaction, int containerViewId, Fragment fragment)
	{
		return fragmentTransaction.Replace(containerViewId, fragment);
	}

	public static FragmentTransaction HideEx(this FragmentTransaction fragmentTransaction, Fragment fragment)
	{
		return fragmentTransaction.Hide(fragment);
	}

	public static bool Contains(this FragmentManager fragmentManager, Fragment fragment)
	{
		// We can't trust `Fragment.IsAdded` due to the async nature of the fragment transactions
		// See: https://stackoverflow.com/questions/22485899/fragment-isadded-returns-false-on-an-already-added-fragment
		return fragmentManager.FindFragmentById(fragment.Id) != null;
	}

	public static FragmentTransaction ShowEx(this FragmentTransaction fragmentTransaction, Fragment fragment)
	{
		return fragmentTransaction.Show(fragment);
	}

	public static FragmentTransaction SetTransitionEx(this FragmentTransaction fragmentTransaction, int transit)
	{
		return fragmentTransaction.SetTransition(transit);
	}

	public static FragmentTransaction SetReorderingAllowedEx(this FragmentTransaction fragmentTransaction, bool reorderingAllowed)
	{
		return fragmentTransaction.SetReorderingAllowed(reorderingAllowed);
	}

	public static int CommitAllowingStateLossEx(this FragmentTransaction fragmentTransaction)
	{
		return fragmentTransaction.CommitAllowingStateLoss();
	}

	public static bool ExecutePendingTransactionsEx(this FragmentManager fragmentManager)
	{
		return fragmentManager.ExecutePendingTransactions();
	}

	public static FragmentTransaction BeginTransactionEx(this FragmentManager fragmentManager)
	{
		return fragmentManager.BeginTransaction();
	}

	public static void SetBackground(this AView view, Drawable? drawable)
	{
		view.Background = drawable;
	}
}