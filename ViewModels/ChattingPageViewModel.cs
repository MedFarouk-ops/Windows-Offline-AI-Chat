using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCodingBot.ViewModels
{
    public class ChattingPageViewModel : ViewModelBase
    {
        public SidebarViewModel Sidebar { get; }
        public ChatViewModel Chat { get; }

        public ChattingPageViewModel(SidebarViewModel sidebar, ChatViewModel chat)
        {
            Sidebar = sidebar;
            Chat = chat;
        }
    }

}
