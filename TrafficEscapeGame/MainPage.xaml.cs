using Microsoft.UI.Xaml.Documents;

namespace TrafficEscapeGame
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            
            InitializeComponent();
        }

        private void OnStartClicked(object? sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            //throw new NotImplementedException();
        }

        private void HighScoreBtn_Clicked(object sender, EventArgs e)
        {
            //Get Highest result from file storing Results
        }

        private void OnInstructionsBtn_Clicked(object sender, EventArgs e)
        {
            //Open Settings Page
        }
    }
}
