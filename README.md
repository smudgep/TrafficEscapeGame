# TrafficEscapeGame

Project description: What it does and why
Traffic Escape is a high-speed arcade game developed using .NET MAUI. The player controls a blue car navigating a three-lane highway, weaving through oncoming traffic (red cars) while collecting coins to increase their score.

The game features dynamic obstacle spawning, difficulty settings (Easy, Normal, Hard), and an MVVM-based architecture to manage game states, animations, and collision detection. This project was designed to demonstrate real-time UI updates, gesture handling, and service-oriented game logic in a cross-platform environment.


Technologies Used
Framework: .NET MAUI (Multi-platform App UI)

Language: C#

Markup: XAML (Extensible Application Markup Language)

Design Pattern: MVVM (Model-View-ViewModel)

Graphics: Microsoft.Maui.Graphics & AbsoluteLayout for high-performance positioning

Setup: 
  SETUP INSTRUCTIONS: To run this project locally, ensure you have the .NET 8 SDK and Visual Studio 2022 (with the .NET         MAUI workload) installed.
  
  Clone the Repository: git clone https://github.com/smudgep/TrafficEscapeGame.git
  Open the project: Open the .sln file in visula studio.
  Run the game: Selct you target device (Android Emulator, iOS Simulator, or Windows Machine) and press start button

Usage: 
  Main Menu: Toggle between Light and Dark mode using the button at the top.

  Controls: Swipe Left: Move the car to the left lane, Swipe Right: Move the car to the right lane.
  
  Objective: Avoid the red cars. Each lane change and coin collection increases your score.
  
  Pause: Tap the menu icon during gameplay to pause.

AI Acknowledgment: 
Gemini: Provided boilerplate code for the BaseViewModel (INotifyPropertyChanged) and suggested logic for the   OnSizeAllocated method.
  
ChatGPT: Used for troubleshooting XAML AbsoluteLayout positioning and debugging the Dispatcher.StartTimer logic for road   animations.
