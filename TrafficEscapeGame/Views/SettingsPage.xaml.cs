using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage()
    {
        InitializeComponent();
        //set binding context to settings view model
        _viewModel = new SettingsViewModel();
        BindingContext = _viewModel;
    }
    //navigate back to main menu and save settings
    private async void OnBackToMainMenuClicked(object sender, EventArgs e)
    {
        await _viewModel.SaveSettings();
        await Navigation.PopAsync();
    }
    //
    private void OnSoundToggled(object sender, ToggledEventArgs e) =>
        _viewModel.OnSoundToggled();
}