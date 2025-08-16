using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCodingBot.Models
{
    public class Conversation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "New Conversation";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ObservableCollection<ChatMessage> Messages { get; set; } = new();
    }

}
