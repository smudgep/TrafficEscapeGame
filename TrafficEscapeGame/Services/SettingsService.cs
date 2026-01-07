using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class SettingsService
{
    //storage service to save and load settings
    private readonly StorageService _storageService;
    //current settings
    private SettingsModel _currentSettings;

    //event to notify when settings change
    public event EventHandler<SettingsModel>? SettingsChanged;

    //constructor to initialize storage service and default settings
    public SettingsService(StorageService storageService)
    {
        _storageService = storageService;
        _currentSettings = new SettingsModel();
    }

    public async Task LoadSettingsAsync()
    {
        _currentSettings = await _storageService.LoadSettingsAsync();
        SettingsChanged?.Invoke(this, _currentSettings);
    }

    //saves new settings and triggers change
    public async Task SaveSettingsAsync(SettingsModel settings)
    {
        _currentSettings = settings;
        await _storageService.SaveSettingsAsync(settings);
        SettingsChanged?.Invoke(this, _currentSettings);
    }

    //loads current settings
    public async Task<SettingsModel> GetCurrentSettingsAsync()
    {
        return _currentSettings;
    }
}