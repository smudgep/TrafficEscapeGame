using TrafficEscapeGame.Views;

namespace TrafficEscapeGame;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MainMenuPage());
    }
}
