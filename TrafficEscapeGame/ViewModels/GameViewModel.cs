using Microsoft.Maui.Graphics;
using System.ComponentModel;
using TrafficEscapeGame.Services;

namespace TrafficEscapeGame.ViewModels;

public class GameViewModel : BaseViewModel
{
    private readonly GameLoopService _gameLoop;
    private Rect _carBounds;
    private int _score;

    public GameViewModel(double width, double height)
    {
        Console.WriteLine($"ViewModel: Screen {width}x{height}");

        _gameLoop = new GameLoopService();
        _gameLoop.Initialize(height, width);

        _gameLoop.PositionChanged += OnPositionChanged;

        UpdateCarBounds();

        Score = 0;
    }

    private void OnPositionChanged(object sender, EventArgs e)
    {
        UpdateCarBounds();
    }

    private void UpdateCarBounds()
    {
        
        var lanePosition = _gameLoop.PlayerXPos;

        CarBounds = new Rect(lanePosition, 0.8, 80, 150);

        Console.WriteLine($"CarBounds updated: X={CarBounds.X}, Y={CarBounds.Y}");
    }

    public void MoveLeft()
    {
        _gameLoop.MoveLeft();
    }

    public void MoveRight()
    {
        _gameLoop.MoveRight();
    }

   
    public Rect CarBounds
    {
        get => _carBounds;
        set
        {
            if (_carBounds != value)
            {
                _carBounds = value;
                OnPropertyChanged(nameof(CarBounds));
            }
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
    }

    public void AddScore()
    {
        Score += 10;
    }
}