### Summary
Over holiday 2023, I decided to try to play around to find an efficient way to store paths in a prefix tree from .NET Core.

The point of this is to make a little C# one that will take tons of file system paths and keep them compressed yet searchable in memory. Ideally one should be able to reference any given leaf and work back up to get the whole path when necessary.
However, most consumers I've seen who want to look at paths anyway don't want the actual full string in the first place. They want to do StartsWith, EndsWith, or Contains on the paths to find ones that fall under a directory, have a specific extension, or have a partial filename match.
All of these should be possible without fully resynthesizing the trie nodes to a string first. Though I haven't implemented all of them up front, they should be relatively easy.

The other thing I want to encompass here is absolutely minimizing allocations. The other examples I've seen of this often don't care about how many nodes or substrings or whatnot that they allocate. As such, I created several block pools that make big honking chunks of bytes or whatnot and dole them out instead of new new newing everything up and adding all that overhead to heap management and the garbage collector.

I was also attempting to use structures where I could to avoid allocations completely and in the main Node structure, I intentionally used 4 ints so it would be 128-bits big (or rather a nice multiple of 32/64-bit word size and not waste bits there).

### Prior Research
I didn't really find an off-the-shelf implementation that looked to do what I wanted. Either my brain turned to mush trying to read the implementation, it was in C++ instead of C# and I really want to avoid compiling for a whole matrix of things (Linux/Windows, x86/amd64, blah blah blah) by leveraging .NET Core, it was heap allocating/freeing a lot, or something of that ilk. 

- https://github.com/Tessil/hat-trie
- https://github.com/dcjones/hat-trie
- https://github.com/kephir4eg/trie
- https://github.com/gmamaladze/trienet
- https://github.com/manly/AdaptiveRadixTree

### Performance Testing
- I dumped my entire filesystem paths to a file using the one little ignored unit test that's in here. It was 59,001KB with 575567 lines (paths).
- I loaded it in with the other unit test into this little trie. When I was done optimizing, it seemed to take the test a little over 1 second to read all the paths from the file and completely build the trie on a Framework Laptop 13 i7-1280p variety. If I recall correctly, it consumed a little over 20MB of RAM when I was staring at it under the Visual Studio .NET Allocations memory profiler and JetBrains' dotMemory.

### Conclusion
In lieu of losing this somewhere in my pile of computers, I threw it on GitHub. Chances that it is effectively "abandoned" like many of the other tries out there is high, but if I do end up using it for something, I might put the bonus tweaks back here.
However, maybe it'll help the next person scouring the internet for something like this. Or future me.
