#pragma warning disable CA1416
using System.Drawing;
using Point = System.Drawing.Point;

namespace MazeGenerator;

public sealed class SquareCell : AbstractMazeCell, IDrawableCell
{
    public SquareCell(int xIndex, int yIndex) 
        : base(xIndex, yIndex, sides: 4, Directions.E | Directions.N | Directions.S | Directions.W)
    { }

    private CellVertex SetVertices(int cellSize)
    {
        var centerX = cellSize/2 + XIndex*cellSize;
        var centerY  = cellSize/2 + YIndex*cellSize;
        var radius = (cellSize / 2f) / Math.Sin(Math.PI / 4);
        var angle = 2 * Math.PI / Sides;

        var vertices = new CellVertex();
        
        for (var i = 0; i < Sides; i++)
        {
            var xPoint = (int) Math.Round(centerX + Math.Cos(angle * i + Math.PI/4) * radius);
            var yPoint = (int) Math.Round(centerY + Math.Sin(angle * i + Math.PI/4) * radius); 
            var vertex = new Point(xPoint, yPoint); 
            if (i == 0)
                vertices.TopRight = vertex;
            if (i == 1)
                vertices.TopLeft = vertex;
            if (i == 2)
                vertices.BottomLeft = vertex;
            if (i == 3)
                vertices.BottomRight = vertex;
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

        var verticesArray = new [] {topLeft, topRight, bottomRight, bottomLeft};
        if(IsEndingCell || IsStartingCell)
            SetCellBackground(graphics, verticesArray, drawParams);

        if(Wals.HasFlag(Directions.N))
            graphics.DrawLine(pen, topLeft, topRight);
        
        if(Wals.HasFlag(Directions.E))
            graphics.DrawLine(pen, topRight, bottomRight);
        
        if(Wals.HasFlag(Directions.S))
            graphics.DrawLine(pen, bottomLeft, bottomRight);
        
        if(Wals.HasFlag(Directions.W))
            graphics.DrawLine(pen, bottomLeft, topLeft);
    }
}