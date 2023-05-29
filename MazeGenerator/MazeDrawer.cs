#pragma warning disable CA1416
using System.Drawing;
using System.Drawing.Imaging;
using Color = System.Drawing.Color;

namespace MazeGenerator;

public class MazeDrawer<T>
    where T : class, IDrawableCell
{
    public static void Draw(DrawParams<T> drawParams)
    {
        if (drawParams.Maze is null)
            throw new InvalidOperationException($"The maze is null {drawParams.Maze}");

        var width = drawParams.CellSize * drawParams.Maze.XLenght;
        var height = drawParams.CellSize * drawParams.Maze.YLenght + 60;

        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(drawParams.Background);
        DrawCells(graphics, drawParams);
        graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
        bitmap.Save(drawParams.Path, ImageFormat.Bmp);
    }

    private static void DrawCells(Graphics graphics, DrawParams<T> drawParams)
    {
        if (drawParams.Maze is null)
            throw new InvalidOperationException($"The maze is null {drawParams.Maze}");

        using var pen = new Pen(drawParams.Walls, drawParams.WallThickness);
        for (var row = 0; row < drawParams.Maze.XLenght; row++)
        for (var column = 0; column < drawParams.Maze.YLenght; column++)
        {
            if (drawParams.Maze.GetCell(row, column) is IDrawableCell drawableCell)
                drawableCell.Draw(graphics, pen, drawParams);
        }
    }

    public static void CreateFrame(DrawParams drawParams, T lastCell)
    {
        var width = drawParams.CellSize * drawParams.XLenght;
        var height = drawParams.CellSize * drawParams.YLenght;

        using var bitmap = (lastCell.XIndex == 0 && lastCell.YIndex == 0)
            ? new Bitmap(width, height)
            : Bitmap.FromFile(drawParams.Path + $"{drawParams.Count - 1}.bmp");

        using var graphics = Graphics.FromImage(bitmap);
        if(lastCell.XIndex == 0 && lastCell.YIndex == 0)
            graphics.Clear(drawParams.Background);

        using var pen = new Pen(drawParams.Walls, drawParams.WallThickness);

        var fileName = $"{drawParams.Count}.bmp";
        if (lastCell is IDrawableCell drawableCell)
            drawableCell.Draw(graphics, pen, drawParams);

        graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
        bitmap.Save(drawParams.Path + fileName, ImageFormat.Bmp);
        
        Thread.Sleep(50);
    }
}

public class DrawParams<T> : DrawParams
    where T : class, IDrawableCell
{
    public Maze<T>? Maze { get; set; }
}

public class DrawParams
{
    public Color Background { get; set; }
    public Color Walls { get; set; }
    public Color StartingCell { get; set; }
    public Color EndingCell { get; set; }

    public int CellSize { get; set; }
    public int WallThickness { get; set; }

    public int YLenght { get; set; }
    public int XLenght { get; set; }
    public int Count { get; set; }
    public string Path { get; set; } = string.Empty;
}