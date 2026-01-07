using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Models;

//inherits baseviewmodel 
public class Coin : BaseViewModel
{
    //x and y coordinates
    private double _x;
    private double _y;

    //gives each coin a unique identifier
    public Guid Id { get; } = Guid.NewGuid();

    //which lane the coin is in (0, 1, or 2)
    public int Lane { get; set; }

    //whether the coin has been collected
    public bool IsCollected { get; set; }


    //properties for x and y with change notification
    public double X
    {
        
        get => _x;
        set
        {
            if (_x == value) return;
            _x = value;
            //position has changed so bindings updates
            OnPropertyChanged();
        }
    }

    public double Y
    {
        get => _y;
        set
        {
            if (_y == value) return;
            _y = value;
            //position has changed so bindings updates
            OnPropertyChanged();
        }
    }

    //set default width, height, and color of the coin
    public double Width { get; set; } = 40;
    public double Height { get; set; } = 40;
    public Color Color { get; set; } = Colors.Gold;

    //calculate bounds of rect for collision detection
    public Rect Bounds => new Rect(X, Y, Width, Height);
}