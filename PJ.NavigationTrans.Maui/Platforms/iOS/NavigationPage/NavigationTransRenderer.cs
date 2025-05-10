using Microsoft.Maui.Controls.Handlers.Compatibility;
using UIKit;

namespace PJ.NavigationTrans.Maui;

sealed class NavigationTransRenderer : NavigationRenderer
{
	bool ignorePopCall;
	public override void PushViewController(UIViewController viewController, bool animated)
	{
		if (UnsafeAccessorClass.GetUnsafeCurrentPageProperty(this) is Page currentPage)
		{
			var fromUIView = VisibleViewController.View;

			Assert(fromUIView is not null);

			animated = CreateAndApplyAnimation(currentPage, NavigationRequestType.Push, fromUIView);
		}

		base.PushViewController(viewController, animated);
	}

	public override UIViewController PopViewController(bool animated)
	{
		ignorePopCall = true;
		if (UnsafeAccessorClass.GetUnsafeCurrentPageProperty(this) is Page currentPage)
		{
			var fromUIView = VisibleViewController.View;

			Assert(fromUIView is not null);

			animated = CreateAndApplyAnimation(currentPage, NavigationRequestType.Pop, fromUIView);
		}

		var r = base.PopViewController(animated);

		return r;
	}

	protected override Task<bool> OnPopViewAsync(Page page, bool animated)
	{
		if (ignorePopCall)
		{
			ignorePopCall = false;
			return Task.FromResult(true);
		}

		var fromUIView = VisibleViewController.View;

		Assert(fromUIView is not null);

		animated = CreateAndApplyAnimation(page, NavigationRequestType.Pop, fromUIView);

		return base.OnPopViewAsync(page, animated);
	}

	bool CreateAndApplyAnimation(Page currentPage, NavigationRequestType navigationRequest, UIView currentView)
	{
		var info = AnimationHelpers.GetInfo(currentPage);

		if (info.AnimationIn == TransitionType.Default)
		{
			return true;
		}

		var toAnimation = navigationRequest == NavigationRequestType.Push ? info.AnimationIn : info.AnimationOut;
		var fromAnimation = navigationRequest != NavigationRequestType.Push ? info.AnimationIn : info.AnimationOut;

		var view = ViewController.View;

		Assert(view is not null);
		Assert(currentView is not null);

		var window = view.Window;

		Assert(window is not null);

		view.Layer.RemoveAllAnimations();
		currentView.Layer.RemoveAllAnimations();

		// If we use the Built-In animations, we don't add the `currentView` to the `window`.
		if (toAnimation.IsBuiltIn() || fromAnimation.IsBuiltIn())
		{
			view.SelectAndRunAnimation(toAnimation, info.Duration);
			goto END;
		}

		return navigationRequest == NavigationRequestType.Pop ? HandlePop() : HandlePush();

		END:
		return false;


		bool HandlePush()
		{
			window.InsertSubview(currentView, 0);
			currentView.SelectAndRunAnimation(fromAnimation, info.Duration, () =>
			{
				currentView.RemoveFromSuperview();
			});
			view.SelectAndRunAnimation(toAnimation, info.Duration);
			return false;
		}

		bool HandlePop()
		{
			// Insert the new view behind the current view
			window.InsertSubview(view, 0);

			// Start animations immediately
			view.SelectAndRunAnimation(fromAnimation, info.Duration);
			currentView.SelectAndRunAnimation(toAnimation, info.Duration, () =>
			{
				currentView.RemoveFromSuperview();
			});

			return false;
		}
	}
}


static class MemoryTest
{
	static readonly List<WeakReference<UIView>> weakReferences = [];

	public static int Count => weakReferences.Count;

	public static void Add(UIView obj)
	{
		foreach(var item in weakReferences)
		{
			if (item.TryGetTarget(out var target) && target == obj)
			{
				return;
			}
		}

		weakReferences.Add(new(obj));
		CleanUp();
	}

	static void CleanUp()
	{
		RunGC();
		for (var i = 0; i < weakReferences.Count; i++)
		{
			var item = weakReferences[i];

			if (item.TryGetTarget(out _))
			{
			}
			else
			{
				weakReferences.Remove(item);
			}
			RunGC();
		}
	}

	public static void IsAliveAsync()
	{
		RunGC();
		for (var i = 0; i < weakReferences.Count; i++)
		{
			var item = weakReferences[i];

			if (item.TryGetTarget(out var target))
			{
			}
			else
			{
				weakReferences.Remove(item);
			}
			RunGC();
		}
	}

	static void RunGC()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
	}
}