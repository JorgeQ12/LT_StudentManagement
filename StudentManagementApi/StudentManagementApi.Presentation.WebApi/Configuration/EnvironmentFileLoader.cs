using DotNetEnv;

namespace StudentManagementApi.Presentation.WebApi.Configuration;

internal static class EnvironmentFileLoader
{
    private const string FileName = ".env";

    public static void Load()
    {
        var path = Find(Directory.GetCurrentDirectory()) ?? Find(AppContext.BaseDirectory);
        if (path is not null) Env.NoClobber().Load(path);
    }

    private static string? Find(string startPath)
    {
        for (var directory = new DirectoryInfo(startPath); directory is not null; directory = directory.Parent)
        {
            var path = Path.Combine(directory.FullName, FileName);
            if (File.Exists(path)) return path;
        }

        return null;
    }
}
