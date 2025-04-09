using PJ.NavigationTransitions.Maui;

namespace PJ.NavigationTrans.Sample;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();

		ShellTrans.SetTransitionIn(this, TransitionType.TopIn);
		ShellTrans.SetTransitionOut(this, TransitionType.BottomOut);
		ShellTrans.SetDuration(this, 200);


		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, __) => Shell.Current.GoToAsync("..", true);

		this.Content.GestureRecognizers.Add(tap);
	}
}