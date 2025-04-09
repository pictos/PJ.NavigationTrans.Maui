namespace PJ.NavigationTransitions.Maui;


readonly struct WeakWrapper<T> where T : class
{
	WeakReference<T> reference { get; }
	public WeakWrapper(T value)
	{
		reference = new WeakReference<T>(value);
	}
	public T? Target => reference.TryGetTarget(out var target) ? target : default;
}

