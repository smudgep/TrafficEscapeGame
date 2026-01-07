namespace TrafficEscapeGame.ViewModels;

public class MainMenuViewModel : BaseViewModel
{
    //main menu title
    private string _title = "Traffic Escape Game!";
    //getter and setter for title with propertychange notification
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

}