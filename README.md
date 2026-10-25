# Atividade Prática 02 – Algoritmos em Grafos

Aplicação de console em C# para analisar deslocamentos em um parque turístico por meio de caminhos mínimos e árvore geradora mínima.

## Objetivo

Representar os pontos de interesse como vértices e as trilhas como arestas ponderadas por distância. O programa imprime o grafo, calcula os menores caminhos a partir de uma origem escolhida e encontra o conjunto de trilhas de menor peso que conecta todos os pontos.

## Tecnologias

- C# e .NET 8.
- Lista de adjacência para representar as conexões bidirecionais.
- `PriorityQueue` do .NET no Dijkstra.
- Union-Find implementado no projeto para o Kruskal.
- Suíte de testes automatizada em console, sem dependências externas.

## Modelagem

O parque possui oito vértices (`V0` a `V7`) e 14 trilhas. Cada trilha é armazenada uma vez em `WeightedGraph.Edges`, usada pelo Kruskal, e aparece nos dois sentidos na lista de adjacência, usada pelo Dijkstra. Assim, a exibição das conexões é bidirecional sem duplicar arestas no cálculo da árvore geradora.

Os pesos são distâncias em metros e não podem ser negativos. A criação do grafo do parque fica centralizada em `ParqueFactory`. A classe também verifica duplicidade de ligação e endpoints inválidos ao adicionar arestas.

## Estrutura de arquivos

```text
.
├── Algorithms/
│   ├── Dijkstra.cs
│   ├── Kruskal.cs
│   ├── MinimumSpanningTreeResult.cs
│   ├── ShortestPathResult.cs
│   └── UnionFind.cs
├── ConsoleApp/
│   ├── InputParser.cs
│   └── ParqueConsoleApp.cs
├── Models/
│   ├── Edge.cs
│   ├── ParqueFactory.cs
│   ├── Vertex.cs
│   └── WeightedGraph.cs
├── Tests/
│   ├── Atv2Grafos.Tests.csproj
│   └── Program.cs
├── Program.cs
├── atv2 grafos.csproj
├── .gitignore
└── README.md
```

`Models` guarda o grafo e os dados do parque; `Algorithms` contém os algoritmos; `ConsoleApp` valida entradas e apresenta os resultados. `Tests` é um executável de testes com verificações e código de saída diferente de zero quando algum caso falha. Essa opção evita dependências externas e ainda permite executar a suíte diretamente com o SDK .NET.

## Algoritmos

### Dijkstra

Inicializa a distância da origem como zero e as demais como desconhecidas. Uma fila de prioridade seleciona o próximo vértice com menor distância estimada. Cada aresta é relaxada e o predecessor atualizado quando um caminho mais curto é encontrado. A reconstrução percorre os predecessores até a origem. Destinos sem distância calculada são apresentados como inalcançáveis. O grafo rejeita pesos negativos, condição necessária para o algoritmo.

### Kruskal

Ordena as arestas por peso e, em caso de empate, pelos identificadores dos endpoints. Para cada aresta, Union-Find verifica se seus extremos já pertencem ao mesmo componente. A aresta é adicionada apenas quando une componentes distintos; o processo termina após selecionar `V - 1` arestas. Se o grafo for desconexo, o método informa que não existe árvore geradora.

## Executar

Na raiz do projeto:

```powershell
dotnet run --project "atv2 grafos.csproj"
```

O programa exibe o grafo ao iniciar e então oferece as opções para visualizar novamente, rodar Dijkstra, rodar Kruskal, executar a demonstração completa ou sair. Na opção de Dijkstra, informe `V0` a `V7` (também são aceitos os números sem o prefixo `V`).

## Testar

```powershell
dotnet run --project Tests/Atv2Grafos.Tests.csproj
```

A suíte executa 14 verificações de modelagem, bidirecionalidade, conectividade, distâncias e caminhos de duas origens, entradas inválidas, pesos negativos, duplicidade, componentes desconexos e estrutura/custo da árvore mínima.

## Resultados esperados

Para Dijkstra partindo de `V0`, as distâncias são `0, 600, 400, 1100, 1400, 1600, 2000 e 2300 m`, na ordem `V0` a `V7`. Para Kruskal, o resultado contém sete arestas, sem ciclos, e totaliza **2500 m**.

Trecho da demonstração executada com origem `V0`:

```text
DIJKSTRA — origem: V0 (Portaria)
V0  Portaria                               0 m   V0
V1  Mirante                              600 m   V0 -> V2 -> V1
V2  Centro de Visitantes                 400 m   V0 -> V2
V3  Cachoeira                           1100 m   V0 -> V2 -> V1 -> V3
...
V7  Jardim Botânico                     2300 m   V0 -> V2 -> V1 -> V7

KRUSKAL — ÁRVORE GERADORA MÍNIMA
V1 (Mirante) — V2 (Centro de Visitantes): 200 m
V4 (Área de Descanso) — V5 (Restaurante): 200 m
V3 (Cachoeira) — V4 (Área de Descanso): 300 m
V0 (Portaria) — V2 (Centro de Visitantes): 400 m
V5 (Restaurante) — V6 (Lago): 400 m
V1 (Mirante) — V3 (Cachoeira): 500 m
V6 (Lago) — V7 (Jardim Botânico): 500 m
Total: 2500 m | Arestas: 7 de 7
```

