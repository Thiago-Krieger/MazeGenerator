#pragma warning disable CA1416
using System.Drawing;
using Point = System.Drawing.Point;

namespace MazeGenerator;

public interface IMazeCell
{
    int XIndex { get; }
    int YIndex { get; }
    bool IsStartingCell { get; set; }
    bool IsEndingCell { get; set; }
    int Sides { get; }
    public Directions Wals { get; set; }
    Directions Neighbours { get; }
    void BrakeWall(IMazeCell neighbour, Directions directions);
}

public interface IDrawableMaze
{
    public int CellSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public interface IDrawableCell : IMazeCell
{
    void Draw(Graphics graphics, Pen pen, DrawParams drawParams);
}

public class CellVertex
{
    public Point? BottomLeft { get; set; }
    public Point? BottomRight { get; set; }
    public Point? TopLeft { get; set; }
    public Point? TopRight { get; set; }
    
    public Point? SideBottomLeft { get; set; }
    public Point? SideBottomRight { get; set; }
    public Point? SideTopLeft { get; set; }
    public Point? SideTopRight { get; set; }
    
    public Point? Left { get; set; }
    public Point? Right { get; set; }
}

public class AbstractMazeCell : IMazeCell
{
    public AbstractMazeCell(int xIndex, int yIndex, int sides, Directions neighbourDirections)
    {
        XIndex = xIndex;
        YIndex = yIndex;
        Sides = sides;
        Neighbours = neighbourDirections;
        Wals = neighbourDirections;
    }

    public int XIndex { get; }
    public int YIndex { get; }
    public bool IsStartingCell { get; set; }
    public bool IsEndingCell { get; set; }
    public int Sides { get; }
    public Directions Wals { get; set; }
    public Directions Neighbours { get; }

    public virtual void BrakeWall(IMazeCell neighbour, Directions directions)
    {
        RemoveWall(this, directions);
        RemoveWall(neighbour, directions.GetOppositeDirection());
    }
    
    protected void RemoveWall(IMazeCell cell, Directions wall)
    {
        if (cell.Wals.HasFlag(wall))
            cell.Wals &= ~ wall;
    }
    
    protected void SetCellBackground(Graphics graphics, Point[] vertices, DrawParams drawParams)
    {
        var background = 
            IsEndingCell 
                ? new SolidBrush(drawParams.EndingCell) 
                : new SolidBrush(drawParams.StartingCell);

        graphics.FillPolygon(background, vertices);
    }
}