using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Windows;

namespace OfflineCodingBot.Pages
{
    public partial class HomePage : UserControl
    {
        private string fullText = "Please select your model file to continue...";
        private int charIndex = 0;
        private DispatcherTimer typingTimer;

        public HomePage()
        {
            InitializeComponent();
            StartTypingEffect();
        }

        private void StartTypingEffect()
        {
            PromptText.Text = "";
            typingTimer = new DispatcherTimer();
            typingTimer.Interval = TimeSpan.FromMilliseconds(5); // typing speed
            typingTimer.Tick += TypingTimer_Tick;
            typingTimer.Start();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
        private void TypingTimer_Tick(object sender, EventArgs e)
        {
            if (charIndex < fullText.Length)
            {
                PromptText.Text += fullText[charIndex];
                charIndex++;
            }
            else
            {
                typingTimer.Stop();

                // Start pulse animation when typing ends
                var sb = (Storyboard)FindResource("PromptPulse");
                sb.Begin(PromptText, true);
            }
        }

        private void DownloadDeepSeek_Click(object sender, RoutedEventArgs e)
        {
            // Opens the official download URL in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://huggingface.co/ggml-org/DeepSeek-R1-Distill-Qwen-1.5B-Q4_0-GGUF/resolve/main/deepseek-r1-distill-qwen-1.5b-q4_0.gguf",
                UseShellExecute = true
            });
        }

        private void DownloadQueenSeek_Click(object sender, RoutedEventArgs e)
        {
            // Opens the official download URL in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://huggingface.co/ggml-org/Qwen3-1.7B-GGUF/resolve/main/Qwen3-1.7B-Q8_0.gguf",
                UseShellExecute = true
            });
        }

        private void DownloadMetaLLama_Click(object sender, RoutedEventArgs e)
        {
            // Opens the official download URL in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://huggingface.co/QuantFactory/Meta-Llama-3-8B-Instruct-GGUF/resolve/main/Meta-Llama-3-8B-Instruct.Q3_K_L.gguf?download=true",
                UseShellExecute = true
            });
        }

        private void OpenHuggingFaceLink(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://huggingface.co/models",
                UseShellExecute = true
            });
        }
    }
}
