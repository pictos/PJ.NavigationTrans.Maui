using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Sample;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();

		ShellTrans.SetTransitionIn(this, TransitionType.BottomIn);
		ShellTrans.SetTransitionOut(this, TransitionType.BottomOut);
		ShellTrans.SetDuration(this, 500);


		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, __) =>
		{
			if (App.IsShell)
				Shell.Current.GoToAsync("..", true);
			else
				Navigation.PopAsync();
		};

		this.Content.GestureRecognizers.Add(tap);
	}
}