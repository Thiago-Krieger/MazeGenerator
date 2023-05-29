using System.Drawing;
using MazeGenerator;


for (var i = 0; i < 30; i++)
{
    // var mazeGenerator = new BackTracker<OcthagonalCell>(64,32);
    // MazeDrawer<OcthagonalCell>.Draw(new DrawParams<OcthagonalCell>
    // {
    //     Background = Color.Black,
    //     Maze = mazeGenerator.Maze,
    //     Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\OcthagonalCell\\{i}-{GetFileName(mazeGenerator)}.bmp",
    //     Walls = Color.White,
    //     CellSize = 60,
    //     EndingCell = Color.Pink,
    //     StartingCell = Color.Yellow,
    //     WallThickness = 3,
    // });
    //
    // var squareMazeGenerator = new BackTracker<SquareCell>(64,32);
    // MazeDrawer<SquareCell>.Draw(new DrawParams<SquareCell>
    // {
    //     Background = Color.Black,
    //     Maze = squareMazeGenerator.Maze,
    //     Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\SquareCell\\{i}{GetFileName(squareMazeGenerator)}.bmp",
    //     Walls = Color.White,
    //     CellSize = 60,
    //     EndingCell = Color.Pink,
    //     StartingCell = Color.Yellow,
    //     WallThickness = 3,
    // });
    
    var hexMazeGenerator = new BackTracker<HexagonalCell>(64,32);
    MazeDrawer<HexagonalCell>.Draw(new DrawParams<HexagonalCell>
    {
        Background = Color.Black,
        Maze = hexMazeGenerator.Maze,
        Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\HexagonalCell\\{i}{GetFileName(hexMazeGenerator)}.bmp",
        Walls = Color.White,
        CellSize = 60,
        EndingCell = Color.Pink,
        StartingCell = Color.Yellow,
        WallThickness = 3,
    });

    string GetFileName<T>(BackTracker<T> generator) where T : class, IDrawableCell
    {
        var generatorName = generator.GetType().GetGenericTypeDefinition().Name.Split("`")[0];
        var cellType = generator.GetType().GetGenericArguments()[0].Name;
        var xSize = generator.Maze.XLenght;
        var ySize = generator.Maze.YLenght;
        var hash = generator.Maze.GetHashCode();

        return generatorName + "-" + cellType + "-" + xSize + "x" + ySize + "-" + hash;
    }
}



