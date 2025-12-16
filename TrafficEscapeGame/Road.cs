/*

namespace TrafficEscapeGame;

public partial class Road : ContentPage
{
    public double Speed { get; set; } = 600;

    public Road()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        Initialize();
    }

    private void Initialize()
    {
        double h = Height;

        road1.TranslationY = 0;
        road2.TranslationY = -h;

        Lane1.TranslationY = 0;
        Lane2.TranslationY = -h;
    }

    public void Update(double deltaTime)
    {
        double move = Speed * deltaTime;
        double h = Height;

        Move(road1, road2, move, h);
        Move(Lane1, Lane2, move, h);
    }

    private static void Move(VisualElement a, VisualElement b, double move, double h)
    {
        a.TranslationY += move;
        b.TranslationY += move;

        if (a.TranslationY >= h)
            a.TranslationY = b.TranslationY - h;

        if (b.TranslationY >= h)
            b.TranslationY = a.TranslationY - h;
    }
}
*/