using System;
using System.IO;
using UnityEngine;

namespace CodeBase.Infrastructure.SaveLoad.Storage
{
    public class FileSaveStorage : ISaveStorage
    {
        private readonly string _rootDirectory;
        
        private const string SaveFileName = "game_data";

        public FileSaveStorage(string rootFolderName = "saves")
        {
            _rootDirectory = Path.Combine(Application.persistentDataPath, rootFolderName);
            Directory.CreateDirectory(_rootDirectory);
        }

        public bool Exists()
        {
            var path = GetAbsolutePath();
            return File.Exists(path);
        }

        public string Read()
        {
            var path = GetAbsolutePath();

            if (!File.Exists(path))
                return null;

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public void Write(string payload)
        {
            var path = GetAbsolutePath();
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(stream);
            writer.Write(payload ?? string.Empty);
        }

        public void Delete()
        {
            var path = GetAbsolutePath();
            if (File.Exists(path))
                File.Delete(path);
        }

        public string GetAbsolutePath()
        {
            if (string.IsNullOrWhiteSpace(SaveFileName))
                throw new ArgumentException("Save key cannot be null or empty.", nameof(SaveFileName));

            var fileName = SanitizeFileName(SaveFileName) + ".json";
            return Path.Combine(_rootDirectory, fileName);
        }

        private static string SanitizeFileName(string fileName)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(invalidChar, '_');

            return fileName;
        }
    }
}


