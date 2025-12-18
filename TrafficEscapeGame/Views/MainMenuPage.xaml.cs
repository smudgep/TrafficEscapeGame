using System.Threading.Tasks;
using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage()
	{
        InitializeComponent();
        BindingContext = new MainMenuViewModel();
    }



    private async void OnStartGameBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage());
    }

    private async void OnSettingsBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
    private async void OnGarageBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GaragePage());
    }

}