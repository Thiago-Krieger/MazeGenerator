namespace MazeGenerator;

[Flags]
public enum Directions
{
    N = 1,
    Ne = 2,
    E = 4,
    Se = 8,
    S = 16,
    Sw = 32,
    W = 64,
    Nw = 128
}

public static class DirectionExtension
{
    public static Directions GetOppositeDirection(this Directions direction)
    {
        return direction switch
        {
            Directions.N => Directions.S,
            Directions.Ne => Directions.Sw,
            Directions.E => Directions.W,
            Directions.Se => Directions.Nw,
            Directions.S => Directions.N,
            Directions.Sw => Directions.Ne,
            Directions.W => Directions.E,
            Directions.Nw => Directions.Se,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}