using Thorarin.AdventOfCode.Framework;

namespace Thorarin.AdventOfCode.Year2022
{
    public abstract class Day07Base : Puzzle
    {
        protected List<Directory> TraverseDirectories(IEnumerable<string> lines)
        {
            List<Directory> foundDirectories = new();
            Stack<Directory> dirStack = new();

            Directory ParentDirectory() => dirStack.Peek();

            foreach (string line in lines)
            {
                if (line.StartsWith("$ cd"))
                {
                    // Change directory
                    string dirName = line.Substring(5);
                    if (dirName == "/")
                    {
                        dirStack.Clear();
                        dirStack.Push(new Directory(dirName));
                    }
                    else if (dirName == "..")
                    {
                        var dir = dirStack.Pop();
                        ParentDirectory().Size += dir.Size;

                        foundDirectories.Add(dir);
                    }
                    else
                    {
                        dirStack.Push(new Directory(dirName));
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    // Nothing to do
                }
                else if (line.StartsWith("dir "))
                {
                    // Nothing to do
                }
                else
                {
                    // File size and name
                    var fileSizeAndName = line.Split(' ');
                    ParentDirectory().Size += int.Parse(fileSizeAndName[0]);
                }
            }

            while (dirStack.Count > 0)
            {
                var dir = dirStack.Pop();

                if (dirStack.Count > 0)
                    ParentDirectory().Size += dir.Size;

                foundDirectories.Add(dir);
            }


            return foundDirectories;
        }

        protected record Directory(string Name)
        {
            public int Size { get; set; }
        }
    }
}
