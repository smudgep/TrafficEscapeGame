using Microsoft.Maui.Layouts;
using TrafficEscapeGame.Models;
using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Views;

public partial class GamePage : ContentPage
{
    //game state variables
    private GameViewModel _viewModel;
    private double _distance = 0;
    private bool _isGameRunning = false;

    //random generator for obstacle positions
    private readonly Random _random = new();
    private readonly List<BoxView> _laneMarkers = new();
    private double _laneMarkerSpeed = 5.0;

    //dictionaries to track obstacle and coin views
    private readonly Dictionary<Guid, Image> _obstacleViews = new();
    private readonly Dictionary<Guid, Image> _coinViews = new();
    public GamePage()
    {
        InitializeComponent();

        //gets actual screen size
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        var screenWidth = displayInfo.Width / displayInfo.Density;
        var screenHeight = displayInfo.Height / displayInfo.Density;

        //initialize view model with screen dimensions
        _viewModel = new GameViewModel(screenWidth, screenHeight);

        //subscribe to view model events
        _viewModel.ObstacleSpawned += OnObstacleSpawned;
        _viewModel.ObstacleRemoved += OnObstacleRemoved;
        _viewModel.CoinSpawned += OnCoinSpawned;
        _viewModel.CoinCollected += OnCoinCollected;
        _viewModel.CoinRemoved += OnCoinRemoved;
        _viewModel.GameOver += OnGameOver;

        BindingContext = _viewModel;
        AddGestureRecognizers();
        InitiateLaneMarkers();
        StartGame();
    }

    //creates the visual representation of a coin
    private void OnCoinSpawned(object? sender, Models.Coin coin)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var image = new Image
            {
                Source = "coin.png",
                WidthRequest = coin.Width,
                HeightRequest = coin.Height,
                Rotation = 0
            };

            AbsoluteLayout.SetLayoutBounds(image,
                new Rect(coin.X, coin.Y, coin.Width, coin.Height));
            AbsoluteLayout.SetLayoutFlags(image,
                AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.YProportional);

