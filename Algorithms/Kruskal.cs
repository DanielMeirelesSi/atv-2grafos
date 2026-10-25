using Atv2Grafos.Models;

namespace Atv2Grafos.Algorithms;

public static class Kruskal
{
    public static MinimumSpanningTreeResult Executar(WeightedGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);
        var selected = new List<Edge>();
        long total = 0;
        var unionFind = new UnionFind(graph.VertexCount);
        foreach (Edge edge in graph.Edges.OrderBy(edge => edge.Weight)
                     .ThenBy(edge => Math.Min(edge.From, edge.To))
                     .ThenBy(edge => Math.Max(edge.From, edge.To)))
        {
            if (!unionFind.Union(edge.From, edge.To)) continue;
            selected.Add(edge);
            total += edge.Weight;
            if (selected.Count == graph.VertexCount - 1) break;
        }
        if (graph.VertexCount > 0 && selected.Count != graph.VertexCount - 1)
            throw new InvalidOperationException("O grafo não é conexo; não existe árvore geradora.");
        return new MinimumSpanningTreeResult(selected, total);
    }
}
