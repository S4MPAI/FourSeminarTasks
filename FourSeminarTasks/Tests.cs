using NUnit.Framework;
using System.Drawing;

namespace FourSeminarTasks;

[TestFixture]
public class ThirdTaskTests
{
    [Test]
    public void ReturnEmptyPath()
    {
        var textMap = new[]
        {
            "########",
            "######1#",
            "########",
            "P1     E",
            "########"
        };
        var map = Map.FromLines(textMap);

        var path = ThirdTask.GetPathWithPortal(map, map.Portals, map.InitialPosition, map.Exit);

        Assert.AreEqual(Array.Empty<Point>(), path);
    }

    [Test]
    public void ReturnPathOnSimpleMaze()
    {
        var textMap = new[]
        {
            "########",
            "#  12  #",
            "#      #",
            "P1    2E",
            "########"
        };
        var bestPathLength = 6;
        var map = Map.FromLines(textMap);

        var path = ThirdTask.GetPathWithPortal(map, map.Portals, map.InitialPosition, map.Exit);

        Assert.AreEqual(bestPathLength, path.Length);
    }

    [Test]
    public void ReturnPathOnBigMaze()
    {
        var textMap = new[]
        {
            "#########################",
            "#     ##  2# 4  ######  #",
            "# ##  ##   #  ###    #  #",
            "#1## 3##      ###    #  #",
            "##2###        ### ## #  #",
            "## #           3  ##6#  #",
            "## #     5     ##########",
            "## ###         #     6  #",
            "P    1#    4   #5       E",
            "#########################"
        };
        var bestPathLength = 23;
        var map = Map.FromLines(textMap);

        var path = ThirdTask.GetPathWithPortal(map, map.Portals, map.InitialPosition, map.Exit);

        Assert.AreEqual(bestPathLength, path.Length);
    }
}

[TestFixture]
public class FourthTaskTests
{
    [Test]
    public void ReturnEmptyPath()
    {
        var textMap = new[]
        {
            "########",
            "P     RE",
            "########"
        };
        var map = Map.FromLines(textMap);

        var path = FourthTask.GetPathWithDoors(map, map.Keys, map.InitialPosition, map.Exit);

        Assert.AreEqual(Array.Empty<Point>(), path);
    }

    [Test]
    public void ReturnPathOnSimpleMaze1()
    {
        var textMap = new[]
        {
            "########",
            "P r   RE",
            "########",
        };
        var bestPathLength = 8;
        var map = Map.FromLines(textMap);

        var path = FourthTask.GetPathWithDoors(map, map.Keys, map.InitialPosition, map.Exit);

        Assert.AreEqual(bestPathLength, path.Length);
    }

    [Test]
    public void ReturnPathOnSimpleMaze2()
    {
        var textMap = new[]
        {
            "########",
            "#rg    #",
            "# #### #",
            "P ####RE",
            "# #### #",
            "#gb    #",
            "########"
        };
        var bestPathLength = 12;
        var map = Map.FromLines(textMap);

        var path = FourthTask.GetPathWithDoors(map, map.Keys, map.InitialPosition, map.Exit);

        Assert.AreEqual(bestPathLength, path.Length);
    }

    [Test]
    public void ReturnPathOnSimpleMaze3()
    {
        var textMap = new[]
        {
            "########",
            "###b####",
            "#r# ####",
            "# #G####",
            "P R   BE",
            "### ####",
            "###g####",
            "########"
        };
        var bestPathLength = 22;
        var map = Map.FromLines(textMap);

        var path = FourthTask.GetPathWithDoors(map, map.Keys, map.InitialPosition, map.Exit);

        Assert.AreEqual(bestPathLength, path.Length);
    }
}