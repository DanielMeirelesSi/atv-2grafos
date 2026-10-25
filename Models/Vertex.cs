namespace Atv2Grafos.Models;

public sealed record Vertex(int Id, string Name)
{
    public string Label => $"V{Id}";
}
