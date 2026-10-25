using Atv2Grafos.Algorithms;
using Atv2Grafos.ConsoleApp;
using Atv2Grafos.Models;

var tests = new (string Name, Action Run)[]
{
    ("Grafo principal tem 8 vértices e 14 arestas", TestGraphCounts),
    ("Conexões aparecem nos dois sentidos", TestBidirectionalEdges),
    ("Grafo principal é conexo", TestConnectedGraph),
    ("Dijkstra calcula distâncias de V0", TestDijkstraFromV0),
    ("Dijkstra funciona a partir de outra origem", TestDijkstraFromV3),
    ("Dijkstra reconstrói caminhos completos", TestPathReconstruction),
    ("Dijkstra identifica destinos inalcançáveis", TestUnreachableVertex),
    ("Entradas vazias e inválidas são rejeitadas", TestInputValidation),
    ("Grafo rejeita peso negativo", TestNegativeWeight),
    ("Kruskal produz custo mínimo de 2500 m", TestKruskalTotal),
    ("Kruskal seleciona V-1 arestas sem ciclos", TestKruskalTreeShape),
    ("Arestas duplicadas não são contadas duas vezes", TestDuplicateEdge),
    ("Kruskal trata grafo desconexo", TestDisconnectedKruskal),
    ("Casos de vértice inválido são rejeitados", TestInvalidVertex)
};

int failures = 0;
foreach (var test in tests)
{
    try
    {
        test.Run();
        Console.WriteLine($"PASSOU: {test.Name}");
    }
    catch (Exception ex)
    {
        failures++;
        Console.WriteLine($"FALHOU: {test.Name} — {ex.Message}");
    }
}

Console.WriteLine($"\nResultado: {tests.Length - failures}/{tests.Length} testes passaram.");
return failures == 0 ? 0 : 1;

static void TestGraphCounts()
{
    var graph = ParqueFactory.Criar();
    Equal(8, graph.VertexCount);
    Equal(14, graph.EdgeCount);
    Equal("Portaria", graph.GetVertex(0).Name);
    Equal("Jardim Botânico", graph.GetVertex(7).Name);
}

static void TestBidirectionalEdges()
{
    var graph = ParqueFactory.Criar();
    var forward = graph.GetNeighbors(0).Single(edge => edge.To == 2);
    var reverse = graph.GetNeighbors(2).Single(edge => edge.To == 0);
    Equal(400, forward.Weight);
    Equal(forward.Weight, reverse.Weight);
    Equal(14, graph.Edges.Count);
}

static void TestConnectedGraph() => True(ParqueFactory.Criar().IsConnected());

static void TestDijkstraFromV0()
{
    long[] expected = [0, 600, 400, 1100, 1400, 1600, 2000, 2300];
    var result = Dijkstra.Executar(ParqueFactory.Criar(), 0);
    for (int i = 0; i < expected.Length; i++) Equal(expected[i], result.GetDistanceTo(i));
}

static void TestDijkstraFromV3()
{
    long[] expected = [1100, 500, 700, 0, 300, 500, 900, 1400];
    var result = Dijkstra.Executar(ParqueFactory.Criar(), 3);
    for (int i = 0; i < expected.Length; i++) Equal(expected[i], result.GetDistanceTo(i));
}

static void TestPathReconstruction()
{
    var graph = ParqueFactory.Criar();
    var result = Dijkstra.Executar(graph, 0);
    SequenceEqual(new[] { 0, 2, 1, 3 }, result.GetPathTo(3)!);
    SequenceEqual(new[] { 0 }, result.GetPathTo(0)!);
    Equal(1100L, PathWeight(graph, result.GetPathTo(3)!));
}

static void TestUnreachableVertex()
{
    var graph = new WeightedGraph();
    graph.AddVertex("A"); graph.AddVertex("B");
    var result = Dijkstra.Executar(graph, 0);
    Equal(null, result.GetDistanceTo(1));
    Equal(null, result.GetPathTo(1));
}

static void TestInputValidation()
{
    True(!InputParser.TryParseMenuOption(null, out _));
    True(!InputParser.TryParseMenuOption("", out _));
    True(!InputParser.TryParseMenuOption("abc", out _));
    True(InputParser.TryParseMenuOption("4", out int option) && option == 4);
    True(!InputParser.TryParseVertex(null, 8, out _));
    True(!InputParser.TryParseVertex(" ", 8, out _));
    True(!InputParser.TryParseVertex("V8", 8, out _));
    True(!InputParser.TryParseVertex("x", 8, out _));
    True(InputParser.TryParseVertex("v3", 8, out int vertex) && vertex == 3);
}

static void TestNegativeWeight()
{
    var graph = new WeightedGraph();
    graph.AddVertex("A"); graph.AddVertex("B");
    Throws<ArgumentOutOfRangeException>(() => graph.AddEdge(0, 1, -1));
}

static void TestKruskalTotal()
{
    var result = Kruskal.Executar(ParqueFactory.Criar());
    Equal(2500L, result.TotalWeight);
}

static void TestKruskalTreeShape()
{
    var graph = ParqueFactory.Criar();
    var result = Kruskal.Executar(graph);
    Equal(graph.VertexCount - 1, result.Edges.Count);
    var parent = Enumerable.Range(0, graph.VertexCount).ToArray();
    int Find(int item) => parent[item] == item ? item : parent[item] = Find(parent[item]);
    foreach (var edge in result.Edges)
    {
        int first = Find(edge.From), second = Find(edge.To);
        True(first != second, "A árvore contém ciclo.");
        parent[first] = second;
    }
    Equal(1, Enumerable.Range(0, graph.VertexCount).Select(Find).Distinct().Count());
}

static void TestDuplicateEdge()
{
    var graph = new WeightedGraph();
    graph.AddVertex("A"); graph.AddVertex("B");
    graph.AddEdge(0, 1, 1);
    Throws<InvalidOperationException>(() => graph.AddEdge(1, 0, 1));
    Equal(1, graph.EdgeCount);
}

static void TestDisconnectedKruskal()
{
    var graph = new WeightedGraph();
    graph.AddVertex("A"); graph.AddVertex("B");
    Throws<InvalidOperationException>(() => Kruskal.Executar(graph));
}

static void TestInvalidVertex()
{
    var graph = ParqueFactory.Criar();
    Throws<ArgumentOutOfRangeException>(() => graph.GetNeighbors(8));
    Throws<ArgumentOutOfRangeException>(() => Dijkstra.Executar(graph, -1));
}

static long PathWeight(WeightedGraph graph, IReadOnlyList<int> path)
{
    long total = 0;
    for (int i = 1; i < path.Count; i++)
        total += graph.GetNeighbors(path[i - 1]).Single(edge => edge.To == path[i]).Weight;
    return total;
}

static void True(bool value, string? message = null)
{
    if (!value) throw new Exception(message ?? "Esperado verdadeiro.");
}

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new Exception($"Esperado '{expected}', obtido '{actual}'.");
}

static void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
{
    if (!expected.SequenceEqual(actual))
        throw new Exception($"Sequência esperada [{string.Join(",", expected)}], obtida [{string.Join(",", actual)}].");
}

static void Throws<T>(Action action) where T : Exception
{
    try { action(); }
    catch (T) { return; }
    throw new Exception($"Esperada exceção {typeof(T).Name}.");
}
