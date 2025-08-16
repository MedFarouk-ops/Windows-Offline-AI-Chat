using DevExpress.Mvvm;

namespace OfflineCodingBot.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private uint _contextSize = 1024;
        public uint ContextSize
        {
            get => _contextSize;
            set
            {
                if (_contextSize != value)
                {
                    _contextSize = value;
                    RaisePropertyChanged(nameof(ContextSize));
                }
            }
        }

        private int _gpuLayerCount = 5;
        public int GpuLayerCount
        {
            get => _gpuLayerCount;
            set
            {
                if (_gpuLayerCount != value)
                {
                    _gpuLayerCount = value;
                    RaisePropertyChanged(nameof(GpuLayerCount));
                }
            }
        }

        private int _maxTokens = 1024;
        public int MaxTokens
        {
            get => _maxTokens;
            set
            {
                if (_maxTokens != value)
                {
                    _maxTokens = value;
                    RaisePropertyChanged(nameof(MaxTokens));
                }
            }
        }

        private float _temperature = 0.8f;
        public float Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    RaisePropertyChanged(nameof(Temperature));
                }
            }
        }

        private int _topK = 40;
        public int TopK
        {
            get => _topK;
            set
            {
                if (_topK != value)
                {
                    _topK = value;
                    RaisePropertyChanged(nameof(TopK));
                }
            }
        }

        private float _topP = 0.9f;
        public float TopP
        {
            get => _topP;
            set
            {
                if (_topP != value)
                {
                    _topP = value;
                    RaisePropertyChanged(nameof(TopP));
                }
            }
        }

        private float _repeatPenalty = 1.1f;
        public float RepeatPenalty
        {
            get => _repeatPenalty;
            set
            {
                if (_repeatPenalty != value)
                {
                    _repeatPenalty = value;
                    RaisePropertyChanged(nameof(RepeatPenalty));
                }
            }
        }

        private float _frequencyPenalty = 0.0f;
        public float FrequencyPenalty
        {
            get => _frequencyPenalty;
            set
            {
                if (_frequencyPenalty != value)
                {
                    _frequencyPenalty = value;
                    RaisePropertyChanged(nameof(FrequencyPenalty));
                }
            }
        }

        private float _presencePenalty = 0.6f;
        public float PresencePenalty
        {
            get => _presencePenalty;
            set
            {
                if (_presencePenalty != value)
                {
                    _presencePenalty = value;
                    RaisePropertyChanged(nameof(PresencePenalty));
                }
            }
        }

    }
}
