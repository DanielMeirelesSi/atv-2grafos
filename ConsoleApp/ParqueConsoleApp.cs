using Atv2Grafos.Algorithms;
using Atv2Grafos.Models;

namespace Atv2Grafos.ConsoleApp;

public sealed class ParqueConsoleApp(WeightedGraph graph)
{
    public void Executar()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║       ATIVIDADE PRÁTICA 02 — ALGORITMOS EM GRAFOS    ║");
        Console.WriteLine("║               Planejamento de trilhas               ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        ExibirGrafo();
        while (true)
        {
            Console.WriteLine("\nMENU PRINCIPAL");
            Console.WriteLine("1. Visualizar o grafo");
            Console.WriteLine("2. Executar Dijkstra");
            Console.WriteLine("3. Executar Kruskal");
            Console.WriteLine("4. Executar demonstração completa");
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                Console.WriteLine("\nEntrada encerrada. Até logo!");
                return;
            }
            if (!InputParser.TryParseMenuOption(input, out int option) || option is < 0 or > 4)
            {
                Console.WriteLine("Entrada inválida. Digite uma opção de 0 a 4.");
                continue;
            }
            switch (option)
            {
                case 0: Console.WriteLine("Até logo!"); return;
                case 1: ExibirGrafo(); break;
                case 2: ExecutarDijkstra(); break;
                case 3: ExibirKruskal(); break;
                case 4:
                    ExecutarDijkstra();
                    ExibirKruskal();
                    break;
            }
        }
    }

    private void ExibirGrafo()
    {
        Console.WriteLine("\nGRAFO DO PARQUE — LISTA DE ADJACÊNCIA");
        Console.WriteLine($"Pontos: {graph.VertexCount} | Trilhas bidirecionais: {graph.EdgeCount} | Conexo: {(graph.IsConnected() ? "sim" : "não")}");
        foreach (Vertex vertex in graph.Vertices)
        {
            string neighbors = string.Join("  |  ", graph.GetNeighbors(vertex.Id)
                .Select(edge => $"V{edge.To} ({graph.GetVertex(edge.To).Name}) — {edge.Weight} m"));
            Console.WriteLine($"{vertex.Label} — {vertex.Name}: {neighbors}");
        }
    }

    private void ExecutarDijkstra()
    {
        int source;
        while (true)
        {
            Console.Write("Informe o vértice de origem (V0 a V7): ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                Console.WriteLine("\nEntrada encerrada.");
                return;
            }
            if (InputParser.TryParseVertex(input, graph.VertexCount, out source)) break;
            Console.WriteLine("Vértice inválido. Informe um identificador entre V0 e V7.");
        }
        Vertex origin = graph.GetVertex(source);
        ShortestPathResult result = Dijkstra.Executar(graph, source);
        Console.WriteLine($"\nDIJKSTRA — origem: {origin.Label} ({origin.Name})");
        Console.WriteLine(new string('─', 82));
        foreach (Vertex destination in graph.Vertices)
        {
            long? distance = result.GetDistanceTo(destination.Id);
            IReadOnlyList<int>? path = result.GetPathTo(destination.Id);
            string pathText = path is null ? "inalcançável" : string.Join(" -> ", path.Select(id => $"V{id}"));
            string distanceText = distance is null ? "inalcançável" : $"{distance} m";
            Console.WriteLine($"{destination.Label,-3} {destination.Name,-25} {distanceText,16}   {pathText}");
        }
        Console.WriteLine(new string('─', 82));
    }

    private void ExibirKruskal()
    {
        MinimumSpanningTreeResult result = Kruskal.Executar(graph);
        Console.WriteLine("\nKRUSKAL — ÁRVORE GERADORA MÍNIMA");
        Console.WriteLine(new string('─', 72));
        foreach (Edge edge in result.Edges)
        {
            Vertex from = graph.GetVertex(edge.From);
            Vertex to = graph.GetVertex(edge.To);
            Console.WriteLine($"{from.Label} ({from.Name}) — {to.Label} ({to.Name}): {edge.Weight} m");
        }
        Console.WriteLine(new string('─', 72));
        Console.WriteLine($"Total: {result.TotalWeight} m | Arestas: {result.Edges.Count} de {graph.VertexCount - 1}");
    }
}
