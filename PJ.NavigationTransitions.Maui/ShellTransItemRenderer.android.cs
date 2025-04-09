using System.Diagnostics;
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PJ.NavigationTransitions.Maui;

public class ShellTransItemRenderer : ShellItemRenderer
{
	Dictionary<Element, IShellObservableFragment>? __fragmentMap;
	IShellObservableFragment? __currentFragment;

	public ShellTransItemRenderer(IShellContext context) : base(context)
	{
		AnimationHelpers.SetFieldValue<ShellItemRendererBase, Dictionary<Element, IShellObservableFragment>>(this, ref __fragmentMap, "_fragmentMap");
	}

	protected override void SetupAnimation(ShellNavigationSource navSource, FragmentTransaction t, Page page)
	{
		var duration = ShellTrans.GetDuration(page);
		var transactionIn = ShellTrans.GetTransitionIn(page);
		var transactionOut = ShellTrans.GetTransitionOut(page);

		var animationIn = transactionIn.ToPlatform(duration);
		var animationOut = transactionOut.ToPlatform(duration);
		AnimationHelpers.SetFieldValue<ShellItemRendererBase, IShellObservableFragment>(this, ref __currentFragment, "_currentFragment");

		IShellObservableFragment? observableFragment;
		Debug.Assert(__fragmentMap is not null);
		if (!__fragmentMap.TryGetValue(page, out observableFragment))
		{
			observableFragment = __fragmentMap[page] = CreateFragmentForPage(page);
		}

		var fragment = observableFragment!.Fragment;
		var oldFragment = __currentFragment?.Fragment;
		//t.Add(GetNavigationTarget().Id, fragment);
		if (oldFragment is not null)
		{
			var runnableOut = new AnimationRunnable(oldFragment, animationOut.Animation);
			t.RunOnCommit(runnableOut);
		}

		var runnableIn = new AnimationRunnable(fragment, animationIn.Animation);
		t.RunOnCommit(runnableIn);
	}
}
