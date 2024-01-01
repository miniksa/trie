namespace PathTrie;

internal class NodeBlock
{
    private List<Node> _storage = [];

    public int Add(Node value)
    {
        _storage.Add(value);
        return _storage.Count - 1;
    }

    public Node Get(int id)
    {
        return _storage[id];
    }

    public void Set(int id, Node node)
    {
        _storage[id] = node;
    }
}
