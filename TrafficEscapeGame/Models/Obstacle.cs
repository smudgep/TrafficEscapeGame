using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Models;

//baseviewmodel for property change
public class Obstacle : BaseViewModel
{
    private double _x;
    private double _y;

    //unique identifier for each obstacle
    public Guid Id { get; } = Guid.NewGuid();
    public int Lane { get; set; }

    //sets x and y with propertychange notification
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

    public double Width { get; set; } = 80;
    public double Height { get; set; } = 120;
    public Color Color { get; set; } = Colors.Red;

    //sets image as redcar.png
    public string ImageSource { get; set; } = "redcar.png";
}