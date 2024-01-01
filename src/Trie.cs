namespace PathTrie;

public class Trie
{
    private readonly NodeBlock _nodes = new();
    private readonly ChildrenBlock _children = new();
    private readonly StringBlock _strings = new();

    private const byte SEPARATOR = (byte)'\\';

    private int RootNode { get; init; }

    public Trie()
    {
        _nodes.Add(new Node());
        RootNode = 0;
    }

    public int Add(ReadOnlySpan<byte> chars)
    {
        var node = RootNode;
        do
        {
            var seg = NextSegment(ref chars);

            var child = GetChild(node, seg);

            if (child == -1)
            {
                var newNode = new Node { ParentId = node, Children = new ChildrenBlock.ChildInfo(), StringId = _strings.Add(seg) };
                child = _nodes.Add(newNode);
                AddChild(node, child);
            }

            node = child;

        } while (chars.Length > 0);

        return node;
    }

    public enum SearchResultsType
    {
        Both,
        Leaf,
        Node,
    }

    public List<int> StartsWith(ReadOnlySpan<byte> chars, SearchResultsType resultsType = SearchResultsType.Both)
    {
        // From the root node
        var node = RootNode;

        // Path segment by segment, try to do full matches with the children
        do
        {
            var prevChars = chars;
            var seg = NextSegment(ref chars);

            var child = GetChild(node, seg);

            // If we miss finding a child, we're as deep as we can get in the tree, break out.
            if (child == -1)
            {
                chars = prevChars;
                break;
            }

            node = child;
        } while (chars.Length > 0);

        var queue = new Queue<int>();

        // There's still some left, attempt substring starts with on children of this node.
        if (chars.Length > 0)
        {
            foreach (var child in _children.Get(_nodes.Get(node).Children))
            {
                var childNode = _nodes.Get(child);

                if (childNode.StringId != -1)
                {
                    var childString = _strings.Get(childNode.StringId);

                    if (childString.StartsWith(chars))
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
        // Nothing's left, just add from this node
        else
        {
            queue.Enqueue(node);
        }

        var ret = new List<int>();

        // In the queue, start adding all the children of the nodes that start with the string.
        while (queue.Count > 0)
        {
            var child = queue.Dequeue();
            var childNode = _nodes.Get(child);

            if (childNode.StringId != -1)
            {
                if (childNode.Children.Length < 1)
                {
                    if (resultsType == SearchResultsType.Leaf || resultsType == SearchResultsType.Both)
                    {
                        ret.Add(child);
                    }
                }
                else
                {
                    if (resultsType == SearchResultsType.Node || resultsType == SearchResultsType.Both)
                    {
                        ret.Add(child);
                    }
                }
            }

            var childChildren = _children.Get(childNode.Children);
            foreach (var grandChild in childChildren)
            {
                queue.Enqueue(grandChild);
            }
        }

        return ret;
    }

    private ReadOnlySpan<byte> NextSegment(ref ReadOnlySpan<byte> chars)
    {
        var idx = chars.IndexOf(SEPARATOR);
        if (idx == -1)
        {
            var ret = chars;
            chars = [];
            return ret;
        }
        else
        {
            var ret = chars.Slice(0, idx);
            chars = chars.Slice(idx + 1);
            return ret;
        }
    }

    private int FindMatchingChild(ReadOnlySpan<int> children, ReadOnlySpan<byte> chars)
    {
        var strId = _strings.Find(chars);

        if (strId == -1)
        {
            return -1;
        }

        foreach (var childId in children)
        {
            var child = _nodes.Get(childId);

            if (child.StringId == strId)
            {
                return childId;
            }
        }

        return -1;
    }

    private int GetChild(int nodeId, ReadOnlySpan<byte> segment)
    {
        var node = _nodes.Get(nodeId);
        var children = _children.Get(node.Children);
        return FindMatchingChild(children, segment);
    }

    private void AddChild(int parentId, int childId)
    {
        var parent = _nodes.Get(parentId);
        parent.Children = _children.Insert(parent.Children, childId);
        _nodes.Set(parentId, parent);
    }
}
