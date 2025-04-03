using System.Drawing;
using MazeGenerator;



var rows = 10;
var columns = 10;
        
var mazeGenerator = new BackTracker<SquareCell>(rows, columns, false);

MazeDrawer<SquareCell>.Draw(new DrawParams<SquareCell>
{
    Background = Color.Black,
    Maze = mazeGenerator.Maze,
    Path = $@"F:\MazeGenerator\RandomizedPrism\SqareCell\{GetFileName(mazeGenerator)}.png",
    Walls = Color.White,
    CellSize = 60,
    EndingCell = Color.Pink,
    StartingCell = Color.Yellow,
    WallThickness = 3,
});

void GenerateSingleWithAnimation<T>()
    where T : class, IDrawableCell
{
    var _ = new BackTracker<T>(40,30, true);
}

void GenerateBulk()
{
    for (var i = 0; i < 30; i++)
    {
        var mazeGenerator = new BackTracker<OcthagonalCell>(64,32, false);
        MazeDrawer<OcthagonalCell>.Draw(new DrawParams<OcthagonalCell>
        {
            Background = Color.Black,
            Maze = mazeGenerator.Maze,
            Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\OcthagonalCell\\{i}-{GetFileName(mazeGenerator)}.bmp",
            Walls = Color.White,
            CellSize = 60,
            EndingCell = Color.Pink,
            StartingCell = Color.Yellow,
            WallThickness = 3,
        });
        
        var squareMazeGenerator = new BackTracker<SquareCell>(64,32, false);
        MazeDrawer<SquareCell>.Draw(new DrawParams<SquareCell>
        {
            Background = Color.Black,
            Maze = squareMazeGenerator.Maze,
            Path = $"C:\\Users\\ThiagoRobsonKrieger\\RiderProjects\\MazeGenerator\\BackTracker\\SquareCell\\{i}{GetFileName(squareMazeGenerator)}.bmp",
            Walls = Color.White,
            CellSize = 60,
            EndingCell = Color.Pink,
            StartingCell = Color.Yellow,
            WallThickness = 3,
        });
        
        var hexMazeGenerator = new BackTracker<HexagonalCell>(64,32, false);
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
    }
}


string GetFileName<T>(IMazeGenerator<T> generator) where T : class, IDrawableCell
{
    var generatorName = generator.GetType().GetGenericTypeDefinition().Name.Split("`")[0];
    var cellType = generator.GetType().GetGenericArguments()[0].Name;
    var xSize = generator.Maze.XLenght;
    var ySize = generator.Maze.YLenght;
    var hash = generator.Maze.GetHashCode();

    return generatorName + "-" + cellType + "-" + xSize + "x" + ySize + "-" + hash;
}
