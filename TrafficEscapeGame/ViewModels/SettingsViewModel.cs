using TrafficEscapeGame.Models;
using TrafficEscapeGame.Services;

namespace TrafficEscapeGame.ViewModels;

public class SettingsViewModel : BaseViewModel
{
    private readonly StorageService _storageService;
    private SettingsModel _settings;
    public event EventHandler<bool>? ThemeChanged;

    public string SettingsTitle => "Settings";

    public SettingsModel Settings
    {
        get => _settings;
        set
        {
            if (_settings == value) return;
            _settings = value;
            OnPropertyChanged();
        }
    }
    //radio button bindings for difficulty levels
    public bool IsEasy
    {
        get => Settings.Difficulty == Difficulty.Easy;
        set
        {
            if (!value) return;
            Settings.Difficulty = Difficulty.Easy;
            UpdateRadioButtons();
            SaveSettings();
        }
    }

    public bool IsNormal
    {
        get => Settings.Difficulty == Difficulty.Normal;
        set
        {
            if (!value) return;
            Settings.Difficulty = Difficulty.Normal;
            UpdateRadioButtons();
            SaveSettings();
        }
    }

    public bool IsHard
    {
        get => Settings.Difficulty == Difficulty.Hard;
        set
        {
            if (!value) return;
            Settings.Difficulty = Difficulty.Hard;
            UpdateRadioButtons();
            SaveSettings();
        }
    }
    //notify radio buttons to update their state
    private void UpdateRadioButtons()
    {
        OnPropertyChanged(nameof(IsEasy));
        OnPropertyChanged(nameof(IsNormal));
        OnPropertyChanged(nameof(IsHard));
    }
    //create storage service and load settings
    public SettingsViewModel()
    {
        _storageService = new StorageService();
        _settings = new SettingsModel();
        LoadSettings();
    }
    //load settings from storage
    private async void LoadSettings()
    {
        Settings = await _storageService.LoadSettingsAsync();
        UpdateRadioButtons();
    }
    //save current settings to storage
    public async Task SaveSettings()
    {
        await _storageService.SaveSettingsAsync(Settings);
    }
    public async void OnSoundToggled() => await SaveSettings();
}