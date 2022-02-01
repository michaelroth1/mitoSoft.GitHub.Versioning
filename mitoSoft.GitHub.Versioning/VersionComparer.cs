using Octokit;
using System;
using System.Reflection;

namespace mitoSoft.GitHub.Versioning
{
    public class VersionComparer
    {
        public Version LatestGitHubVersion { get; private set; }

        public Version LocalVersion { get; private set; }

        /// <summary>
        /// read the version number and compares ist to the actual version of the assembly
        /// if lower, write a warning log message
        /// </summary>
        /// <remarks>
        /// In public repos it is possible to read the release versions without an accessToken
        /// Default Assembly is 'ExecutingAssembly'
        /// </remarks>
        public virtual void Check(string owner, string repoName)
        {
            this.Check(owner, repoName, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// read the version number and compares ist to the actual version of the assembly
        /// if lower, write a warning log message
        /// </summary>
        /// <remarks>
        /// In public repos it is possible to read the release versions without an accessToken
        /// </remarks>
        public virtual void Check(string owner, string repoName, Assembly assembly)
        {
            this.IsUpToDate(owner, repoName, assembly, null);
        }

        /// <summary>
        /// Reads the version number and compares it to the actual version of the assembly
        /// if lower, write a warning log message
        /// </summary>
        public virtual bool IsUpToDate(string owner, string repoName, Assembly assembly, string accessToken)
        {
            this.LocalVersion = (new AssemblyVersionHelper(assembly)).GetVersion();

            var productInformation = new ProductHeaderValue(repoName);

            GitHubClient client;
            if (string.IsNullOrEmpty(accessToken))
            {
                client = new GitHubClient(productInformation);
            }
            else
            {
                var credentials = new Credentials(accessToken);
                client = new GitHubClient(productInformation) { Credentials = credentials };
            }
            var releases = client.Repository.Release.GetAll(owner, repoName).GetAwaiter().GetResult();
            this.LatestGitHubVersion = new Version(releases[0].TagName.ToLower().Replace("v", string.Empty).Replace("version", string.Empty));

            int versionComparison = this.LocalVersion.CompareTo(this.LatestGitHubVersion);
            if (versionComparison < 0) //The version on GitHub is more up to date than this local release.
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}