using DevExpress.Mvvm;
using OfflineCodingBot.Helpers;
using OfflineCodingBot.Models;
using OfflineCodingBot.Services;
using System.Collections.ObjectModel;

namespace OfflineCodingBot.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        private readonly ConversationManager _conversationManager;

        public ObservableCollection<Conversation> Conversations => _conversationManager.Conversations;

        private Conversation _selectedConversation;
        public Conversation SelectedConversation
        {
            get => _selectedConversation;
            set
            {
                if (_selectedConversation != value)
                {
                    _selectedConversation = value;
                    RaisePropertyChanged(nameof(SelectedConversation));

                    // Update CanExecute for Delete and Rename commands when selection changes
                    DeleteConversationCommand.RaiseCanExecuteChanged();
                    RenameConversationCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand NewConversationCommand { get; }
        public DelegateCommand DeleteConversationCommand { get; }
        public DelegateCommand RenameConversationCommand { get; }

        public SidebarViewModel()
        {
            _conversationManager = new ConversationManager();

            NewConversationCommand = new DelegateCommand(CreateNewConversation);
            DeleteConversationCommand = new DelegateCommand(DeleteConversation, CanModifySelected);
            RenameConversationCommand = new DelegateCommand(RenameConversation, CanModifySelected);

            if (Conversations.Count == 0)
                CreateNewConversation();
        }

        private void CreateNewConversation()
        {
            var convo = new Conversation { Title = $"Chat {Conversations.Count + 1}" };
            _conversationManager.AddConversation(convo);
            SelectedConversation = convo;
        }

        private bool CanModifySelected() => SelectedConversation != null;

        private void DeleteConversation()
        {
            if (SelectedConversation != null)
            {
                _conversationManager.RemoveConversation(SelectedConversation);
                SelectedConversation = Conversations.Count > 0 ? Conversations[0] : null;
            }
        }

        private void RenameConversation()
        {
            if (SelectedConversation != null)
            {
                // For now just simple rename, replace this with a dialog or inline editing later
                SelectedConversation.Title = $"Renamed {SelectedConversation.Title}";
                RaisePropertyChanged(nameof(Conversations)); // Notify UI in case of binding quirks
                SaveConversations();
            }
        }

        public void SaveConversations() => _conversationManager.SaveConversations();
    }
}
