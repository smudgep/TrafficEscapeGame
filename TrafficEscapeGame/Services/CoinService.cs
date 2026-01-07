using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class CoinService
{
    //list to track all active coins
    private readonly List<Coin> _activeCoins = new();
    //random generator for coin positions
    private readonly Random _random = new();
    private bool _isSpawning = false;
    //timer for spawning coins
    private IDispatcherTimer? _spawnTimer;

    //events for coin lifecycle
    public event EventHandler<Coin>? CoinSpawned;
    public event EventHandler<Coin>? CoinCollected;
    public event EventHandler<Coin>? CoinRemoved;

    public void StartSpawning()
    {
        if (_isSpawning) return;

        _isSpawning = true;
        //creates timer
        _spawnTimer = Application.Current?.Dispatcher.CreateTimer();
        //sets timer intrval to 3 seconds and starts it
        if (_spawnTimer != null)
        {
            _spawnTimer.Interval = TimeSpan.FromSeconds(3);
            _spawnTimer.Tick += OnSpawnTimerTick;
            _spawnTimer.Start();
        }
    }

    //stops spawning coins
    public void StopSpawning()
    {
        if (!_isSpawning) return;
        _isSpawning = false;
        _spawnTimer?.Stop();
        _spawnTimer = null;
    }

    //creates and adds a new coin
    private void OnSpawnTimerTick(object? sender, EventArgs e)
    {
        var coin = CreateRandomCoin();
        _activeCoins.Add(coin);
        CoinSpawned?.Invoke(this, coin);
    }
    //adds a new coin at random lane
    private Coin CreateRandomCoin()
    {
        var lane = _random.Next(0, 3);
        double laneX = lane switch
        {
            0 => 0.22,
            1 => 0.5,
            2 => 0.78,
            _ => 0.5
        };

        return new Coin
        {
            Lane = lane,
            X = laneX,
            Y = -0.2
        };
    }

    public void UpdateCoins(double deltaY)
    {

        //temp list for coins to be removed
        var toRemove = new List<Coin>();

        //loops for all coins in game 
        foreach (var coin in _activeCoins)
        {
            //if coin is collected, add to removal list
            if (coin.IsCollected)
            {
                toRemove.Add(coin);
                continue;
            }

            coin.Y += deltaY;

            if (coin.Y > 1.5)
            {
                toRemove.Add(coin);
            }
        }

        //removes coins in removal list
        foreach (var coin in toRemove)
        {
            _activeCoins.Remove(coin);
            CoinRemoved?.Invoke(this, coin);
        }
    }

    //
    public bool CheckCoinCollection(Rect playerBounds, double gameWidth, double gameHeight)
    {
        //tracks if any coins collected
        bool collectedAny = false;

        //loops through active coins not yet collected
        foreach (var coin in _activeCoins.Where(c => !c.IsCollected))
        {
            //center coin position 
            double coinX = coin.X * gameWidth - coin.Width / 2;
            double coinY = coin.Y * gameHeight - coin.Height / 2;
            Rect coinBounds = new Rect(coinX, coinY, coin.Width, coin.Height);

            //if player rectangle intersects with coin rectangle
            if (playerBounds.IntersectsWith(coinBounds))
            {
                //coin is collected
                coin.IsCollected = true;
                //trigger viewmodel event 
                CoinCollected?.Invoke(this, coin);
                collectedAny = true;
            }
        }

        return collectedAny;
    }

    //returns list of active coins
    public List<Coin> GetActiveCoins() => new(_activeCoins);

}