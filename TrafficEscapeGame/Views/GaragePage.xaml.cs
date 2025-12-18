namespace TrafficEscapeGame.Views;

public partial class GaragePage : ContentPage
{
	public GaragePage()
	{
		InitializeComponent();
	}

    private async void OnMainMenuButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopToRootAsync();
    }
}