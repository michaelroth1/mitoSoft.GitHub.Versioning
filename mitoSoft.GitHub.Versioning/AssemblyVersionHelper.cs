using System.Linq;
using System.Reflection;

namespace mitoSoft.GitHub.Versioning
{
    public class AssemblyVersionHelper
    {
        public Assembly Assembly { get; }

        public AssemblyVersionHelper() : this(System.Reflection.Assembly.GetExecutingAssembly())
        {
        }

        public AssemblyVersionHelper(Assembly assembly)
        {
            this.Assembly = assembly;
        }

        public System.Version GetVersion()
        {
            return this.GetVersion(3);
        }

        public System.Version GetVersion(int length)
        {
            var version = this.Assembly.GetName().Version;

            var result = CutVersion(version, length);

            return result;
        }

        private static System.Version CutVersion(System.Version version, int length)
        {
            try
            {
                var versionNumbers = version.ToString().Split('.').ToList();

                versionNumbers.RemoveRange(3, versionNumbers.Count - 3);

                return new System.Version(string.Join(".", versionNumbers));
            }
            catch
            {
                var versionNumbers = version.ToString().Split('.').ToList();

                return new System.Version(string.Join(".", versionNumbers));
            }
        }
    }
}