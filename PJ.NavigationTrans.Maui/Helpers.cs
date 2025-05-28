namespace PJ.NavigationTrans;
static partial class Helpers
{
	public static T? GetTargetOrDefault<T>(this WeakReference<T> weak)
		where T : class
	{
		weak.TryGetTarget(out var target);
		return target;
	}
}