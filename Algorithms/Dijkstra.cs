using Atv2Grafos.Models;

namespace Atv2Grafos.Algorithms;

public static class Dijkstra
{
    public static ShortestPathResult Executar(WeightedGraph graph, int source)
    {
        ArgumentNullException.ThrowIfNull(graph);
        if (source < 0 || source >= graph.VertexCount)
            throw new ArgumentOutOfRangeException(nameof(source), "O vértice de origem não existe.");

        var distances = new long?[graph.VertexCount];
        var predecessors = new int?[graph.VertexCount];
        var queue = new PriorityQueue<int, long>();
        distances[source] = 0;
        queue.Enqueue(source, 0);
        while (queue.TryDequeue(out int current, out long queuedDistance))
        {
            if (distances[current] != queuedDistance) continue;
            foreach (Edge edge in graph.GetNeighbors(current))
            {
                long candidate = queuedDistance + edge.Weight;
                if (distances[edge.To] is not null && distances[edge.To] <= candidate) continue;
                distances[edge.To] = candidate;
                predecessors[edge.To] = current;
                queue.Enqueue(edge.To, candidate);
            }
        }
        return new ShortestPathResult(distances, predecessors, source);
    }
}
