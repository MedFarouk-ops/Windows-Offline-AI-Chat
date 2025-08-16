using DevExpress.Mvvm;
using OfflineCodingBot.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCodingBot.ViewModels
{

    public class LatestModelsPageViewModel : ViewModelBase
    {
        private readonly RecentModelsService _recentModelsService = new();

        public ObservableCollection<string> RecentModels => _recentModelsService.RecentModels;

        private string _selectedModel;
        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    RaisePropertyChanged(nameof(SelectedModel));
                    LoadModelCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand<string> LoadModelCommand { get; }

        public event Action<string> OnModelSelected;

        public LatestModelsPageViewModel()
        {
            LoadModelCommand = new DelegateCommand<string>(LoadModel, CanLoadModel);
        }

        private bool CanLoadModel(string path) => !string.IsNullOrWhiteSpace(path);

        private void LoadModel(string path)
        {
            OnModelSelected?.Invoke(path);
        }
    }
}
