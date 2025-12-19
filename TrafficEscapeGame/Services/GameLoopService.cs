using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class GameLoopService
{
    private int _currentLane = 1;
    private readonly double[] _lanePositions = { 0.25, 0.5, 0.75 };
    public double PlayerXPos { get; set; }
    public double PlayerYPos { get; set; }

    public event EventHandler PositionChanged;

    public void Initialize(double height, double width)
    { 
        UpdatePlayerPosition();
    }
    public void MoveRight()
    {
        if (_currentLane < 2)
        {
            _currentLane++;
            UpdatePlayerPosition();
        }
    }

    public void MoveLeft()
    {
        if (_currentLane > 0)
        {
            _currentLane--;
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        PlayerXPos = _lanePositions[_currentLane];
        PlayerYPos = 0.8;
        Console.WriteLine($"Lane: {_currentLane}, X: {PlayerXPos}, Y: {PlayerYPos}");

        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

}