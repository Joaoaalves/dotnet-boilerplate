using DotNetEnv;

namespace Project.Tests.Integration.Setup
{
    /// <summary>
    /// Ensures .env variables from the solution root are loaded into the test environment.
    /// </summary>
    public static class EnvLoader
    {
        private static bool _loaded = false;

        public static void EnsureLoaded()
        {
            if (_loaded) return;

            // Go up to solution root
            var root = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../../../"));

            var envPath = Path.Combine(root, ".env");
            if (File.Exists(envPath))
            {
                Env.Load(envPath);
            }

            _loaded = true;
        }
    }
}
