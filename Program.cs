using Atv2Grafos.ConsoleApp;
using Atv2Grafos.Models;

var graph = ParqueFactory.Criar();
new ParqueConsoleApp(graph).Executar();
