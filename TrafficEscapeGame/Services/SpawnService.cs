using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class SpawnService
{
    //list for all active obstacles
    private readonly List<Obstacle> _activeObstacles = new();
    private readonly Random _random = new();
    private bool _isSpawning = false;
    //timer for spawning obstacles
    private IDispatcherTimer? _spawnTimer;
    //base spawn interval
    private double _spawnIntervalSeconds = 2.0;

    //events for created and removed obstacles
    public event EventHandler<Obstacle>? ObstacleSpawned;
    public event EventHandler<Obstacle>? ObstacleRemoved;

    public void SetDifficulty(string difficulty)
    {
        //changes spawn interval based on difficulty
        _spawnIntervalSeconds = difficulty.ToLower() switch
        {
            "easy" => 2.0,
            "normal" => 1.0,
            "hard" => 0.4,
            _ => 1.0
        };
        //if currently spawning, update timer interval
        if (_isSpawning && _spawnTimer != null)
        {
            _spawnTimer.Stop();
            _spawnTimer.Interval = TimeSpan.FromSeconds(_spawnIntervalSeconds);
            _spawnTimer.Start();
        }
    }

    public void StartSpawning(string difficulty = "normal")
    {
        if (_isSpawning) return;

        _isSpawning = true;
        //set difficulty which adjusts spawn interval
        SetDifficulty(difficulty);

        //create and start timer
        _spawnTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_spawnTimer != null)
        {
            _spawnTimer.Interval = TimeSpan.FromSeconds(_spawnIntervalSeconds);
            _spawnTimer.Tick += OnSpawnTimerTick;
            _spawnTimer.Start();
        }
    }

    //stops spawn timer
    public void StopSpawning()
    {
        if (!_isSpawning) return;
        _isSpawning = false;
        _spawnTimer?.Stop();
        _spawnTimer = null;
    }

    //timer tick to add new obstacle
    private void OnSpawnTimerTick(object? sender, EventArgs e)
    {
        var obstacle = CreateRandomObstacle();
        _activeObstacles.Add(obstacle);
        ObstacleSpawned?.Invoke(this, obstacle);
    }

    //creates obstacle in random lane
    private Obstacle CreateRandomObstacle()
    {
        var lane = _random.Next(0, 3);
        double laneX = lane switch
        {
            0 => 0.22,
            1 => 0.5,
            2 => 0.78,
            _ => 0.5
        };

        return new Obstacle
        {
            Lane = lane,
            X = laneX,
            Y = -0.2
        };
    }

    public void UpdateObstacles(double deltaY)
    {
        //new temp list for obstacles to remove
        var toRemove = new List<Obstacle>();
        //update position of each obstacle
        foreach (var obstacle in _activeObstacles)
        {
            obstacle.Y += deltaY;

            if (obstacle.Y > 1.5)
            {
                toRemove.Add(obstacle);
            }
        }
        //remove obstacles that are out of bounds
        foreach (var obstacle in toRemove)
        {
            _activeObstacles.Remove(obstacle);
            ObstacleRemoved?.Invoke(this, obstacle);
        }
    }
    //returns copylist of active obstacles
    public List<Obstacle> GetActiveObstacles() => new(_activeObstacles);
}