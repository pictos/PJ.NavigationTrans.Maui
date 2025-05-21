using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Microsoft.Maui.Platform;

namespace PJ.NavigationTrans;

// A lot of methods from .NET MAUI, that are internals
partial class Helpers
{
	
	public static LayoutInflater GetLayoutInflater(this IMauiContext mauiContext)
	{
		var layoutInflater = mauiContext.Services.GetService<LayoutInflater>();

		if (!layoutInflater!.IsAlive() && mauiContext.Context != null)
		{
			var activity = mauiContext.Context!.GetActivity();

			if (activity != null)
				layoutInflater = LayoutInflater.From(activity);
		}

		return layoutInflater ?? throw new InvalidOperationException("LayoutInflater Not Found");
	}
	public static bool IsAlive([NotNullWhen(true)] this Java.Lang.Object? obj)
	{
		if (obj == null)
			return false;

		return !obj.IsDisposed();
	}

	public static bool IsAlive([NotNullWhen(true)] this global::Android.Runtime.IJavaObject? obj)
	{
		if (obj == null)
			return false;

		return !obj.IsDisposed();
	}

	public static bool IsDisposed(this Java.Lang.Object obj)
	{
		return obj.Handle == IntPtr.Zero;
	}

	public static bool IsDisposed(this global::Android.Runtime.IJavaObject obj)
	{
		return obj.Handle == IntPtr.Zero;
	}
}
