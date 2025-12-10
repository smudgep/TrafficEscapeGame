using System.Runtime.CompilerServices;

namespace TrafficEscapeGame
{
    public partial class MainPage : ContentPage
    {
        private bool isGameRunning = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnStartBtn_Clicked(object? sender, EventArgs e)
        {
            GameOverLay.IsVisible = false;
            GameCanvas.IsVisible = true;
            PlayerCar.IsVisible = true;
            StartGame();
        }

        private void StartGame()
        {
            while (isGameRunning)
            {
                
            }
        }



        private void HighScoreBtn_Clicked(object sender, EventArgs e)
        {
            //Get Highest result from file storing Results
        }

        private void OnInstructionsBtn_Clicked(object sender, EventArgs e)
        {
            //Open Settings Page
        }

        private void OnExitBtn_Clicked(object sender, EventArgs e)
        {
           
        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {

        }
    }
}
