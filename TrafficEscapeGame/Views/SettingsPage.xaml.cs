using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsViewModel settingsViewModel;

	public SettingsPage()
	{
		settingsViewModel = new SettingsViewModel();
		BindingContext = settingsViewModel;
        InitializeComponent();
	}
	private async void OnBackToMainMenuClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
    }
}