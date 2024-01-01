using System.Collections;

namespace PathTrie;

internal class ZeroGenerator : IEnumerable<int>
{
    public ZeroGenerator(int howMany)
    {
        HowMany = howMany;
        _enum = new ZeroEnumerator(howMany);
    }

    public int HowMany { get; set; }

    private ZeroEnumerator _enum;

    public IEnumerator<int> GetEnumerator()
    {
        _enum.HowMany = HowMany;
        return _enum;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        _enum.HowMany = HowMany;
        return _enum;
    }
}
