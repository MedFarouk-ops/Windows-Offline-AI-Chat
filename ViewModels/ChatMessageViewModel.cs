using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCodingBot.ViewModels
{
    public class ChatMessageViewModel
    {
        public string Text { get; set; }
        public bool IsUser { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
