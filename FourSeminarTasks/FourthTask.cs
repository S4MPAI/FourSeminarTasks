using System.Drawing;

namespace FourSeminarTasks;

public class FourthTask
{
    private class PathInfo
    {
        public SinglyLinkedList<Point> Path { get; }

        public Dictionary<Key, bool> HavingKeys { get; }

        public PathInfo(SinglyLinkedList<Point> path, Dictionary<Key, bool> havingKeys = null)
        {
            Path = path;
            HavingKeys = havingKeys ??
                         new()
                         {
                             { Key.Red, false },
                             { Key.Green, false },
                             { Key.Blue, false }
                         };
        }
    }

    public static (bool haveRKey, bool haveGKey, bool haveBKey) GetKeyInfo(Dictionary<Key, bool> havingKeys) =>
        (havingKeys[Key.Red], havingKeys[Key.Green], havingKeys[Key.Blue]);
    
    private static Dictionary<MapCell, Key> doorsAndKeys = new()
    {
        { MapCell.RedDoor, Key.Red },
        { MapCell.GreenDoor, Key.Green },
        { MapCell.BlueDoor, Key.Blue }
    }; 

    public static Point[] GetPathWithDoors(Map map, Dictionary<Point, Key> keys, Point start, Point finish)
    {
        var pathsInfo = new Queue<PathInfo>();
        pathsInfo.Enqueue(new PathInfo(new SinglyLinkedList<Point>(start, null)));
        var directions = new Size[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        var visited = new HashSet<(Point point, (bool haveRKey, bool haveGKey, bool haveBKey))>{(start, (false, false, false))};

        while (pathsInfo.Count != 0)
        {
            var pathInfo = pathsInfo.Dequeue();
            var point = pathInfo.Path.Value;

            if (point == finish) return pathInfo.Path.Reverse().ToArray();

            var nextPoints = directions
                .Select(size => point + size)
                .Where(x => map.InBounds(x) && map.Dungeon[x.Y, x.X] != MapCell.Wall);

            foreach (var nextPoint in nextPoints)
            {
                if(doorsAndKeys.TryGetValue(map.Dungeon[nextPoint.Y, nextPoint.X], out var neededKey) && 
                   pathInfo.HavingKeys[neededKey] == false) continue;

                Dictionary<Key, bool> havingKeys = new(pathInfo.HavingKeys);

                if (keys.TryGetValue(nextPoint, out var key))
                    havingKeys[key] = true;

                var keyInfo = GetKeyInfo(havingKeys);
                if (visited.Contains((nextPoint, keyInfo))) 
                    continue;

                visited.Add((nextPoint, keyInfo));
                pathsInfo.Enqueue(new PathInfo(new SinglyLinkedList<Point>(nextPoint, pathInfo.Path), havingKeys));
            }
        }
        
        return Array.Empty<Point>();
    }
}