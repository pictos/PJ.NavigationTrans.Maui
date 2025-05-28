using PJ.NavigationTrans.Maui;

namespace PJ.NavigationTrans.Sample;

public partial class NewPage1 : ContentPage
{
	TransitionType[] transitions = Enum.GetValues<TransitionType>();
	TransitionType animIn, animOut;
	public NewPage1()
	{
		InitializeComponent();

		ShellTrans.SetTransitionIn(this, TransitionType.BottomIn);
		ShellTrans.SetTransitionOut(this, TransitionType.TopOut);
		ShellTrans.SetDuration(this, 2500);

		animationIn.ItemsSource = animationOut.ItemsSource = transitions;

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

	void animationIn_SelectedIndexChanged(object sender, EventArgs e)
	{
		animIn = transitions[animationIn.SelectedIndex];
	}

	void animationOut_SelectedIndexChanged(object sender, EventArgs e)
	{
		animOut = transitions[animationOut.SelectedIndex];

	}

	void Button_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new TestTrans(animIn, animOut));
	}
}

class TestTrans : ContentPage
{
	public TestTrans(TransitionType animationIn, TransitionType animationOut)
	{
		ShellTrans.SetTransitionIn(this, animationIn);
		ShellTrans.SetTransitionOut(this, animationOut);
		ShellTrans.SetDuration(this, 2500);


		var tap = new TapGestureRecognizer();
		tap.Tapped += (_, __) =>
		{
			if (App.IsShell)
				Shell.Current.GoToAsync("..", true);
			else
				Navigation.PopAsync();
		};


		Content = new Label
		{
			Text = "Hello there"
		};

		this.Content.GestureRecognizers.Add(tap);

	}
}