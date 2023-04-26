namespace FourSeminarTasks;

public class Node
{
    public int Value;

    public Node From;
        
    public List<Node> NearByNodes { get; } = new();

    public Node(int value) => Value = value;

    public void AddNearByNode(Node node) => NearByNodes.Add(node);
}

public static class PenguinAvia
{
    public class FlightsInfo
    {
        public HashSet<(int firstAirport, int secondAirport)> AddedFligths { get; }
        
        public HashSet<(int firstAirport, int secondAirport)> DeletedFlights { get; }

        public int TotalPrice { get; }
        
        public FlightsInfo(HashSet<(int firstAirport, int secondAirport)> addedFligths,
            HashSet<(int firstAirport, int secondAirport)> deletedFlights,
            int totalPrice)
        {
            AddedFligths = addedFligths;
            DeletedFlights = deletedFlights;
            TotalPrice = totalPrice;
        }
    }

    private static int AddFlightPrice;

    private static int DeleteFlightPrice;

    private static Dictionary<int, Node> AllNodes;

    private static int AirportsCount;

    public static void GetNewFlights()
    {
        GetInput();

        var flightsInfo = GetDeletedAndAddedFlights();

        PrintResult(flightsInfo);
    }

    private static void GetInput()
    {
        AirportsCount = int.Parse(Console.ReadLine());
        var flightsPrice = Console.ReadLine().Split().Select(int.Parse).ToArray();
        (DeleteFlightPrice, AddFlightPrice) = (flightsPrice[0], flightsPrice[1]);

        var airportFlightsInfo = new string[AirportsCount];
        AllNodes = new Dictionary<int, Node>();
        for (var i = 0; i < AirportsCount; i++)
        {
            AllNodes.Add(i, new Node(i));
            airportFlightsInfo[i] = Console.ReadLine();
        }

        for (var i = 0; i < AirportsCount; i++)
        for (var j = i + 1; j < AirportsCount; j++)
        {
            if (airportFlightsInfo[j][i] == '0') continue;

            var firstNode = AllNodes[i];
            var secondNode = AllNodes[j];

            firstNode.AddNearByNode(secondNode);
            secondNode.AddNearByNode(firstNode);
        }
    }
    
    private static FlightsInfo GetDeletedAndAddedFlights()
    {
        var deletedFlights = new HashSet<(int firstAirport, int secondAirport)>();
        var addedFlights = new HashSet<(int firstAirport, int secondAirport)>();
        var visited = new HashSet<int>();

        var totalPrice = 0;
        Node currentNode = null;
        while (visited.Count != AirportsCount)
        {
            var lastNode = AllNodes.Values.First(x => !visited.Contains(x.Value));

            if (currentNode != null)
            {
                addedFlights.Add((lastNode.Value, currentNode.Value));
                totalPrice += AddFlightPrice;
            }

            currentNode = lastNode;

            var queue = new Queue<Node>();
            queue.Enqueue(currentNode);
            visited.Add(currentNode.Value);

            while (queue.Count != 0)
            {
                currentNode = queue.Dequeue();

                foreach (var nextNode in currentNode.NearByNodes.Where(x => x.Value != (currentNode.From?.Value ?? -1)))
                {
                    if (visited.Contains(nextNode.Value))
                    {
                        if (deletedFlights.Contains((nextNode.Value, currentNode.Value))) continue;

                        deletedFlights.Add((currentNode.Value, nextNode.Value));
                        totalPrice += DeleteFlightPrice;
                        continue;
                    }

                    nextNode.From = currentNode;
                    queue.Enqueue(nextNode);
                    visited.Add(nextNode.Value);
                }
            }
        }

        return new FlightsInfo(addedFlights, deletedFlights, totalPrice);
    }

    private static void PrintResult(FlightsInfo flightsInfo)
    {
        var result = new char[AirportsCount][];
        for (int i = 0; i < AirportsCount; i++)
            result[i] = Enumerable.Repeat('0', AirportsCount).ToArray();

        foreach (var flight in flightsInfo.AddedFligths)
        {
            result[flight.firstAirport][flight.secondAirport] = 'a';
            result[flight.secondAirport][flight.firstAirport] = 'a';
        }

        foreach (var flight in flightsInfo.DeletedFlights)
        {
            result[flight.firstAirport][flight.secondAirport] = 'd';
            result[flight.secondAirport][flight.firstAirport] = 'd';
        }

        Console.WriteLine(flightsInfo.TotalPrice);
        foreach (var line in result)
            Console.WriteLine(line);
    }
}

public class SubwayChase
{
    
    
    public static void GetHidingPlaces()
    {
        var(allNodes, criminalPath) = GetInput();

        var queue = new Queue<Node>();
        var visited = new HashSet<int>();
        var hidingPlaces = new HashSet<int>{criminalPath[^1]};
        
        queue.Enqueue(allNodes[criminalPath[0]]);
        visited.Add(criminalPath[0]);

        while (queue.Count != 0)
        {
            var node = queue.Dequeue();
            if(hidingPlaces.Contains(node.From?.Value ?? -1))
                hidingPlaces.Add(node.Value);

            foreach (var nextNodes in node.NearByNodes
                         .Where(x => !visited.Contains(x.Value))
                         .OrderBy(x => !criminalPath.Contains(x.Value)).ToArray())
            {
                nextNodes.From = node;
                
                queue.Enqueue(nextNodes);
                visited.Add(nextNodes.Value);
            }
        }

        foreach (var place in hidingPlaces.Order())
            Console.WriteLine(place);
    }

    private static (Dictionary<int, Node>, int[]) GetInput()
    {
        var linesCount = int.Parse(Console.ReadLine());
        var allNodes = new Dictionary<int, Node>();

        for (int i = 0; i < linesCount; i++)
        {
            var stations = Console.ReadLine().Split().Skip(1).Select(int.Parse).ToArray();
            var stationsCount = stations.Length;

            var currentNode = GetNode(allNodes, stations[0]);

            for (int j = 1; j < stationsCount; j++)
            {
                var nextNode = GetNode(allNodes, stations[j]);

                nextNode.AddNearByNode(currentNode);
                currentNode.AddNearByNode(nextNode);
                currentNode = nextNode;
            }
        }

        var criminalPath = Console.ReadLine().Split().Skip(1).Select(int.Parse).ToArray();

        return (allNodes, criminalPath);
    }

    private static Node GetNode(Dictionary<int, Node> allNodes, int nodeValue)
    {
        Node currentNode;
        if (allNodes.TryGetValue(nodeValue, out Node node))
            currentNode = node;
        else
        {
            currentNode = new Node(nodeValue);
            allNodes.Add(nodeValue, currentNode);
        }

        return currentNode;
    }
}