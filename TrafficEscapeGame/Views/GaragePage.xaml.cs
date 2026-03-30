namespace TrafficEscapeGame.Views;

public partial class GaragePage : ContentPage
{
    public GaragePage()
    {
        InitializeComponent();
    }

    private void OnSelectBlue(object sender, EventArgs e)
    {
        CarPreview.Source = "bluecar.png";
        StatusLabel.Text = "Currently Selected: Blue";
    }

    private void OnSelectRed(object sender, EventArgs e)
    {
        CarPreview.Source = "redcar.png";
        StatusLabel.Text = "Currently Selected: Red";
    }

    // Fixes the "not awaited" error by using await
    private async void OnMainMenuButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}