            CoinsContainer.Children.Add(image);
            _coinViews[coin.Id] = image;
        });
    }

    private async void OnCoinCollected(object? sender, Models.Coin coin)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (_coinViews.TryGetValue(coin.Id, out var image))
            {
                await image.ScaleTo(1.5, 150);
                await image.FadeTo(0, 150);
                CoinsContainer.Children.Remove(image);
                _coinViews.Remove(coin.Id);
            }
        });
    }
    //removes coin from view when it is no longer active
    private void OnCoinRemoved(object? sender, Models.Coin coin)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_coinViews.TryGetValue(coin.Id, out var image))
            {
                CoinsContainer.Children.Remove(image);
                _coinViews.Remove(coin.Id);
            }
        });
    }
    //handles game over scenario
    private async void OnGameOver(object? sender, EventArgs e)
    {
        if (!_isGameRunning) return;

        _isGameRunning = false;
        //car crash animation
        await PlayerCar.ScaleTo(1.3, 150, Easing.SpringOut);
        await PlayerCar.ScaleTo(1.0, 150, Easing.SpringIn);

        //show game over alert
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var originalColor = PlayerCar.BackgroundColor;
            //flash red to indicate crash
            PlayerCar.BackgroundColor = Colors.Red.WithAlpha(0.7f);
            await Task.Delay(200);
            PlayerCar.BackgroundColor = originalColor;

            bool restart = await DisplayAlert(
                "Crash!",
                $"You hit an obstacle!\n\nFinal Score: {_viewModel.Score}\nDistance: {_distance:F0}m",
                "Play Again",
                "Main Menu");

            if (restart)
            {
                await Navigation.PushAsync(new GamePage());
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PopAsync();
            }
        });
    }

    private void AddGestureRecognizers()
    {
        //swipe gestures for car movement
        var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
        swipeLeft.Swiped += (s, e) => {
            _viewModel.MoveLeft();
            AnimateCarMovement(-1);
            _viewModel.AddScore();
        };

        var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
        swipeRight.Swiped += (s, e) => {
            _viewModel.MoveRight();
            AnimateCarMovement(1);
            _viewModel.AddScore();
        };
        //add gestures to game area
        GameArea.GestureRecognizers.Add(swipeLeft);
        GameArea.GestureRecognizers.Add(swipeRight);
    }

    private void StartGame()
    {
        _isGameRunning = true;
        //timer to update distance and score
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isGameRunning) return false;
            _distance += 0.1;
            DistanceLabel.Text = $"DISTANCE: {_distance:F0}m";
            _viewModel.Score = (int)_distance;
            return true;
        });
        //timer to animate road lane markers
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isGameRunning) return false;
            AnimateRoad();
            return true;
        });
        //timer to update obstacles and coins
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isGameRunning) return false;

            var deltaY = _laneMarkerSpeed * 0.002;
            _viewModel.UpdateObstacles(deltaY);
            //update obstacle positions
            foreach (var obstacle in _viewModel.Obstacles)
            {
                if (_obstacleViews.TryGetValue(obstacle.Id, out var image))
                {
                    AbsoluteLayout.SetLayoutBounds(image,
           new Rect(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height));
                }
            }
            //update coin positions
            foreach (var coin in _viewModel.ActiveCoins.Where(c => !c.IsCollected))
            {
                if (_coinViews.TryGetValue(coin.Id, out var image))
                {
                    AbsoluteLayout.SetLayoutBounds(image, coin.Bounds);
                }
            }

            return true;
        });
    }
    //initializes lane markers for road animation
    private void InitiateLaneMarkers()
    {
        _laneMarkers.Add(LeftMarker1);
        _laneMarkers.Add(LeftMarker2);
        _laneMarkers.Add(LeftMarker3);
        _laneMarkers.Add(MiddleMarker1);
        _laneMarkers.Add(MiddleMarker2);
        _laneMarkers.Add(MiddleMarker3);
        _laneMarkers.Add(RightMarker1);
        _laneMarkers.Add(RightMarker2);
        _laneMarkers.Add(RightMarker3);
    }
    //animates lane markers to simulate road movement
    private void AnimateRoad()
    {
        var screenHeight = Height;
        //update position of each lane marker
        foreach (var marker in _laneMarkers)
        {
            //get current bounds
            var bounds = AbsoluteLayout.GetLayoutBounds(marker);
            var newY = bounds.Y + _laneMarkerSpeed;
            //reset position if it goes off screen
            if (newY > screenHeight + 50)
            {
                newY = -100;
            }
            //set new position
            AbsoluteLayout.SetLayoutBounds(marker,
                new Rect(bounds.X, newY, bounds.Width, bounds.Height));
        }
    }
    //handles menu button click to pause/resume game
    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        if (_isGameRunning)
        {
            _isGameRunning = false;
            await DisplayAlert("Game Paused", "Game is paused. Tap OK to resume.", "OK");
            _isGameRunning = true;
        }
    }
    //animates car movement on swipe
    private async void AnimateCarMovement(int direction)
    {
        await PlayerCar.TranslateTo(direction * 20, 0, 100, Easing.SinOut);
        await PlayerCar.TranslateTo(0, 0, 100, Easing.SpringOut);
    }
    //stop game when page disappears
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isGameRunning = false;
    }
    //adjust lane marker speed based on page size
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        _laneMarkerSpeed = height * 0.01;
    }
    //creates the visual representation of an obstacle
    private void OnObstacleSpawned(object? sender, Obstacle obstacle)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //create image for obstacle
            var image = new Image
            {
                Source = "redcar.png", 
                WidthRequest = obstacle.Width,
                HeightRequest = obstacle.Height,
            };
            //set position and size
            AbsoluteLayout.SetLayoutBounds(image,
            new Rect(obstacle.X, obstacle.Y, obstacle.Width, obstacle.Height));
            AbsoluteLayout.SetLayoutFlags(image,
                AbsoluteLayoutFlags.XProportional | AbsoluteLayoutFlags.YProportional);
            //add to container and track in dictionary
            ObstaclesContainer.Children.Add(image);
            _obstacleViews[obstacle.Id] = image;
        });
    }
    //removes obstacle from view when it is no longer active
    private void OnObstacleRemoved(object? sender, Obstacle obstacle)
    {
        //runs on main thread to update UI
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_obstacleViews.TryGetValue(obstacle.Id, out var imageView)) 
            {
                ObstaclesContainer.Children.Remove(imageView);
                _obstacleViews.Remove(obstacle.Id);
            }
        });
    }
}