using System.IO;

namespace Faker.ReplaceFileName;

public class Program
{
    static void Main(string[] args)
    {
        string path = args[0], current = args[1], target = args[2];
        var dirs = Directory.GetDirectories(path, $"*{current}*", SearchOption.AllDirectories);
        foreach (var dir in dirs)
        {
            string name = dir.Replace(current, target);
            Directory.Move(dir, name);
        }

        var files = Directory.GetFiles(path, $"*{current}*.cs", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string name = Path.GetFileName(file).Replace(current, target);
            File.Move(file, Path.GetDirectoryName(file) + "\\" + name + Path.GetExtension(file));
        }
    }
}