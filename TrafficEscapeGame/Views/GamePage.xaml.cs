using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class GamePage : ContentPage
{
    private GameViewModel viewModel;

    public GamePage()
	{
		InitializeComponent();
        viewModel = new GameViewModel();
        BindingContext = viewModel;
    }

    private void OnScoreClicked(object sender, EventArgs e)
    {
       viewModel.AddScore();
    }
}