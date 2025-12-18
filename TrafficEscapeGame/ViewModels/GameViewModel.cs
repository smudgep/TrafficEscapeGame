
using System.ComponentModel;

namespace TrafficEscapeGame.ViewModels;

public class GameViewModel : INotifyPropertyChanged
{
	public int _score { get; set; }

	public int Score 
	{
		get => _score;
		set 
		{
			if(_score != value)
			{
				_score = value;
				OnPropertyChanged(nameof(Score));
            }
        }
    }

	public void AddScore()
	{ 
		Score += 10;
    }

public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}




