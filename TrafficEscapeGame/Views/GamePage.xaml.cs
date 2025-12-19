using TrafficEscapeGame.ViewModels;
using TrafficEscapeGame.Services;

namespace TrafficEscapeGame.Views;

public partial class GamePage : ContentPage
{
    private GameViewModel _viewModel;
    private double _distance = 0;
    private bool _isGameRunning = false;

    
    private double _laneMarkerSpeed = 5.0;

    public GamePage()
    {
        InitializeComponent();

        Console.WriteLine("=== GAME PAGE STARTED ===");

       
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;
        var screenWidth = displayInfo.Width / displayInfo.Density;
        var screenHeight = displayInfo.Height / displayInfo.Density;

        Console.WriteLine($"Screen: {screenWidth}x{screenHeight}");

        
        _viewModel = new GameViewModel(screenWidth, screenHeight);
        BindingContext = _viewModel;

       
        AddGestureRecognizers();

       
        StartGame();

        Console.WriteLine("Game initialized successfully!");
    }

    private void AddGestureRecognizers()
    {
        
        var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
        swipeLeft.Swiped += OnSwipeLeft;

        var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
        swipeRight.Swiped += OnSwipeRight;

        GameArea.GestureRecognizers.Add(swipeLeft);
        GameArea.GestureRecognizers.Add(swipeRight);

       

    }

    private void StartGame()
    {
        _isGameRunning = true;

        
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isGameRunning) return false;

            
            _distance += 0.1;
            DistanceLabel.Text = $"DISTANCE: {_distance:F0}m";

          
            _viewModel.Score = (int)_distance;

            return true;
        });

        
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!_isGameRunning) return false;

            
            AnimateRoad();

            return true;
        });
    }

    private void AnimateRoad()
    {
        
        MoveLaneMarker(LaneMarker1);
        MoveLaneMarker(LaneMarker2);
        MoveLaneMarker(LaneMarker3);

       
        if (AbsoluteLayout.GetLayoutBounds(LaneMarker1).Y > Height)
        {
            ResetLaneMarker(LaneMarker1);
        }
        if (AbsoluteLayout.GetLayoutBounds(LaneMarker2).Y > Height)
        {
            ResetLaneMarker(LaneMarker2);
        }
        if (AbsoluteLayout.GetLayoutBounds(LaneMarker3).Y > Height)
        {
            ResetLaneMarker(LaneMarker3);
        }
    }

    private void MoveLaneMarker(BoxView marker)
    {
        var bounds = AbsoluteLayout.GetLayoutBounds(marker);
        var newY = bounds.Y + _laneMarkerSpeed;
        AbsoluteLayout.SetLayoutBounds(marker,
            new Rect(bounds.X, newY, bounds.Width, bounds.Height));
    }

    private void ResetLaneMarker(BoxView marker)
    {
        var bounds = AbsoluteLayout.GetLayoutBounds(marker);
        AbsoluteLayout.SetLayoutBounds(marker,
            new Rect(bounds.X, -bounds.Height, bounds.Width, bounds.Height));
    }

    

    private void OnSwipeLeft(object sender, SwipedEventArgs e)
    {
        Console.WriteLine("Swiped Left");
        _viewModel.MoveLeft();
        AnimateCarMovement(-1);
        _viewModel.AddScore();
    }

    private void OnSwipeRight(object sender, SwipedEventArgs e)
    {
        Console.WriteLine("Swiped Right");
        _viewModel.MoveRight();
        AnimateCarMovement(1);
        _viewModel.AddScore();
    }

    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        
        _isGameRunning = !_isGameRunning;

        if (!_isGameRunning)
        {
            
            DisplayAlert("Game Paused", "Game is paused. Tap OK to resume.", "OK");
            _isGameRunning = true;
        }
    }

    

    private async void AnimateCarMovement(int direction)
    {
        
        await PlayerCar.TranslateTo(direction * 20, 0, 100, Easing.SinOut);
        await PlayerCar.TranslateTo(0, 0, 100, Easing.SpringOut);
    }

    

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isGameRunning = false;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        
        _laneMarkerSpeed = height * 0.01; 
    }
}