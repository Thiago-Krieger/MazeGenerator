namespace MazeGenerator;

public interface IMazeGenerator<TMazeCell>
    where TMazeCell : class, IMazeCell
{
    public Maze<TMazeCell> Maze { get; }
}