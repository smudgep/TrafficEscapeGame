using Microsoft.Maui.Layouts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TrafficEscapeGame
{
    public partial class MainPage : ContentPage
    {
        private bool isGameRunning = false;
        private readonly double[] Lanes = new double[]
        {
                0.25, // Lane 1 
                0.50, // Lane 2
                0.75, // Lane 3
        };

        private int currentPlayerLane = 1;
        private const int LaneChangeDuration = 100;
        private IDispatcherTimer? gameTimer, countdownTimer;
        private double roadSpeed = 600; // pixels per second
        private DateTime lastFrameTime;
        private int countdownValue = 3;

        public MainPage()
        {
            InitializeComponent();

            // Ensure layout-driven coordinates are valid before we rely on Width/Height.
            GameCanvas.SizeChanged += GameCanvas_SizeChanged;
        }

        private void GameCanvas_SizeChanged(object? sender, EventArgs e)
        {
            // Only initialize once when canvas has a real size
            if (GameCanvas.Width <= 0 || GameCanvas.Height <= 0)
                return;
        }

        private void OnStartBtn_Clicked(object? sender, EventArgs e)
        {
            GameOverLay.IsVisible = false;
            GameCanvas.IsVisible = true;
            PlayerCar.IsVisible = true;
            PlayerCar.TranslationX = 0;
            PlayerCar.TranslationY = 0;
            StartCountdown();
        }

        private void StartGame()
        {
            isGameRunning = true;

            // Initialize lastFrameTime so deltaTime calculation is valid on first tick
            lastFrameTime = DateTime.Now;

            // Initialize and start the game loop timer
            gameTimer = Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += OnGameLoop;
            gameTimer.Start();
        }

        private void OnGameLoop(object? sender, EventArgs e)
        {
            if (!isGameRunning) return;
            var now = DateTime.Now;

            double deltaTime = (now - lastFrameTime).TotalSeconds;
            lastFrameTime = now;

            MoveRoad(deltaTime);
        }

        private void MoveRoad(double deltaTime)
        {
            double move = roadSpeed * deltaTime;
            double h = GameCanvas.Height;

            MovePair(Road1, Road2, move, h);
            MovePair(LeftLane1, LeftLane2, move, h);
            MovePair(RightLane1, RightLane2, move, h);
        }

        private void MovePair(VisualElement a, VisualElement b, double move, double height)
        {
            a.TranslationY += move;
            b.TranslationY += move;

            if (a.TranslationY >= height)
                a.TranslationY = b.TranslationY - height;

            if (b.TranslationY >= height)
                b.TranslationY = a.TranslationY - height;
        }

        private void HighScoreBtn_Clicked(object sender, EventArgs e)
        {
            //Get Highest result from file storing Results
        }

        private void OnInstructionsBtn_Clicked(object sender, EventArgs e)
        {
            //Open Settings Page
        }

        private void OnExitBtn_Clicked(object? sender, EventArgs e)
        {
            isGameRunning = false;
            GameOverLay.IsVisible = true;
            GameCanvas.IsVisible = false;
        }

        private async Task MoveCarToLane(int newLane)
        {
            //Make sure its swiped to a possible lane
            if (newLane >= 0 && newLane < 3)
            {
                currentPlayerLane = newLane;
                double targetLane = Lanes[currentPlayerLane];

                double laneCenterPixelX = GameCanvas.Width * targetLane;

                double targetPixelX = laneCenterPixelX - (PlayerCar.Width);
                double pixelShift = GameCanvas.Width * (targetLane - 0.5);

                await PlayerCar.TranslateTo(pixelShift, PlayerCar.TranslationY, LaneChangeDuration);
            }
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs swipeDirectionData)
        {
            int direction = 0;

            if (swipeDirectionData.Direction == SwipeDirection.Left)
            {
                direction = -1;
            }
            else if (swipeDirectionData.Direction == SwipeDirection.Right)
            {
                direction = 1;
            }

            int newLaneIndex = currentPlayerLane + direction;

            await MoveCarToLane(newLaneIndex);
        }

        //when i want to use this add it as a setting the player can choose to enable and disable swipe controls
        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {

        }

        private void InitializeRoad()
        {
           
        }

        private void OnCountdownTick(object? sender, EventArgs e)
        {
            if (countdownValue > 0)
            {
                CountdownLbl.Text = countdownValue.ToString();
                countdownValue--;
            }
            else
            {
                countdownTimer?.Stop();
                CountdownLbl.IsVisible = false;
                CountdownLbl.Text = "3";
                countdownValue = 3;

                StartGame();
            }
        }

        private void StartCountdown()
        {
            isGameRunning = false;

            CountdownLbl.Text = "3";
            CountdownLbl.IsVisible = true;

            countdownTimer = Dispatcher.CreateTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += OnCountdownTick;
            countdownTimer.Start();
        }
    }
}