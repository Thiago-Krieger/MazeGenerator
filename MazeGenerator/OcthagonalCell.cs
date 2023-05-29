#pragma warning disable CA1416
using System.Drawing;
using Point = System.Drawing.Point;

namespace MazeGenerator;

public class OcthagonalCell : AbstractMazeCell, IDrawableCell
{
    public OcthagonalCell(int xIndex, int yIndex) : base(xIndex, yIndex, sides: 8,
        neighbourDirections: Directions.E | Directions.N | Directions.Ne | Directions.S | Directions.Se | Directions.Sw |
                    Directions.W | Directions.Nw)
    { }

    private CellVertex SetVertices(int cellSize)
    {
        var centerX = cellSize/2 + XIndex*cellSize;
        var centerY  = cellSize/2 + YIndex*cellSize;
        var radius = cellSize/2f / Math.Cos(Math.PI / Sides);
        var angle = 2 * Math.PI / Sides;
        var angleOffset = angle / 2;
        
        var vertices = new CellVertex();
        
        for (var i = 0; i < Sides; i++)
        {
            var xPoint = (int) Math.Round(centerX + Math.Cos(angle * i + angleOffset) * radius);
            var yPoint = (int) Math.Round(centerY + Math.Sin(angle * i + angleOffset) * radius); 
            var vertex = new Point(xPoint, yPoint); 
            if (i == 0)
                vertices.SideTopRight = vertex;
            if (i == 1)
                vertices.TopRight = vertex;
            if (i == 2)
                vertices.TopLeft = vertex;
            if (i == 3)
                vertices.SideTopLeft = vertex;
            if (i == 4)
                vertices.SideBottomLeft = vertex;
            if (i == 5)
                vertices.BottomLeft = vertex;
            if (i == 6)
                vertices.BottomRight = vertex;
            if (i == 7)
                vertices.SideBottomRight = vertex;
        }

        return vertices;
    }

    public void Draw(Graphics graphics, Pen pen, DrawParams drawParams)
    {
        var vertices = SetVertices(drawParams.CellSize);
        
        var topLeft = vertices.TopLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.TopLeft}");
        var topRight = vertices.TopRight ?? throw new InvalidOperationException($"The vertex is null {vertices.TopRight}");
        var bottomRight = vertices.BottomRight ?? throw new InvalidOperationException($"The vertex is null {vertices.BottomLeft}");
        var bottomLeft = vertices.BottomLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.BottomRight}");
        
        var sideTopLeft = vertices.SideTopLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.SideTopLeft}");
        var sideTopRight = vertices.SideTopRight ?? throw new InvalidOperationException($"The vertex is null {vertices.SideTopRight}");
        var sideBottomRight = vertices.SideBottomRight ?? throw new InvalidOperationException($"The vertex is null {vertices.SideBottomRight}");
        var sideBottomLeft = vertices.SideBottomLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.SideBottomLeft}");

        var verticesArray = new [] {topLeft, topRight, sideTopRight, sideBottomRight, bottomRight, bottomLeft, sideBottomLeft, sideTopLeft};
        if(IsEndingCell || IsStartingCell)
            SetCellBackground(graphics, verticesArray, drawParams);

        if(Wals.HasFlag(Directions.N))
            graphics.DrawLine(pen, topLeft, topRight);
        
        if(Wals.HasFlag(Directions.E))
            graphics.DrawLine(pen, sideTopRight, sideBottomRight);
        
        if(Wals.HasFlag(Directions.S))
            graphics.DrawLine(pen, bottomLeft, bottomRight);
        
        if(Wals.HasFlag(Directions.W))
            graphics.DrawLine(pen, sideBottomLeft, sideTopLeft);
        
        if(Wals.HasFlag(Directions.Ne))
            graphics.DrawLine(pen, topRight, sideTopRight);
        
        if(Wals.HasFlag(Directions.Sw))
            graphics.DrawLine(pen, sideBottomLeft, bottomLeft);
        
        if(Wals.HasFlag(Directions.Se))
            graphics.DrawLine(pen, sideBottomRight, bottomRight);
        
        if(Wals.HasFlag(Directions.Nw))
            graphics.DrawLine(pen, sideTopLeft, topLeft);
    }
}