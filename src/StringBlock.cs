using System.IO.Hashing;
using System.Runtime.InteropServices;

namespace PathTrie;

internal class StringBlock
{
    private readonly List<byte> _storage = [];
    private readonly Dictionary<int, int> _lengths = [];
    private readonly Dictionary<ulong, int> _hashes = [];

    public int Find(ReadOnlySpan<byte> value)
    {
        Span<byte> hashBuffer = stackalloc byte[8];

        var written = XxHash3.Hash(value, hashBuffer);

        var hash = BitConverter.ToUInt64(hashBuffer.Slice(0, written));
        if (_hashes.TryGetValue(hash, out var id))
        {
            return id;
        }

        return -1;
    }

    public int Add(ReadOnlySpan<byte> value)
    {
        Span<byte> hashBuffer = stackalloc byte[8];

        var written = XxHash3.Hash(value, hashBuffer);

        var hash = BitConverter.ToUInt64(hashBuffer.Slice(0, written));
        if (_hashes.TryGetValue(hash, out var id))
        {
            if (value.SequenceEqual(CollectionsMarshal.AsSpan(_storage).Slice(id, _lengths[id])))
            {
                return id;
            }
        }

        id = _storage.Count;
        _storage.AddRange(value);
        _lengths.Add(id, value.Length);
        _hashes.Add(hash, id);
        return id;
    }

    public ReadOnlySpan<byte> Get(int id)
    {
        return CollectionsMarshal.AsSpan(_storage).Slice(id, _lengths[id]);
    }
}
