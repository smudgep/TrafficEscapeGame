using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using TrafficEscapeGame.Models;
using TrafficEscapeGame.Services;

namespace TrafficEscapeGame.ViewModels;

public class GameViewModel : BaseViewModel
{
    private readonly GameLoopService _gameLoop;
    private readonly SpawnService _spawnService;
    private readonly CoinService _coinService;
    private readonly CollisionService _collisionService;
    private readonly SettingsService _settingsService;

    private int _score;
    private double _gameWidth;
    private double _gameHeight;

    private ObservableCollection<Obstacle> _obstacles = new();
    private ObservableCollection<Coin> _activeCoins = new();

    private bool _isGameOver = false;
    private double _coinCount = 0;
    private Rect _carBounds;

    private double _carXProportional;
    private double _carYProportional = 0.8;

    //events for ui updates
    public event EventHandler<Obstacle>? ObstacleSpawned;
    public event EventHandler<Obstacle>? ObstacleRemoved;
    public event EventHandler? GameOver;
    public event EventHandler<Coin>? CoinSpawned;
    public event EventHandler<Coin>? CoinCollected;
    public event EventHandler<Coin>? CoinRemoved;

    public GameViewModel(double width, double height)
    {
        //stores game dimensions
        _gameWidth = width;
        _gameHeight = height;

        //initialize score and coin count
        Score = 0;
        CoinCount = 0;

        //create and initialize game loop
        _gameLoop = new GameLoopService();
        _gameLoop.Initialize();

        _settingsService = new SettingsService(new StorageService());

        //create spawn service 
        _spawnService = new SpawnService();
        //subscribe to obstacle events
        _spawnService.ObstacleSpawned += (sender, obstacle) =>
        {
            Obstacles.Add(obstacle);
            ObstacleSpawned?.Invoke(this, obstacle);
        };

        //subscribe to obstacle removal event
        _spawnService.ObstacleRemoved += (sender, obstacle) =>
        {
            //removes obstacle from active list
            var toRemove = Obstacles.FirstOrDefault(o => o.Id == obstacle.Id);
            if (toRemove != null)
                Obstacles.Remove(toRemove);
            ObstacleRemoved?.Invoke(this, obstacle);
        };

        //create coin service
        _coinService = new CoinService();
        //subscribe to coin events
        _coinService.CoinSpawned += (sender, coin) =>
        {
            ActiveCoins.Add(coin);
            CoinSpawned?.Invoke(this, coin);
        };

        //subscribe to coin collected event
        _coinService.CoinCollected += (sender, coin) =>
        {
            //increment coin count and score
            CoinCount += 1;
            Score += 10;
            CoinCollected?.Invoke(this, coin);
        };

        //remove coin event
        _coinService.CoinRemoved += (sender, coin) =>
        {
            //returns first coin with matching id
            var toRemove = ActiveCoins.FirstOrDefault(c => c.Id == coin.Id);
            if (toRemove != null)
                ActiveCoins.Remove(toRemove);
            CoinRemoved?.Invoke(this, coin);
        };

        //create collision service
        _collisionService = new CollisionService();
        //
        _gameLoop.PositionChanged += OnPositionChanged;
        //set car position
        UpdateCarPosition();
       
        InitializeDifficulty();
    }

    private async void InitializeDifficulty()
    {
        //fetch current settings asynchronously
        var settings = await _settingsService.GetCurrentSettingsAsync();
        //change enum to string
        string difficulty = settings.Difficulty.ToString().ToLower();
        //configure spawn service based on difficulty
        _spawnService.SetDifficulty(difficulty);
        _spawnService.StartSpawning(difficulty);
        _coinService.StartSpawning();
    }
    //when called player moves lanes
    private void OnPositionChanged(object? sender, EventArgs e)
    {
        UpdateCarPosition();
    }
    //updates car position
    private void UpdateCarPosition()
    {
        _carXProportional = _gameLoop.PlayerXPos;
        CarBounds = new Rect(_carXProportional, _carYProportional, 80, 150);
    }

    public void MoveLeft() => _gameLoop.MoveLeft();
    public void MoveRight() => _gameLoop.MoveRight();

    //car bounding box 
    public Rect CarBounds
    {
        get => _carBounds;
        set
        {
            if (_carBounds == value) return;
            _carBounds = value;
            OnPropertyChanged();
        }
    }
    //car bounding box in pixel coordinates
    public Rect CarPixelBounds
    {
        get
        {
            double carPixelX = _carXProportional * _gameWidth - 40;
            double carPixelY = _carYProportional * _gameHeight - 75;
            return new Rect(carPixelX, carPixelY, 80, 150);
        }
    }

    //player score 
    public int Score
    {
        get => _score;
        set
        {
            //if score unchanged return
            if (_score == value) return;
            _score = value;
            OnPropertyChanged();
        }
    }

    //number of coins collected
    public double CoinCount
    {
        get => _coinCount;
        set
        {
            //if coin count unchanged return
            if (_coinCount == value) return;
            _coinCount = value;
            OnPropertyChanged();
        }
    }

    //list of active coins
    public ObservableCollection<Coin> ActiveCoins
    {
        get => _activeCoins;
        set
        {
            //if unchanged return
            _activeCoins = value;
            OnPropertyChanged();
        }
    }

    //adds points to score
    public void AddScore(int points = 10) => Score += points;

    //list of active obstacles
    public ObservableCollection<Obstacle> Obstacles
    {
        get => _obstacles;
        set
        {
            _obstacles = value;
            OnPropertyChanged();
        }
    }

    //updates obstacle positions
    public void UpdateObstacles(double deltaY)
    {
        if (_isGameOver) return;
        //update obstacle and coin positions
        _spawnService.UpdateObstacles(deltaY);
        _coinService.UpdateCoins(deltaY);
        //perform collision checks
        CheckCollisions();
        CheckCoinCollection();
    }

    //checks for collisions between car and obstacles
    private void CheckCollisions()
    {
        if (_isGameOver) return;
        Rect carBounds = CarPixelBounds;

        foreach (var obstacle in Obstacles)
        {
            double obstaclePixelX = obstacle.X * _gameWidth - obstacle.Width / 2;
            double obstaclePixelY = obstacle.Y * _gameHeight;
            Rect obstacleBounds = new Rect(obstaclePixelX, obstaclePixelY, obstacle.Width, obstacle.Height);

            if (_collisionService.CheckCollision(carBounds, obstacleBounds))
            {
                _isGameOver = true;
                GameOver?.Invoke(this, EventArgs.Empty);
                break;
            }
        }
    }
    //checks for coin collection
    private void CheckCoinCollection()
    {
        
        Rect carBounds = CarPixelBounds;
        _coinService.CheckCoinCollection(carBounds, _gameWidth, _gameHeight);
    }
    //checks if game is over
    public bool IsGameOver => _isGameOver;

    //stops spawnign when game ends
    public void StopGame()
    {
        _spawnService.StopSpawning();
        _coinService.StopSpawning();
    }

}