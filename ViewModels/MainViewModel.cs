using DevExpress.Mvvm;
using OfflineCodingBot.Services;
using OfflineCodingBot.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using OfflineCodingBot.Services.LLMServices;
using OfflineCodingBot.Views.PagesView;
public class MainViewModel : ViewModelBase
{
    public SidebarViewModel Sidebar { get; }

    private UserControl _currentPage;
    public UserControl CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                RaisePropertyChanged(nameof(CurrentPage));
            }
        }
    }

    public MainViewModel()
    {
        Sidebar = new SidebarViewModel();

        // Show HomePage first
        ShowHomePage();
    }

    private void ShowHomePage()
    {
        var homeVm = new HomePageViewModel();
        var homePage = new HomePage() { DataContext = homeVm };

        homeVm.OnModelLoaded += OnModelLoaded;

        CurrentPage = homePage;
    }

    private async void OnModelLoaded(LocalLLMService llmService)
    {
        // ensure there's a selected conversation
        if (Sidebar.SelectedConversation == null)
        {
            // call the command to create a new conversation (keeps MVVM)
            Sidebar.NewConversationCommand.Execute(null);
        }

        // create ChatViewModel with the selected conversation and pass Sidebar for saving
        var chatVm = new ChatViewModel(llmService, Sidebar.SelectedConversation, Sidebar);

        var chattingPage = new ChattingPage
        {
            DataContext = new ChattingPageViewModel(Sidebar, chatVm)
        };

        await Task.Delay(100); // optional short delay for UI update
        CurrentPage = chattingPage;
    }

  

}
