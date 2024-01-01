using PathTrie;

namespace PathTrie.Tests;

[TestClass]
public class StringBlockTest
{
    [TestMethod]
    public void Add()
    {
        var block = new StringBlock();
        var id = block.Add("Hello, world!"u8);
        Assert.AreEqual(0, id);

        id = block.Add("Hello, world!"u8);
        Assert.AreEqual(0, id);

        Assert.IsTrue("Hello, world!"u8.SequenceEqual(block.Get(id)));
    }
}