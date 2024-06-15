using System.Collections.Generic;

public class Pathfinder<T>
{
    readonly Dictionary<T, HashSet<T>> map;

    public Pathfinder(Dictionary<T, HashSet<T>> map)
    {
        this.map = map;
    }

    public string PathAsString(T from, T to)
    {
        var pathAsString = string.Empty;
        var path = FindPath(from, to);

        if (path != null)
        {
            path.ForEach(t => pathAsString += $"{t} ");
            return pathAsString;
        }
        else
        {
            return "path not possible";
        }
    }

    public List<T> FindPath(T from, T to)
    {
        if (!map.ContainsKey(from) || !map.ContainsKey(to))
        {
            return null;
        }

        var visited = new HashSet<T>();
        var parents = new Dictionary<T, T>();
        var toVisit = new Queue<T>();

        toVisit.Enqueue(from);

        while (toVisit.Count > 0)
        {
            var current = toVisit.Dequeue();
            visited.Add(current);

            if (current.Equals(to))
            {
                break;
            }

            foreach (var neighbor in map[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    toVisit.Enqueue(neighbor);
                    parents[neighbor] = current;
                }
            }
        }

        if (!visited.Contains(to))
        {
            return null;
        }

        var node = to;
        var path = new List<T>();

        while (!node.Equals(from))
        {
            path.Add(node);

            node = parents[node];
        }

        path.Add(from);
        path.Reverse();

        return path;
    }
}