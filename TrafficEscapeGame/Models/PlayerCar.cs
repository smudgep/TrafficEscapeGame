using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Models;

public class PlayerCar : BaseViewModel
{
    //stores x and y position of player car
    private double _x;
    private double _y;

    public PlayerCar()
    {
        Width = 80;
        Height = 150;
    }

    //getter and setter for x and y properties with propertychange notification
    public double X
    {
        get => _x;
        set
        {
            if (_x == value) return;
            _x = value;
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
            OnPropertyChanged();
        }
    }

    public double Width { get; set; }
    public double Height { get; set; }
    public Color Color { get; set; }
    public string ImageSource { get; set; } = "bluecar.png";

    //calculate collision bounds for car 
    public Rect Bounds => new Rect(X, Y, Width, Height);
}