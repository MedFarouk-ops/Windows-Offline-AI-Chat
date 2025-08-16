using DevExpress.Mvvm;
using OfflineCodingBot.Models;
using OfflineCodingBot.Services;
using OfflineCodingBot.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using OfflineCodingBot.Services.LLMServices;

public class ChatViewModel : ViewModelBase
{
    private readonly LocalLLMService _llmService;
    private readonly SidebarViewModel? _sidebarVm;
    private Conversation _currentConversation;
    private string _currentMessage;
    private CancellationTokenSource? _cts;
    private bool _isGenerating;

    public Conversation CurrentConversation
    {
        get => _currentConversation;
        set
        {
            if (_currentConversation != value)
            {
                _currentConversation = value ?? new Conversation { Title = "New Conversation", Messages = new ObservableCollection<ChatMessage>() };

                // Ensure Messages collection is not null
                if (_currentConversation.Messages == null)
                    _currentConversation.Messages = new ObservableCollection<ChatMessage>();

                RaisePropertyChanged(nameof(Messages));
            }
        }
    }

    // Bind directly to the current conversation's messages
    public ObservableCollection<ChatMessage> Messages => CurrentConversation.Messages;

    public string CurrentMessage
    {
        get => _currentMessage;
        set
        {
            if (_currentMessage != value)
            {
                _currentMessage = value;
                RaisePropertyChanged(nameof(CurrentMessage));
                (SendMessageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public bool IsGenerating
    {
        get => _isGenerating;
        set
        {
            if (_isGenerating != value)
            {
                _isGenerating = value;
                RaisePropertyChanged(nameof(IsGenerating));
                (SendMessageCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (StopCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }
    }

    public DelegateCommand SendMessageCommand { get; }
    public DelegateCommand StopCommand { get; }

    public ChatViewModel(LocalLLMService llmService, Conversation? initialConversation = null, SidebarViewModel? sidebarVm = null)
    {
        _llmService = llmService;
        _sidebarVm = sidebarVm;

        CurrentConversation = initialConversation
                              ?? _sidebarVm?.SelectedConversation
                              ?? new Conversation { Title = "New Conversation", Messages = new ObservableCollection<ChatMessage>() };

        SendMessageCommand = new DelegateCommand(async () => await SendMessageAsync(), CanSend);
        StopCommand = new DelegateCommand(() => CancelGeneration(), CanStop);

        if (Messages.Count == 0)
            Messages.Add(new ChatMessage { Text = "Welcome! This is an offline coding assistant UI.", IsUser = false });

        if (_sidebarVm != null)
            _sidebarVm.PropertyChanged += SidebarVm_PropertyChanged;
    }

    private void SidebarVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SidebarViewModel.SelectedConversation))
        {
            CurrentConversation = _sidebarVm.SelectedConversation;
        }
    }

    private bool CanSend() => !IsGenerating && !string.IsNullOrWhiteSpace(CurrentMessage);
    private bool CanStop() => IsGenerating;

    private void CancelGeneration()
    {
        if (IsGenerating)
            _cts?.Cancel(true);
    }

    private async Task SendMessageAsync()
    {
        if (IsGenerating || string.IsNullOrWhiteSpace(CurrentMessage))
            return;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        IsGenerating = true;

        var userMessage = CurrentMessage;
        Messages.Add(new ChatMessage { Text = userMessage, IsUser = true });
        CurrentMessage = string.Empty;

        ChatMessage? assistantMessage = null;

        try
        {
            await foreach (var chunk in _llmService.GetResponseStreamAsync(userMessage, _cts.Token))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (assistantMessage == null)
                    {
                        assistantMessage = new ChatMessage { Text = chunk, IsUser = false };
                        Messages.Add(assistantMessage);
                    }
                    else
                    {
                        assistantMessage.Text += chunk;
                    }
                });
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (assistantMessage != null && assistantMessage.Text.EndsWith("User:"))
                {
                    assistantMessage.Text = assistantMessage.Text[..^"User:".Length].TrimEnd();
                }
            });

            // Save updated conversation list
            _sidebarVm?.SaveConversations();
        }
        catch (OperationCanceledException)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (assistantMessage != null)
                {
                    assistantMessage.Text += "\n\n[Generation stopped by user]";
                    _llmService.AddAssistantMessageToHistory(assistantMessage.Text);
                }
            });
            _sidebarVm?.SaveConversations();
        }
        catch (Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(new ChatMessage { Text = $"[Error]: {ex.Message}", IsUser = false });
            });
        }
        finally
        {
            IsGenerating = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    // ========== UI Settings for Font & Background ==========
    private FontFamily _chatFontFamily = new FontFamily("Segoe UI Semibold");
    public FontFamily ChatFontFamily
    {
        get => _chatFontFamily;
        set
        {
            if (_chatFontFamily != value)
            {
                _chatFontFamily = value;
                RaisePropertyChanged(nameof(ChatFontFamily));
            }
        }
    }

    private string _chatBackgroundColorString = "White";
    public string ChatBackgroundColorString
    {
        get => _chatBackgroundColorString;
        set
        {
            if (_chatBackgroundColorString != value)
            {
                _chatBackgroundColorString = value;
                RaisePropertyChanged(nameof(ChatBackgroundColorString));

                ChatBackgroundBrush = ConvertStringToBrush(value);
            }
        }
    }

    private Brush _chatBackgroundBrush = Brushes.White;
    public Brush ChatBackgroundBrush
    {
        get => _chatBackgroundBrush;
        private set
        {
            if (_chatBackgroundBrush != value)
            {
                _chatBackgroundBrush = value;
                RaisePropertyChanged(nameof(ChatBackgroundBrush));
            }
        }
    }

    private Brush ConvertStringToBrush(string colorString)
    {
        try
        {
            return (Brush)(new BrushConverter().ConvertFromString(colorString) ?? Brushes.White);
        }
        catch
        {
            return Brushes.White;
        }
    }


}
