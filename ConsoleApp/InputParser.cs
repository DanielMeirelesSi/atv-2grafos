namespace Atv2Grafos.ConsoleApp;

public static class InputParser
{
    public static bool TryParseMenuOption(string? input, out int option) =>
        int.TryParse(input, out option);

    public static bool TryParseVertex(string? input, int vertexCount, out int vertex)
    {
        vertex = -1;
        if (string.IsNullOrWhiteSpace(input)) return false;
        string value = input.Trim();
        if (value.StartsWith('V') || value.StartsWith('v')) value = value[1..];
        return int.TryParse(value, out vertex) && vertex >= 0 && vertex < vertexCount;
    }
}
