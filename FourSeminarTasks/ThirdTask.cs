using System.Drawing;

namespace FourSeminarTasks;

public class ThirdTask
{
    public static Point[] GetPathWithPortal(Map map, Dictionary<Point, Point> portals, Point start, Point finish)
    {
        var paths = new Queue<SinglyLinkedList<Point>>();
        paths.Enqueue(new SinglyLinkedList<Point>(start, null));
        var directions = new Size[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        var visited = new HashSet<Point>{start};

        while (paths.Count != 0)
        {
            var path = paths.Dequeue();
            var point = path.Value;

            if (point == finish) return path.Reverse().ToArray();

            var nextPoints = directions
                .Select(size => point + size)
                .Where(x => !visited.Contains(x) && map.InBounds(x) && map.Dungeon[x.Y, x.X] != MapCell.Wall);

            foreach (var nextPoint in nextPoints)
            {
                visited.Add(nextPoint);
                
                if (portals.TryGetValue(nextPoint, out var portalPoint) && !visited.Contains(portalPoint))
                {
                    paths.Enqueue(new SinglyLinkedList<Point>(portalPoint, new SinglyLinkedList<Point>(nextPoint, path)));
                    visited.Add(portalPoint);
                }
                else
                    paths.Enqueue(new SinglyLinkedList<Point>(nextPoint, path));
            }
        }
        
        return Array.Empty<Point>();
    }
}