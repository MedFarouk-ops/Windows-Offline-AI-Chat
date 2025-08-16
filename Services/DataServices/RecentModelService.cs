using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OfflineCodingBot.Services.DataServices
{
    public class RecentModelsService
    {
        private const int MaxRecentModels = 10;
        private readonly string _filePath;

        public ObservableCollection<string> RecentModels { get; private set; }

        public RecentModelsService()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OfflineCodingBot", "recent_models.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            RecentModels = new ObservableCollection<string>(LoadRecentModels());
        }

        private string[] LoadRecentModels()
        {
            if (!File.Exists(_filePath))
                return Array.Empty<string>();

            try
            {
                var json = File.ReadAllText(_filePath);
                var models = JsonSerializer.Deserialize<string[]>(json);
                return models ?? Array.Empty<string>();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }

        public void AddModel(string modelPath)
        {
            if (string.IsNullOrWhiteSpace(modelPath))
                return;

            // Remove if already exists
            var existing = RecentModels.FirstOrDefault(x => string.Equals(x, modelPath, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                RecentModels.Remove(existing);

            // Insert at the start (most recent first)
            RecentModels.Insert(0, modelPath);

            // Limit list size
            while (RecentModels.Count > MaxRecentModels)
                RecentModels.RemoveAt(RecentModels.Count - 1);

            SaveRecentModels();
        }

        public void SaveRecentModels()
        {
            try
            {
                var json = JsonSerializer.Serialize(RecentModels.ToArray(), new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch
            {
                // Handle exceptions as needed (e.g., logging)
            }
        }
    }
}
