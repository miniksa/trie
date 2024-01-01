using System.Runtime.InteropServices;

namespace PathTrie;

internal class ChildrenBlock
{
    internal struct ChildInfo
    {
        public int Start;
        public int Length;

        public ChildInfo()
        {
            Start = -1;
            Length = -1;
        }
    }

    private readonly List<(int, int)> _reuse = [];
    private readonly List<int> _storage = [];

    internal const int MIN_SIZE = 4;
    private const int GROWTH_RATE = 2;

    private readonly ZeroGenerator _zeros = new(MIN_SIZE);

    public ReadOnlySpan<int> Get(ChildInfo info)
    {
        if (info.Start == -1)
        {
            return [];
        }

        return CollectionsMarshal.AsSpan(_storage).Slice(info.Start, info.Length);
    }

    internal static int GetRealSize(int size)
    {
        if (size == 0 || size == -1)
        {
            return 0;
        }

        if (size <= MIN_SIZE)
        {
            return MIN_SIZE;
        }

        --size;

        int highestBit = 0;
        while (size != 0)
        {
            size >>= 1;
            highestBit++;
        }

        return 1 << highestBit;
    }

    private bool TryFindReuse(int requestedSize, out int posFound)
    {
        for (int i = 0; i < _reuse.Count; i++)
        {
            var (size, position) = _reuse[i];

            if (size == requestedSize)
            {
                _reuse.RemoveAt(i);
                posFound = position;
                return true;
            }
        }
        posFound = -1;
        return false;
    }

    public ChildInfo GetNewChild(int sizeRequested)
    {
        var size = GetRealSize(sizeRequested);

        if (size == -1)
        {
            throw new InvalidOperationException("Invalid size requested");
        }

        if (TryFindReuse(size, out var id))
        {
            return new ChildInfo { Start = id, Length = size };
        }
        else
        {
            var start = _storage.Count;
            _zeros.HowMany = size;
            _storage.AddRange(_zeros);
            return new ChildInfo { Start = start, Length = size };
        }
    }

    public ChildInfo Insert(ChildInfo existing, int childIndex)
    {
        int capacity;
        if (existing.Start == -1)
        {
            existing = GetNewChild(MIN_SIZE);
            capacity = existing.Length;
            existing.Length = 0;
        }
        else
        {
            capacity = GetRealSize(existing.Length);
        }

        if (existing.Length == capacity)
        {
            var newCapacity = capacity * GROWTH_RATE;
            var newChild = GetNewChild(newCapacity);
            var oldSpan = CollectionsMarshal.AsSpan(_storage).Slice(existing.Start, existing.Length);
            var newSpan = CollectionsMarshal.AsSpan(_storage).Slice(newChild.Start, newChild.Length);

            oldSpan.CopyTo(newSpan);
            oldSpan.Clear();

            newChild.Length = existing.Length;

            _reuse.Add((existing.Length, existing.Start));

            existing = newChild;
        }

        _storage[existing.Start + existing.Length] = childIndex;
        existing.Length++;
        return existing;
    }
}
