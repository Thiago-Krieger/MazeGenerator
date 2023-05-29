#pragma warning disable CA1416
using System.Drawing;
using Point = System.Drawing.Point;

namespace MazeGenerator;

public class HexagonalCell : AbstractMazeCell, IDrawableCell
{
    public HexagonalCell(int xIndex, int yIndex)
        : base(xIndex, yIndex, 6, Directions.N | Directions.S | Directions.E | Directions.W)
    {
        Wals = Directions.N | Directions.S | Directions.Ne | Directions.Nw | Directions.Se | Directions.Sw;
    }

    private CellVertex SetVertices(int cellSize)
    {
        var centerX = 1.5 * XIndex + 1;
        var centerY = XIndex % 2 == 0 
            ? YIndex * Math.Sqrt(3) + Math.Sqrt(3)/2 
            : YIndex * Math.Sqrt(3) + Math.Sqrt(3);
        const double angle = Math.PI / 3;
        var radius = (cellSize / 2f) / Math.Cos(Math.PI / Sides);

        var vertices = new CellVertex();
        
        for (var i = 0; i < Sides; i++)
        {
            var vertex = new Point(
                (int) Math.Round(centerX * radius + Math.Cos(angle * i) * radius),
                (int) Math.Round(centerY * radius + Math.Sin(angle * i) * radius)); 
            if (i == 0)
                vertices.Right = vertex;
            if (i == 1)
                vertices.TopRight = vertex;
            if (i == 2)
                vertices.TopLeft = vertex;
            if (i == 3)
                vertices.Left = vertex;
            if (i == 4)
                vertices.BottomLeft = vertex;
            if (i == 5)
                vertices.BottomRight = vertex;
        }

        return vertices;
    }

    public void Draw(Graphics graphics, Pen pen, DrawParams drawParams)
    {
        var vertices = SetVertices(drawParams.CellSize);

        if (vertices is null)
            throw new InvalidOperationException("The vertices can't be null");
        
        var topLeft = vertices.TopLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.TopLeft}");
        var topRight = vertices.TopRight ?? throw new InvalidOperationException($"The vertex is null {vertices.TopRight}");
        var bottomRight = vertices.BottomRight ?? throw new InvalidOperationException($"The vertex is null {vertices.BottomLeft}");
        var bottomLeft = vertices.BottomLeft ?? throw new InvalidOperationException($"The vertex is null {vertices.BottomRight}");
        var left = vertices.Left ?? throw new InvalidOperationException($"The vertex is null {vertices.Left}");
        var right = vertices.Right ?? throw new InvalidOperationException($"The vertex is null {vertices.Right}");

        var verticesArray = new [] {topLeft, topRight, right, bottomRight, bottomLeft, left};
        if(IsEndingCell || IsStartingCell)
            SetCellBackground(graphics, verticesArray, drawParams);

        if(Wals.HasFlag(Directions.N))
            graphics.DrawLine(pen, topLeft, topRight);
        
        if(Wals.HasFlag(Directions.S))
            graphics.DrawLine(pen, bottomLeft, bottomRight);
        
        if(Wals.HasFlag(Directions.Ne))
            graphics.DrawLine(pen, topRight, right);
        
        if(Wals.HasFlag(Directions.Sw))
            graphics.DrawLine(pen, left, bottomLeft);
        
        if(Wals.HasFlag(Directions.Se))
            graphics.DrawLine(pen, right, bottomRight);
        
        if(Wals.HasFlag(Directions.Nw))
            graphics.DrawLine(pen, left, topLeft);
        
    }

    public override void BrakeWall(IMazeCell neighbour, Directions _)
    {
        var neighbourDirection = GetNeighbourDirection(neighbour);

        RemoveWall(this, neighbourDirection);
        RemoveWall(neighbour, neighbourDirection.GetOppositeDirection());
    }

    private Directions GetNeighbourDirection(IMazeCell neighbour)
    {
        if (XIndex == neighbour.XIndex && YIndex + 1 == neighbour.YIndex)
            return Directions.N;

        if (XIndex == neighbour.XIndex && YIndex - 1 == neighbour.YIndex)
            return Directions.S;

        if (YIndex == neighbour.YIndex && XIndex + 1 == neighbour.XIndex && XIndex % 2 == 0)
            return Directions.Ne;
        
        if (YIndex == neighbour.YIndex && XIndex + 1 == neighbour.XIndex && XIndex % 2 != 0)
            return Directions.Se;
        
        if (YIndex == neighbour.YIndex && XIndex - 1 == neighbour.XIndex && XIndex % 2 == 0)
            return Directions.Nw;
        
        if (YIndex == neighbour.YIndex && XIndex - 1 == neighbour.XIndex && XIndex % 2 != 0)
            return Directions.Sw;

        return Directions.N;
    }
}