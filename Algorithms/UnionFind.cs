namespace Atv2Grafos.Algorithms;

internal sealed class UnionFind
{
    private readonly int[] _parent;
    private readonly byte[] _rank;

    public UnionFind(int count)
    {
        _parent = Enumerable.Range(0, count).ToArray();
        _rank = new byte[count];
    }

    public int Find(int item)
    {
        if (_parent[item] != item) _parent[item] = Find(_parent[item]);
        return _parent[item];
    }

    public bool Union(int first, int second)
    {
        int rootFirst = Find(first);
        int rootSecond = Find(second);
        if (rootFirst == rootSecond) return false;
        if (_rank[rootFirst] < _rank[rootSecond]) _parent[rootFirst] = rootSecond;
        else if (_rank[rootFirst] > _rank[rootSecond]) _parent[rootSecond] = rootFirst;
        else
        {
            _parent[rootSecond] = rootFirst;
            _rank[rootFirst]++;
        }
        return true;
    }
}
