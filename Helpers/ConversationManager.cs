using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using OfflineCodingBot.Models;

namespace OfflineCodingBot.Helpers
{
    public class ConversationManager
    {
        private readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OfflineCodingBot",
            "conversations.json"
        );

        public ObservableCollection<Conversation> Conversations { get; private set; } = new();

        public ConversationManager()
        {
            LoadConversations();
        }

        public void AddConversation(Conversation convo)
        {
            Conversations.Add(convo);
            SaveConversations();
        }

        public void RemoveConversation(Conversation convo)
        {
            if (Conversations.Contains(convo))
            {
                Conversations.Remove(convo);
                SaveConversations();
            }
        }

        public void SaveConversations()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            var json = JsonSerializer.Serialize(Conversations);
            File.WriteAllText(_filePath, json);
        }

        public void LoadConversations()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                Conversations = JsonSerializer.Deserialize<ObservableCollection<Conversation>>(json) ?? new ObservableCollection<Conversation>();
            }
            else
            {
                Conversations = new ObservableCollection<Conversation>();
            }
        }
    }
}
