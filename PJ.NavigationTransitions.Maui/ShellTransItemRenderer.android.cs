using System.Diagnostics;
using Android.Views;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public class ShellTransItemRenderer : ShellItemRenderer
{
	Dictionary<Element, IShellObservableFragment> __fragmentMap;
	public ShellTransItemRenderer(IShellContext context) : base(context)
	{
		var fieldInfo = typeof(ShellItemRendererBase).GetField("_fragmentMap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		Debug.Assert(fieldInfo is not null);
		__fragmentMap = (Dictionary<Element, IShellObservableFragment>)fieldInfo.GetValue(this)!;
		Debug.Assert(__fragmentMap is not null);
	}

	protected override void SetupAnimation(ShellNavigationSource navSource, FragmentTransaction t, Page page)
	{
		base.SetupAnimation(navSource, t, page);
		var animationIn = ShellTrans.GetTransitionIn(page);
		var animationOut = ShellTrans.GetTransitionOut(page);
		var duration = ShellTrans.GetDuration(page);

		var animIn = animationIn.ToPlatform(duration);
		var animOut = animationOut.ToPlatform(duration);


		Debug.Assert(animIn.Animation.Duration == duration);
		Debug.Assert(animOut.Animation.Duration == duration);

		if (!__fragmentMap.ContainsKey(page))
			__fragmentMap[page] = CreateFragmentForPage(page);


		var fragment = __fragmentMap[page].Fragment;


		t.RunOnCommit(new NavigationAnimationListener(fragment, ParentFragment!, animIn.Animation, animOut.Animation));
	}

	internal ViewGroup NavigationTarget() => this.GetNavigationTarget();


	sealed class NavigationAnimationListener : Java.Lang.Object, Java.Lang.IRunnable
	{
		readonly WeakWrapper<Fragment> _fragmentRef;
		readonly WeakWrapper<Fragment> _parentFragmentRef;
		readonly AAnimation _enterAnimation;
		readonly AAnimation _exitAnimation;

		public NavigationAnimationListener(
			Fragment fragment,
			Fragment parentFragment,
			AAnimation enterAnimation,
			AAnimation exitAnimation)
		{
			_fragmentRef = new(fragment);
			_parentFragmentRef = new(parentFragment);
			_enterAnimation = enterAnimation;
			_exitAnimation = exitAnimation;
		}

		public void Run()
		{

			if (_fragmentRef.Target is not Fragment fragment)
				return;

			var parent = _parentFragmentRef.Target;

			// Find any visible fragments that should be animated out
			if (parent is not null)
			{
				parent.View!.StartAnimation(_exitAnimation);
			}

			// Apply enter animation
			fragment.View!.StartAnimation(_enterAnimation);
		}
	}
}
