namespace PathTrie.Tests;

[TestClass]
public class ChildrenBlockTest
{
    [TestMethod]
    public void GetRealSizeTest()
    {
        Assert.AreEqual(0, ChildrenBlock.GetRealSize(-1));
        Assert.AreEqual(0, ChildrenBlock.GetRealSize(0));
        Assert.AreEqual(ChildrenBlock.MIN_SIZE, ChildrenBlock.GetRealSize(1));
        Assert.AreEqual(ChildrenBlock.MIN_SIZE, ChildrenBlock.GetRealSize(2));
        Assert.AreEqual(ChildrenBlock.MIN_SIZE, ChildrenBlock.GetRealSize(3));
        Assert.AreEqual(ChildrenBlock.MIN_SIZE, ChildrenBlock.GetRealSize(4));
        Assert.AreEqual(8, ChildrenBlock.GetRealSize(5));
        Assert.AreEqual(8, ChildrenBlock.GetRealSize(6));
        Assert.AreEqual(8, ChildrenBlock.GetRealSize(7));
        Assert.AreEqual(8, ChildrenBlock.GetRealSize(8));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(9));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(10));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(11));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(12));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(13));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(14));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(15));
        Assert.AreEqual(16, ChildrenBlock.GetRealSize(16));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(17));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(18));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(19));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(20));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(21));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(22));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(23));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(24));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(25));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(26));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(27));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(28));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(29));
        Assert.AreEqual(32, ChildrenBlock.GetRealSize(30));

    }
}
