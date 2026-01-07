using System.Text.Json;
using TrafficEscapeGame.Models;

namespace TrafficEscapeGame.Services;

public class StorageService
{
    private const string SETTINGS_KEY = "game_settings";
    // JSON serialization options
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        //make json human readable
        WriteIndented = true
    };

    public async Task SaveSettingsAsync(SettingsModel settings)
    {
        try
        {
            //convert settings to json and saves
            var json = JsonSerializer.Serialize(settings, _jsonOptions);
            await SecureStorage.SetAsync(SETTINGS_KEY, json);
        }//fail
        catch {  }
    }

    public async Task<SettingsModel> LoadSettingsAsync()
    {
        try
        {
            //loads settings from storage
            var json = await SecureStorage.GetAsync(SETTINGS_KEY);
            //if json not null or empty, deserialize and return
            if (!string.IsNullOrEmpty(json))
            {
                var settings = JsonSerializer.Deserialize<SettingsModel>(json);
                if (settings != null)
                {
                    return settings;
                }
            }
        }
        catch {  }//fail

        return new SettingsModel();
    }

    public async Task<int> SaveHighScoreAsync(int score)
    {
        return score;
    }

    public async Task<int> LoadHighScoreAsync()
    {
        return 0; 
    }
}