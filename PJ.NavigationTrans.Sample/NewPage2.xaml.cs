using PJ.NavigationTransitions.Maui;

namespace PJ.NavigationTrans.Sample;

public partial class NewPage2 : ContentPage
{
	public NewPage2()
	{
		InitializeComponent();

		ShellTrans.SetTransitionIn(this, TransitionType.ScaleIn);
		ShellTrans.SetTransitionOut(this, TransitionType.FlipOut);
		ShellTrans.SetDuration(this, 2000);

		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, __) => Shell.Current.GoToAsync("..", true);

		this.Content.GestureRecognizers.Add(tap);
	}
}