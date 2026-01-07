using System.Threading.Tasks;
using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class MainMenuPage : ContentPage
{
    private bool _isDarkMode = false;

    public MainMenuPage()
    {
        InitializeComponent();
        //set binding context to main menu view model
        BindingContext = new MainMenuViewModel();

    }


    //navigate to game page on start button click
    private async void OnStartGameBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }
    //navigate to settings page on settings button click
    private async void OnSettingsBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
    //navigate to garage page on garage button click
    private async void OnGarageBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GaragePage());
    }
    //toggle dark mode on dark mode button click
    private void OnDarkModeButtonClicked(object sender, EventArgs e)
    {
        if (_isDarkMode)
        {
            //switch to light mode
            Application.Current!.UserAppTheme = AppTheme.Light;
            _isDarkMode = false;
        }
        else
        {
            //switch to dark mode
            Application.Current!.UserAppTheme = AppTheme.Dark;
            _isDarkMode = true;
        }
    }

}