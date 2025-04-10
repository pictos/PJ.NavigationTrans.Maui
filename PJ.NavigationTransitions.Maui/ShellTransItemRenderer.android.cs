using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public class ShellTransItemRenderer : ShellItemRenderer
{
	Dictionary<Element, IShellObservableFragment> fragmentMap = [];
	IShellObservableFragment? currentFragment;

	public ShellTransItemRenderer(IShellContext context) : base(context)
	{
		//AnimationHelpers.SetFieldValue<ShellItemRendererBase, Dictionary<Element, IShellObservableFragment>>(this, ref __fragmentMap, "_fragmentMap");
	}

	// Use this method to setup animation
	protected override Task<bool> HandleFragmentUpdate(ShellNavigationSource navSource, ShellSection shellSection, Page page, bool animated)
	{
		//base.HandleFragmentUpdate
		var result = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

		var isForCurrentTab = shellSection == ShellSection;
		var initialUpdate = fragmentMap.Count is 0;

		fragmentMap.TryAdd(ShellSection, GetOrCreateFragmentForTab(ShellSection));

		switch (navSource)
		{
			case ShellNavigationSource.Push:
			{
				fragmentMap.TryAdd(page, CreateFragmentForPage(page));
				if (!isForCurrentTab)
				{
					return Task.FromResult(true);
				}
				break;
			}
			case ShellNavigationSource.Pop:
			{
				if (fragmentMap.TryGetValue(page, out var frag))
				{
					if (!isForCurrentTab && ChildFragmentManager.Contains(frag.Fragment))
					{
						RemoveFragment(frag.Fragment);
					}
					fragmentMap.Remove(page);
				}

				if (!isForCurrentTab)
				{
					return Task.FromResult(true);
				}
				break;
			}
			case ShellNavigationSource.PopToRoot:
			{
				RemoveAllPushedPages(shellSection, isForCurrentTab);
				if (!isForCurrentTab)
				{
					return Task.FromResult(true);
				}
				break;
			}
			case ShellNavigationSource.Insert:
			{
				if (!isForCurrentTab)
				{
					return Task.FromResult(true);
				}
				break;
			}
			case ShellNavigationSource.Remove:
			{
				if (fragmentMap.TryGetValue(page, out var frag))
				{
					if (!isForCurrentTab && frag != currentFragment && ChildFragmentManager.Contains(frag.Fragment))
					{
						RemoveFragment(frag.Fragment);
					}
					fragmentMap.Remove(page);
				}
				if (!isForCurrentTab)
				{
					return Task.FromResult(true);
				}
				break;
			}
			case ShellNavigationSource.ShellSectionChanged:
				break;
			default:
				throw new InvalidOperationException("Unexpected navigation type");
		}

		var stack = ShellSection.Stack;
		Element? targetElement = null;
		IShellObservableFragment? target = null;

		if (stack.Count == 1 || navSource == ShellNavigationSource.PopToRoot)
		{
			target = fragmentMap[ShellSection];
			targetElement = ShellSection;
		}
		else
		{
			targetElement = stack[^1];
			fragmentMap.TryAdd(targetElement, CreateFragmentForPage(target as Page));
			target = fragmentMap[targetElement];
		}

		if (navSource == ShellNavigationSource.ShellSectionChanged)
		{
			RemoveAllButCurrent(target.Fragment);
		}

		if (target == currentFragment)
		{
			return Task.FromResult(true);
		}

		var t = ChildFragmentManager.BeginTransactionEx();

		var destinationFragment = currentFragment?.Fragment;
		var fragmentOrigin = target.Fragment;

		
		if (animated)
		{
			SetupAnimationImpl(navSource, t, page, destinationFragment, fragmentOrigin);
		}

		IShellObservableFragment? trackFragment = null;

		switch (navSource)
		{
			case ShellNavigationSource.Push:
			{
				trackFragment = target;

				if (currentFragment is not null)
				{
					t.HideEx(currentFragment.Fragment);
				}

				if (!ChildFragmentManager.Contains(target.Fragment))
				{
					t.AddEx(GetNavigationTarget().Id, target.Fragment);
				}

				t.ShowEx(target.Fragment);
				break;
			}

			case ShellNavigationSource.ShellSectionChanged:
			{
				if (currentFragment is not null)
				{
					t.HideEx(currentFragment.Fragment);
				}

				if (!ChildFragmentManager.Contains(target.Fragment))
				{
					t.AddEx(GetNavigationTarget().Id, target.Fragment);
				}

				t.ShowEx(target.Fragment);

				break;
			}

			case ShellNavigationSource.Pop:
			case ShellNavigationSource.PopToRoot:
			case ShellNavigationSource.Remove:
			{
				trackFragment = currentFragment;

				if (currentFragment is not null)
				{
					t.HideEx(currentFragment.Fragment);
				}

				if (!ChildFragmentManager.Contains(target.Fragment))
				{
					t.AddEx(GetNavigationTarget().Id, target.Fragment);
				}

				t.ShowEx(target.Fragment);

				break;
			}
		}
		t.CommitAllowingStateLossEx();

		if (animated && trackFragment is not null)
		{
			//GetNavigationTarget().SetBackgroundColor(Colors.Black.ToPlatform());
			trackFragment.AnimationFinished += CallBack;
		}
		else
		{
			result.TrySetResult(true);
		}

		if (initialUpdate)
		{
			t.SetReorderingAllowedEx(true);
		}

		currentFragment = target;

		return result.Task;


		void CallBack(object? s, EventArgs e)
		{
			trackFragment.AnimationFinished -= CallBack;
			result.TrySetResult(true);
			GetNavigationTarget().SetBackground(null);
		}
	}


	void SetupAnimationImpl(ShellNavigationSource navSource, FragmentTransaction t, Page page, Fragment? destination, Fragment? originFragment)
	{
		var duration = ShellTrans.GetDuration(page);
		var transactionIn = ShellTrans.GetTransitionIn(page);
		var transactionOut = ShellTrans.GetTransitionOut(page);

		var animationIn = transactionIn.ToPlatform(duration);
		var animationOut = transactionOut.ToPlatform(duration);


		//if (navSource is ShellNavigationSource.Pop or ShellNavigationSource.PopToRoot or ShellNavigationSource.Remove)
		//{
		//	(oldFragment, fragment) = (fragment, oldFragment);
		//}

		if (destination is not null)
		{
			var runnableOut = new AnimationRunnable(destination, animationOut.Animation);
			t.RunOnCommit(runnableOut);
		}

		if (originFragment is not null)
		{
			var runnableIn = new AnimationRunnable(originFragment, animationIn.Animation);
			t.RunOnCommit(runnableIn); 
		}
	}

	protected override void SetupAnimation(ShellNavigationSource navSource, FragmentTransaction t, Page page)
	{
		var duration = ShellTrans.GetDuration(page);
		var transactionIn = ShellTrans.GetTransitionIn(page);
		var transactionOut = ShellTrans.GetTransitionOut(page);

		var animationIn = transactionIn.ToPlatform(duration);
		var animationOut = transactionOut.ToPlatform(duration);

		if (!fragmentMap.TryGetValue(page, out var observableFragment))
		{
			observableFragment = fragmentMap[page] = CreateFragmentForPage(page);
		}

		var fragment = observableFragment.Fragment;
		var oldFragment = currentFragment?.Fragment;

		if (oldFragment is not null)
		{
			var runnableOut = new AnimationRunnable(oldFragment, animationOut.Animation);
			t.RunOnCommit(runnableOut);
		}

		var runnableIn = new AnimationRunnable(fragment, animationIn.Animation);
		t.RunOnCommit(runnableIn);
	}

	void RemoveFragment(Fragment fragment)
	{
		var t = ChildFragmentManager.BeginTransactionEx();
		t.RemoveEx(fragment);
		t.CommitAllowingStateLossEx();
	}

	void RemoveAllPushedPages(ShellSection shellSection, bool keepCurrent)
	{
		if (shellSection.Stack.Count <= 1 || (keepCurrent && shellSection.Stack.Count == 2))
			return;

		var t = ChildFragmentManager.BeginTransactionEx();

		foreach (var kvp in fragmentMap.ToArray())
		{
			if (kvp.Key.Parent != shellSection)
				continue;

			fragmentMap.Remove(kvp.Key);

			if (keepCurrent && kvp.Value.Fragment == currentFragment)
				continue;

			t.RemoveEx(kvp.Value.Fragment);
		}

		t.CommitAllowingStateLossEx();
	}

	void RemoveAllButCurrent(Fragment skip)
	{
		FragmentTransaction? trans = null;
		foreach (var kvp in fragmentMap)
		{
			var f = kvp.Value.Fragment;
			if (kvp.Value == currentFragment || kvp.Value.Fragment == skip || !f.IsAdded)
				continue;

			trans ??= ChildFragmentManager.BeginTransactionEx();
			trans.Remove(f);
		}
		;

		trans?.CommitAllowingStateLossEx();
	}

}
