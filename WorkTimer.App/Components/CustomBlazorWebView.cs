using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace WorkTimer.App.Components
{
    public class CustomBlazorWebView : BlazorWebView
    {
        public override IFileProvider CreateFileProvider(string contentRootDir)
        {
            var baseFileProvider = base.CreateFileProvider(contentRootDir);
            return new FixUrlFileProvider(baseFileProvider);
        }
    }

    public class FixUrlFileProvider : IFileProvider
    {
        private readonly IFileProvider baseFileProvider;

        public FixUrlFileProvider(IFileProvider baseFileProvider)
        {
            this.baseFileProvider = baseFileProvider;
        }

        public IDirectoryContents GetDirectoryContents(string subpath) => baseFileProvider.GetDirectoryContents(subpath);

        public IFileInfo GetFileInfo(string subpath) => baseFileProvider.GetFileInfo(GetPathWithoutFragment(subpath));

        public IChangeToken Watch(string filter) => baseFileProvider.Watch(filter);

        private string GetPathWithoutFragment(string subpath)
        {
            var fragmentIndex = subpath.IndexOf('#');
            if (fragmentIndex != -1)
                subpath = subpath.Substring(0, fragmentIndex);
            return subpath;
        }
    }

    public sealed class ModifyFileProvider : IFileProvider
    {
        private readonly IFileProvider baseFileProvider;
        public Dictionary<string, Func<string, string>> Modifications = new();

        public ModifyFileProvider(IFileProvider baseFileProvider)
        {
            this.baseFileProvider = baseFileProvider;
        }

        public IDirectoryContents GetDirectoryContents(string subpath) => null;

        public IFileInfo GetFileInfo(string subpath)
        {
            if (!Modifications.ContainsKey(subpath)) return baseFileProvider.GetFileInfo(subpath);

            var baseFileInfo = baseFileProvider.GetFileInfo(subpath);
            if (baseFileInfo == null || !baseFileInfo.Exists) return null;

            using var stream = baseFileInfo.CreateReadStream();
            var modification = Modifications[subpath];
            using var reader = new StreamReader(stream);
            var fileContent = reader.ReadToEnd();
            var newFileContent = modification(fileContent);
            return new ModifiedFileInfo(baseFileInfo, newFileContent);
        }

        public IChangeToken Watch(string filter) => null;

        class ModifiedFileInfo : IFileInfo
        {
            private readonly string fileContent;

            public ModifiedFileInfo(IFileInfo fileInfo, string fileContent)
            {
                this.fileContent = fileContent;
                Exists = fileInfo.Exists;
                Length = fileContent.Length;
                PhysicalPath = fileInfo.PhysicalPath;
                Name = fileInfo.Name;
                LastModified = fileInfo.LastModified;
                IsDirectory = fileInfo.IsDirectory;
            }

            public bool Exists { get; private set; }

            public long Length { get; private set; }

            public string PhysicalPath { get; private set; }

            public string Name { get; private set; }

            public DateTimeOffset LastModified { get; private set; }

            public bool IsDirectory { get; private set; }

            public Stream CreateReadStream() => new MemoryStream(Encoding.Default.GetBytes(fileContent));
        }
    }

}