namespace TrafficEscapeGame.Services;

public class CollisionService
{
    // Checks for collision between two rectangles
    public bool CheckCollision(Rect rect1, Rect rect2)
    {
        return rect1.IntersectsWith(rect2);
    }
}