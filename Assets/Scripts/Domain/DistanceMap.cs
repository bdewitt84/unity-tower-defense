
using Unity.VisualScripting;

public class DistanceMap : GridMap
{
    private int[,] distances;
    private const int maxValue = int.MaxValue;

    public DistanceMap(int width, int height) : base(width, height)
    {
        distances = new int[width, height];
    }

    public void SetDistance(GridCoordinate gridCoordinate, int distance)
    {
        distances[gridCoordinate.X, gridCoordinate.Y] = distance;
    }

    public int GetDistance(GridCoordinate gridCoordinate)
    {
        return distances[gridCoordinate.X, gridCoordinate.Y];
    }

    public void Reset()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                distances[i, j] = maxValue;
            }
        }
    }
}
