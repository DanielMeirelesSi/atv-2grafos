using Atv2Grafos.Models;

namespace Atv2Grafos.Algorithms;

public sealed record MinimumSpanningTreeResult(IReadOnlyList<Edge> Edges, long TotalWeight);
