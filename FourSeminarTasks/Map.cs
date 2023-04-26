using System.Drawing;

namespace FourSeminarTasks;

public class Map
{
    public readonly MapCell[,] Dungeon;
    public readonly Point InitialPosition;
    public readonly Point Exit;
    public readonly Dictionary<Point, Point> Portals;
    public readonly Dictionary<Point, Key> Keys;

    private static readonly Dictionary<char, MapCell> convertDoor = new()
    {
        { 'R', MapCell.RedDoor },
        { 'G', MapCell.GreenDoor},
        {'B', MapCell.BlueDoor}
    };
    
    private static readonly Dictionary<char, Key> convertKey = new()
    {
        { 'r', Key.Red },
        { 'g', Key.Green },
        {'b', Key.Blue }
    };

    private Map(MapCell[,] dungeon, Dictionary<Point, Point> portals, Dictionary<Point, Key> keys, Point initialPosition, Point exit)
    {
        Dungeon = dungeon;
        Portals = portals;
        Keys = keys;
        InitialPosition = initialPosition;
        Exit = exit;
    }

    public static Map FromText(string text)
    {
        var lines = text.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        return FromLines(lines);
    }

    public static Map FromLines(string[] lines)
    {
        var dungeon = new MapCell[lines.Length, lines[0].Length];
        var initialPosition = new Point();
        var exit = new Point();
        var portalsIndex = new Dictionary<char, Point>();
        var portals = new Dictionary<Point, Point>();
        var keys = new Dictionary<Point, Key>();

        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                var c = lines[y][x];
                switch (c)
                {
                    case '#':
                        dungeon[y, x] = MapCell.Wall;
                        break;
                    case 'P':
                        dungeon[y, x] = MapCell.Empty;
                        initialPosition = new Point(x, y);
                        break;
                    case 'E':
                        dungeon[y, x] = MapCell.Empty;
                        exit = new Point(x, y);
                        break;
                    case ' ':
                        dungeon[y, x] = MapCell.Empty;
                        break;
                    default:
                        if (Char.IsUpper(c) && convertDoor.TryGetValue(c, out var door))
                            dungeon[y, x] = door;
                        else if (Char.IsLower(c) && convertKey.TryGetValue(c, out var key))
                        {
                            keys.Add(new Point(x, y), key);
                            dungeon[y, x] = MapCell.Empty;
                        }
                        break;
                }

                if (char.IsDigit(lines[y][x]))
                {
                    if (portalsIndex.TryGetValue(lines[y][x], out var portal))
                    {
                        dungeon[y, x] = MapCell.Portal;
                        var secondPortal = new Point(x, y);
                        portals.Add(portal, secondPortal);
                        portals.Add(secondPortal, portal);
                    }
                    else
                    {
                        dungeon[y, x] = MapCell.Portal;
                        portalsIndex.Add(lines[y][x], new Point(x, y));
                    }
                }
            }
        }

        return new Map(dungeon, portals, keys, initialPosition, exit);
    }

    public bool InBounds(Point point)
        => point is { X: >= 0, Y: >= 0 }
           && Dungeon.GetLength(0) > point.Y
           && Dungeon.GetLength(1) > point.X;
}

public enum MapCell
{
    Wall,
    Empty,
    Portal,
    RedDoor,
    GreenDoor,
    BlueDoor,
}

public enum Key
{
    Red,
    Green,
    Blue
}