namespace MazeGenerator;

public sealed class RandomizedPrism<TMazeCell> : IMazeGenerator<TMazeCell> 
    where TMazeCell : class, IMazeCell
{
    private int _numRows;
    private int _numColumns;
    private readonly Maze<TMazeCell> _maze;
    private readonly Random _random;
    private readonly IDictionary<Tuple<int, int>, bool> _exploredRegistry;

    public Maze<TMazeCell> Maze { get; }

    public RandomizedPrism(int rows, int columns)
    {
        _numRows = rows;
        _numColumns = columns;
        _maze = new Maze<TMazeCell>(rows, columns);
        _random = new Random();
        
        _exploredRegistry = new Dictionary<Tuple<int, int>, bool>();
        for (var row = 0; row < rows; row++)
        for (var column = 0; column < columns; column++)
            _exploredRegistry.Add(new KeyValuePair<Tuple<int, int>, bool>(new Tuple<int, int>(row, column), false));

        Maze = GenerateMaze();
    }
    
    private Maze<TMazeCell> GenerateMaze()
    {
        // Start with a random cell
        int startRow = _random.Next(_numRows);
        int startColumn = _random.Next(_numColumns);

        var startCell = _maze.GetCell(startRow, startColumn);
        if (startCell is null)
            return _maze;
        
        MarkAsExplored(startCell);

        // Create a list to store frontier cells
        var frontier = new List<TMazeCell>();
        frontier.Add(startCell);

        while (frontier.Count > 0)
        {
            // Choose a random frontier cell
            int randomIndex = _random.Next(frontier.Count);
            var currentCell = frontier[randomIndex];

            // Get the neighboring cells
            List<TMazeCell> neighbors = GetNeighbors(currentCell);

            // Choose a random neighbor
            int neighborIndex = _random.Next(neighbors.Count);

            if (neighborIndex > neighbors.Count - 1)
                break;
            var neighborCell = neighbors[neighborIndex];

            if (!IsExplored(neighborCell))
            {
                // Remove the wall between current cell and neighbor cell
                RemoveWall(currentCell, neighborCell);

                MarkAsExplored(neighborCell);

                // Add the neighbor cell to the frontier
                frontier.Add(neighborCell);
            }

            // Remove the current cell from the frontier
            frontier.Remove(currentCell);
        }

        return _maze;
    }

    private List<TMazeCell> GetNeighbors(TMazeCell cell)
    {
        var neighbors = new List<TMazeCell>();


        // Check top neighbor
        if (cell.YIndex > 0 && !IsExplored(_maze.GetCell(cell.YIndex - 1, cell.XIndex)))
        {
            neighbors.Add(_maze.GetCell(cell.YIndex - 1, cell.XIndex));
        }

        // Check right neighbor
        if (cell.XIndex < _numColumns - 1 && !IsExplored(_maze.GetCell(cell.YIndex, cell.XIndex + 1)))
        {
            neighbors.Add(_maze.GetCell(cell.YIndex, cell.XIndex + 1));
        }

        // Check bottom neighbor
        if (cell.YIndex < _numRows - 1 && !IsExplored(_maze.GetCell(cell.YIndex + 1, cell.XIndex)))
        {
            neighbors.Add(_maze.GetCell(cell.YIndex + 1, cell.XIndex));
        }

        // Check left neighbor
        if (cell.XIndex > 0 && !IsExplored(_maze.GetCell(cell.YIndex, cell.XIndex - 1)))
        {
            neighbors.Add(_maze.GetCell(cell.YIndex, cell.XIndex - 1));
        }

        return neighbors;
    }

    private void RemoveWall(TMazeCell currentCell, TMazeCell neighborCell)
    {
        // Remove the wall between the cells
        if (currentCell.YIndex < neighborCell.YIndex)
        {
            currentCell.BrakeWall(neighborCell, Directions.S); // Remove the bottom wall of the current cell
            // neighborCell.Wall = false; // Remove the top wall of the neighbor cell
        }
        else if (currentCell.YIndex > neighborCell.YIndex)
        {
            currentCell.BrakeWall(neighborCell, Directions.N); // Remove the top wall of the current cell
            // neighborCell.Wall = false; // Remove the bottom wall of the neighbor cell?
        }
        else if (currentCell.XIndex < neighborCell.XIndex)
        {
            currentCell.BrakeWall(neighborCell, Directions.E); // Remove the right wall of the current cell
            // neighborCell.Wall = false; // Remove the left wall of the neighbor cell
        }
        else if (currentCell.XIndex > neighborCell.XIndex)
        {
            currentCell.BrakeWall(neighborCell, Directions.W); // Remove the left wall of the current cell
            // neighborCell.Wall = false; // Remove the right wall of the neighbor cell
        }
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
    
    private bool IsExplored(TMazeCell cell)
    {
        return _exploredRegistry
            .FirstOrDefault(pair => pair.Key.Item1 == cell.XIndex && pair.Key.Item2 == cell.YIndex)
            .Value;
    }
}