namespace TrafficEscapeGame.Views;
//incomplete garage page
public partial class GaragePage : ContentPage
{
	public GaragePage()
	{
		InitializeComponent();
	}
    //navigate back to main menu
    private async void OnMainMenuButton_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopToRootAsync();
    }
}