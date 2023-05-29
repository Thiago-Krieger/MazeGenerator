namespace MazeGenerator;

public class Factory<TMazeCell>
    where TMazeCell : class, IMazeCell
{
    public TMazeCell GetInstance(params object[] args)
    {
        var instance =  Activator.CreateInstance(typeof(TMazeCell),args) as TMazeCell;
        return instance!;
    }
}