using System.IO.MemoryMappedFiles;
using System.Text;

namespace PathTrie.Tests;

[TestClass]
public class OddTest
{
    [TestMethod]
    [Ignore("This is just a test method to generate test content; run manually only")]
    public void DumpPaths()
    {
        // Enumerate every path on the drive
        var paths = Directory.EnumerateFiles(@"C:\", "*", SearchOption.AllDirectories);

        using var file = File.OpenWrite(@"C:\scratch\paths.txt");
        using var writer = new StreamWriter(file);

        using var e = paths.GetEnumerator();
    again:
        try
        {
            while (e.MoveNext())
            {
                writer.WriteLine(e.Current);
            }
        }
        catch
        {
            goto again;
        }


    }

    [TestMethod]
    public void LoadToTrie()
    {
        //var bytes = File.ReadAllBytes(@"C:\scratch\paths.txt");

        //using var view = new MemoryStream(bytes);
        using var view = File.OpenRead(@"C:\scratch\paths.txt");

        //using var mmf = MemoryMappedFile.CreateFromFile(@"C:\scratch\paths.txt");

        //using var view = mmf.CreateViewStream();

        var trie = new Trie();

        var buffer = new byte[1024 * 1024];

        int read = view.Read(buffer.AsSpan());
        while (read != 0)
        {
            var span = buffer.AsSpan(0, read);

            while (span.Length > 0)
            {
                var cr = span.IndexOf((byte)'\r');

                if (cr == -1)
                {
                    break;
                }

                var line = span.Slice(0, cr);

                trie.Add(line);

                span = span.Slice(cr + 2);
            }

            // shift the remaining to the beginning of the buffer
            span.CopyTo(buffer);

            // Read more after what we copied to the front
            read = view.Read(buffer.AsSpan().Slice(span.Length));

            if (read == 0)
            {
                break;
            }

            // Add back on the bytes we put on the front for the next round of the loop
            read += span.Length;
        }

        GC.Collect();

        //Thread.Sleep(2000);

        Console.WriteLine("WOO!");
    }
}
