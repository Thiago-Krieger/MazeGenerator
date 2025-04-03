#pragma warning disable CA1416
using System.Drawing;

namespace MazeGenerator;

public class BackTracker<TMazeCell> : IMazeGenerator<TMazeCell>
    where TMazeCell : class, IDrawableCell
{
    private readonly bool _animate;
    private readonly Random _random;
    private readonly IDictionary<Tuple<int, int>, bool> _exploredRegistry;
    private readonly Maze<TMazeCell> _maze;
    private readonly List<Image> _frames = new();

    public BackTracker(int rows, int columns, bool animate)
    {
        _animate = animate;
        _maze = new Maze<TMazeCell>(rows, columns);
        _exploredRegistry = new Dictionary<Tuple<int, int>, bool>();
        for (var row = 0; row < rows; row++)
        for (var column = 0; column < columns; column++)
            _exploredRegistry.Add(new KeyValuePair<Tuple<int, int>, bool>(new Tuple<int, int>(row, column), false));
        _random = new Random();
        SetStartingCell();
        SetEndingCell();
        Maze = WriteMaze(0, 0);

        if (animate)
            MazeDrawer<TMazeCell>.Animate(_frames);
    }

    private void SetEndingCell()
    {
        var xMin = (int) Math.Floor(_maze.XLenght * 2f / 5);
        var xMax = (int) Math.Ceiling(_maze.XLenght * 3f / 5);

        var yMin = (int) Math.Floor(_maze.YLenght * 2f / 5);
        var yMax = (int) Math.Ceiling(_maze.YLenght * 3f / 5);

        var xEnding = _random.Next(xMin, xMax);
        var yEnding = _random.Next(yMin, yMax);

        var endingCell = _maze.GetCell(xEnding, yEnding);

        if (endingCell is null)
            throw new ArgumentOutOfRangeException($"The range is out of the maze x:{xEnding}, y:{yEnding}");

        endingCell.IsEndingCell = true;
    }

    private void SetStartingCell()
    {
        var xRand = _random.Next(0, _maze.XLenght - 1);
        var yRand = _random.Next(0, _maze.YLenght - 1);

        var startingCell = new[]
        {
            _maze.GetCell(0, yRand),
            _maze.GetCell(_maze.XLenght - 1, yRand),
            _maze.GetCell(xRand, 0),
            _maze.GetCell(xRand, _maze.YLenght - 1)
        }.MinBy(_ => _random.Next());

        if (startingCell is null)
            return;

        startingCell.IsStartingCell = true;
    }

    public Maze<TMazeCell> Maze { get; }

    private Maze<TMazeCell> WriteMaze(int xInitial, int yInitial)
    {
        var track = new Stack<TMazeCell>();

        var initialPoint = _maze.GetCell(xInitial, yInitial);
        if (initialPoint is null)
            throw new ArgumentException("The initial point is out of the grid");

        track.Push(initialPoint);
        MarkAsExplored(initialPoint);

        var count = 1;
        while (track.Count > 0)
        {
            var currentCell = track.Peek();
            
            var nextCell = GetNextNeighbour(currentCell);
            
            if (nextCell == null)
            {
                track.Pop();
                continue;
            }

            if (_animate)
                CreateFrame(count, currentCell);

            track.Push(nextCell);
            MarkAsExplored(nextCell);
            count++;
        }

        if (_exploredRegistry.All(pair => pair.Value))
            return _maze;

        var (newXInitial, newYInitial) = _exploredRegistry.FirstOrDefault(pair => pair.Value == false).Key;

        WriteMaze(newXInitial, newYInitial);
        return _maze;
    }

    private void CreateFrame(int count, TMazeCell currentCell)
    {
        var image = MazeDrawer<TMazeCell>.CreateFrame(new DrawParams
        {
            Background = Color.Black,
            Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\Frames\\",
            Walls = Color.White,
            CellSize = 60,
            EndingCell = Color.Pink,
            StartingCell = Color.Yellow,
            WallThickness = 3,
            YLenght = _maze.YLenght,
            XLenght = _maze.XLenght,
            Count = count,
        }, currentCell, _frames.LastOrDefault());
        
        _frames.Add(image);
    }

    private TMazeCell? GetNextNeighbour(TMazeCell currentCell)
    {
        var neighbourDirections = Enum.GetValues<Directions>()
            .Where(direction => currentCell.Neighbours.HasFlag(direction))
            .OrderBy(_ => _random.Next());

        var neighbour = default(TMazeCell);

        foreach (var direction in neighbourDirections)
        {
            neighbour = CheckNeighbour(currentCell, direction);
            if (neighbour is null) 
                continue;
            currentCell.BrakeWall(neighbour, direction);
            break;
        }
        return neighbour;
    }

    private TMazeCell? CheckNeighbour(TMazeCell currentCell, Directions direction)
    {
        var neighbour = _maze.GetNeighbour(currentCell, direction);

        if (neighbour is not null && !IsExplored(neighbour))
            return neighbour;

        return null;
    }

    private bool IsExplored(TMazeCell cell)
    {
        return _exploredRegistry
            .FirstOrDefault(pair => pair.Key.Item1 == cell.XIndex && pair.Key.Item2 == cell.YIndex)
            .Value;
    }

    private void MarkAsExplored(TMazeCell? point)
    {
        if (point is null)
            return;
        var key = _exploredRegistry
            .FirstOrDefault(pair => pair.Key.Item1 == point.XIndex && pair.Key.Item2 == point.YIndex)
            .Key;

        _exploredRegistry[key] = true;
    }
}