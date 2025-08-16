using DevExpress.Mvvm;
using Microsoft.Win32;
using OfflineCodingBot.Helpers;
using OfflineCodingBot.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OfflineCodingBot.Services.DataServices;
using OfflineCodingBot.Services.LLMServices;

namespace OfflineCodingBot.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private string _modelPath;
        private bool _isLoading;
        private int _loadingProgress;
        private string _selectedRecentModel;

        private readonly RecentModelsService _recentModelsService = new();

        public ObservableCollection<string> RecentModels => _recentModelsService.RecentModels;

        public string SelectedRecentModel
        {
            get => _selectedRecentModel;
            set
            {
                if (_selectedRecentModel != value)
                {
                    _selectedRecentModel = value;
                    RaisePropertyChanged(nameof(SelectedRecentModel));
                    LoadRecentModelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ModelPath
        {
            get => _modelPath;
            set
            {
                if (_modelPath != value)
                {
                    _modelPath = value;
                    RaisePropertyChanged(nameof(ModelPath));
                    RaisePropertyChanged(nameof(CanGoNext));
                }
            }
        }

        public bool CanGoNext => !string.IsNullOrWhiteSpace(ModelPath);

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        public int LoadingProgress
        {
            get => _loadingProgress;
            set
            {
                _loadingProgress = value;
                RaisePropertyChanged();
            }
        }

        public SettingsPageViewModel SettingsVM { get; } = new SettingsPageViewModel();

        public DelegateCommand BrowseModelCommand { get; }
        public DelegateCommand NextCommand { get; }
        public DelegateCommand<string> LoadRecentModelCommand { get; }

        public event Action<LocalLLMService> OnModelLoaded;

        public HomePageViewModel()
        {
            BrowseModelCommand = new DelegateCommand(OnBrowse);
            NextCommand = new DelegateCommand(async () => await OnNextAsync(), () => CanGoNext);
            LoadRecentModelCommand = new DelegateCommand<string>(LoadRecentModel, path => !string.IsNullOrWhiteSpace(path));

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CanGoNext))
                {
                    NextCommand.RaiseCanExecuteChanged();
                }
            };
        }

        private void OnBrowse()
        {
            var dlg = new OpenFileDialog()
            {
                Title = "Select GGML model file",
                Filter = "GGML Model Files (*.bin;*.gguf)|*.bin;*.gguf|All files|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                ModelPath = dlg.FileName;
            }
        }

        private async Task OnNextAsync()
        {
            if (!CanGoNext) return;

            try
            {
                IsLoading = true;
                LoadingProgress = 0;

                // Simulated loading progress (replace with real logic)
                await Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        Thread.Sleep(30); // Simulate work
                        LoadingProgress = i;
                        RaisePropertyChanged(nameof(LoadingProgress));
                    }
                });

                var llmService = new LocalLLMService(ModelPath, SettingsVM.ContextSize, SettingsVM.GpuLayerCount, SettingsVM.MaxTokens,
                    SettingsVM.Temperature , SettingsVM.TopK, SettingsVM.TopP, SettingsVM.RepeatPenalty,
                    SettingsVM.FrequencyPenalty, SettingsVM.PresencePenalty);

                _recentModelsService.AddModel(ModelPath); // Add to recent list on load
                OnModelLoaded?.Invoke(llmService);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load model:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadRecentModel(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            ModelPath = path;
            // Optionally you can start loading immediately by calling OnNextAsync here, but be careful since it is async
            // You might want to await or trigger loading differently depending on your UI flow.
        }
    }
}
