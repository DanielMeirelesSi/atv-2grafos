namespace Atv2Grafos.Algorithms;

public sealed class ShortestPathResult(long?[] distances, int?[] predecessors, int source)
{
    public int Source { get; } = source;
    public int Count => distances.Length;

    public long? GetDistanceTo(int destination)
    {
        Validate(destination);
        return distances[destination];
    }

    public IReadOnlyList<int>? GetPathTo(int destination)
    {
        Validate(destination);
        if (distances[destination] is null) return null;
        var path = new List<int>();
        int? current = destination;
        while (current is not null)
        {
            path.Add(current.Value);
            if (current.Value == Source) break;
            current = predecessors[current.Value];
        }
        if (path[^1] != Source) return null;
        path.Reverse();
        return path;
    }

    private void Validate(int destination)
    {
        if (destination < 0 || destination >= Count)
            throw new ArgumentOutOfRangeException(nameof(destination));
    }
}
