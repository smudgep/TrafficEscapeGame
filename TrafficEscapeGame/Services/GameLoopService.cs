using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class GameLoopService
{
    private int _currentLane = 1;
    private readonly double[] _lanePositions = { 0.22, 0.5, 0.78 };

    // Player's current position
    public double PlayerXPos { get; private set; }
    public double PlayerYPos { get; private set; }

    //event when player moves to new position
    public event EventHandler? PositionChanged;


    //initial player position 
    public void Initialize()
    {
        PlayerXPos = _lanePositions[_currentLane];
        PlayerYPos = 0.8;
        PositionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void MoveRight()
    {
        //move right if not in right lane
        if (_currentLane < 2)
        {
            _currentLane++;
            UpdatePlayerPosition();
        }
    }

    public void MoveLeft()
    {
        //move left if not in left lane
        if (_currentLane > 0)
        {
            _currentLane--;
            UpdatePlayerPosition();
        }
    }

    //updates player position and triggers event
    private void UpdatePlayerPosition()
    {
        PlayerXPos = _lanePositions[_currentLane];
        PlayerYPos = 0.8;
        PositionChanged?.Invoke(this, EventArgs.Empty);
    }
}