using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrafficEscapeGame.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    //method to raise property changed event for data binding
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        // invoke event if there are subscribers
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}