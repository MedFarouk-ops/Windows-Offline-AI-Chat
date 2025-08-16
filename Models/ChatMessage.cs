using DevExpress.Mvvm;
using System;

public class ChatMessage : ViewModelBase
{
    private string _text;
    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value, nameof(Text));
    }

    public bool IsUser { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
