
namespace MazeGenerator;

public class Maze<TMazeCell>
where TMazeCell : class, IMazeCell
{
    private readonly TMazeCell[,] _maze;
    
    public Maze(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("The number of columns and rows mus be greater than or equal to 0");
        var factory = new Factory<TMazeCell>();
        _maze = new TMazeCell[rows, columns];
        for (var row = 0; row < rows; row++)
        for (var column = 0; column < columns; column++)
            _maze[row, column] = factory.GetInstance(row, column);
    }

    public int XLenght => _maze.GetLength(0);
    public int YLenght => _maze.GetLength(1);

    public TMazeCell? GetNeighbour(TMazeCell cell, Directions neighbourDirection)
    {
        var xCoordinate = cell.XIndex;
        var yCoordinate = cell.YIndex;
        
        switch (neighbourDirection)
        {
            case Directions.N:
                yCoordinate += 1;
                break;
            case Directions.S:
                yCoordinate -= 1;
                break;
            case Directions.E:
                xCoordinate += 1;
                break;
            case Directions.W:
                xCoordinate -= 1;
                break;
            case Directions.Ne:
                yCoordinate += 1;
                xCoordinate += 1;
                break;
            case Directions.Se:
                yCoordinate -= 1;
                xCoordinate += 1;
                break;
            case Directions.Sw:
                yCoordinate -= 1;
                xCoordinate -= 1;
                break;
            case Directions.Nw:
                yCoordinate += 1;
                xCoordinate -= 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(neighbourDirection), neighbourDirection, null);

        }
        
        return GetCell(xCoordinate, yCoordinate);
    }

    public TMazeCell? GetCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _maze.GetLength(0) || y >= _maze.GetLength(1))
            return null;
        return _maze[x, y];
    }

    public void SetCell(TMazeCell cell)
    {
        _maze[cell.XIndex, cell.YIndex] = cell;
    }
}