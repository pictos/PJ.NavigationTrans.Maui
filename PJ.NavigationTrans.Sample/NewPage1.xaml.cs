using PJ.NavigationTransitions.Maui;

namespace PJ.NavigationTrans.Sample;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();

		ShellTrans.SetTransitionIn(this, TransitionType.BottomIn);
		ShellTrans.SetTransitionOut(this, TransitionType.TopOut);
		ShellTrans.SetDuration(this, 5000);
	}
}