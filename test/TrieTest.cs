namespace PathTrie.Tests;

[TestClass]
public class TrieTest
{
    [TestMethod]
    public void Add()
    {
        var trie = new Trie();
        var id = trie.Add(@"C:\foo\bar"u8);
        Assert.AreEqual(3, id);

        id = trie.Add(@"C:\foo\bar"u8);
        Assert.AreEqual(3, id);

    }

    [TestMethod]
    public void StartsWith()
    {
        var trie = new Trie();
        trie.Add(@"C:\foo\bar"u8);
        trie.Add(@"C:\foo\baz"u8);
        trie.Add(@"C:\foo\qux"u8);
        trie.Add(@"C:\foo\quux"u8);
        trie.Add(@"C:\foo\quuz"u8);
        trie.Add(@"C:\foo\corge"u8);
        trie.Add(@"C:\foo\grault"u8);
        trie.Add(@"C:\foo\garply"u8);
        trie.Add(@"C:\foo\waldo"u8);
        trie.Add(@"C:\foo\fred"u8);
        trie.Add(@"C:\foo\plugh"u8);
        trie.Add(@"C:\foo\xyzzy"u8);
        trie.Add(@"C:\foo\thud"u8);

        var ids = trie.StartsWith(@"C:\foo\ba"u8);
        Assert.AreEqual(2, ids.Count);

        ids = trie.StartsWith(@"C:\foo\qu"u8);
        Assert.AreEqual(3, ids.Count);  
       
        ids = trie.StartsWith(@"C:\foo\grault"u8);
        Assert.AreEqual(1, ids.Count);

        ids = trie.StartsWith(@"C:\foo"u8);
        Assert.AreEqual(14, ids.Count);

        ids = trie.StartsWith(@"C:\foo"u8, Trie.SearchResultsType.Leaf);
        Assert.AreEqual(13, ids.Count);

        ids = trie.StartsWith(@"C:\foo"u8, Trie.SearchResultsType.Node);
        Assert.AreEqual(1, ids.Count);
    }
}
