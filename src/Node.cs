namespace PathTrie;

internal struct Node
{
    public int StringId;
    public int ParentId;
    public ChildrenBlock.ChildInfo Children;

    public Node()
    {
        StringId = -1;
        ParentId = -1;
        Children = new ChildrenBlock.ChildInfo();
    }
}
