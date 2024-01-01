using System.Collections;

namespace PathTrie;

internal class ZeroEnumerator : IEnumerator<int>
{
    public ZeroEnumerator(int howMany)
    {
        HowMany = howMany;
    }

    public int HowMany { get; set; }

    public int Current => 0;

    object IEnumerator.Current => 0;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        return HowMany-- > 0;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}
