namespace Atv2Grafos.Models;

public static class ParqueFactory
{
    public static WeightedGraph Criar()
    {
        var graph = new WeightedGraph();
        string[] nomes = ["Portaria", "Mirante", "Centro de Visitantes", "Cachoeira",
            "Área de Descanso", "Restaurante", "Lago", "Jardim Botânico"];
        foreach (string nome in nomes) graph.AddVertex(nome);
        (int From, int To, int Weight)[] trilhas =
        [
            (0, 1, 700), (0, 2, 400), (1, 2, 200), (1, 3, 500),
            (2, 3, 800), (2, 4, 1200), (3, 4, 300), (3, 5, 600),
            (4, 5, 200), (4, 6, 700), (5, 6, 400), (5, 7, 900),
            (6, 7, 500), (1, 7, 1700)
        ];
        foreach (var trilha in trilhas) graph.AddEdge(trilha.From, trilha.To, trilha.Weight);
        return graph;
    }
}
