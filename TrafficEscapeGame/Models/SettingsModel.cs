using TrafficEscapeGame.ViewModels;

namespace TrafficEscapeGame.Models;

public class SettingsModel : BaseViewModel
{
    //stores current settings for the game
    private Difficulty _difficulty = Difficulty.Normal;
    private bool _darkMode = false;
    private bool _soundEnabled = true;

    //settrs and getters for settings
    public Difficulty Difficulty
    {
        get => _difficulty;
        set
        {
            if (_difficulty == value) return;
            _difficulty = value;
            OnPropertyChanged();
        }
    }

    public bool DarkMode
    {
        get => _darkMode;
        set
        {
            if (_darkMode == value) return;
            _darkMode = value;
            OnPropertyChanged();

        }
    }

    public bool SoundEnabled
    {
        get => _soundEnabled;
        set
        {
            if (_soundEnabled == value) return;
            _soundEnabled = value;
            OnPropertyChanged();
        }
    }
}