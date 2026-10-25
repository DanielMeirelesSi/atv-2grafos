namespace Atv2Grafos.Models;

public sealed class WeightedGraph
{
    private readonly List<Vertex> _vertices = [];
    private readonly List<List<Edge>> _adjacency = [];
    private readonly List<Edge> _edges = [];

    public IReadOnlyList<Vertex> Vertices => _vertices;
    public IReadOnlyList<Edge> Edges => _edges;
    public int VertexCount => _vertices.Count;
    public int EdgeCount => _edges.Count;

    public int AddVertex(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do vértice não pode ser vazio.", nameof(name));
        int id = _vertices.Count;
        _vertices.Add(new Vertex(id, name.Trim()));
        _adjacency.Add([]);
        return id;
    }

    public void AddEdge(int from, int to, int weight)
    {
        ValidateVertex(from);
        ValidateVertex(to);
        if (from == to) throw new ArgumentException("Não são permitidas arestas de um vértice para ele mesmo.");
        if (weight < 0) throw new ArgumentOutOfRangeException(nameof(weight), "O peso deve ser não negativo.");
        if (_edges.Any(edge => SamePair(edge, from, to)))
            throw new InvalidOperationException("Essa conexão já existe.");
        _edges.Add(new Edge(from, to, weight));
        _adjacency[from].Add(new Edge(from, to, weight));
        _adjacency[to].Add(new Edge(to, from, weight));
    }

    public IReadOnlyList<Edge> GetNeighbors(int vertex)
    {
        ValidateVertex(vertex);
        return _adjacency[vertex];
    }

    public Vertex GetVertex(int id)
    {
        ValidateVertex(id);
        return _vertices[id];
    }

    public bool IsConnected()
    {
        if (VertexCount == 0) return false;
        var visited = new bool[VertexCount];
        var queue = new Queue<int>();
        visited[0] = true;
        queue.Enqueue(0);
        int count = 0;
        while (queue.TryDequeue(out int current))
        {
            count++;
            foreach (Edge edge in _adjacency[current])
            {
                if (visited[edge.To]) continue;
                visited[edge.To] = true;
                queue.Enqueue(edge.To);
            }
        }
        return count == VertexCount;
    }

    private void ValidateVertex(int id)
    {
        if (id < 0 || id >= VertexCount)
            throw new ArgumentOutOfRangeException(nameof(id), "O vértice informado não existe.");
    }

    private static bool SamePair(Edge edge, int from, int to) =>
        (edge.From == from && edge.To == to) || (edge.From == to && edge.To == from);
}
