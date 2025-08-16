using LLama.Common;
using LLama.Sampling;
using LLama;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OfflineCodingBot.Services.LLMServices
{
    public class LocalLLMService : IDisposable
    {
        private readonly InteractiveExecutor _executor;
        private readonly ChatSession _session;
        private readonly int _maxTokens;
        private readonly float _temperature;       // Creativity
        private readonly int _topK;              // Top K sampling
        private readonly float _topP;              // Nucleus sampling
        private readonly float _repeatPenalty;     // Penalize repetition
        private readonly float _frequencyPenalty;  // Penalize frequent tokens
        private readonly float _presencePenalty;   // Encourage new topics

        public LocalLLMService(
            string modelPath,
            uint contextSize,
            int gpuLayerCount,
            int maxTokens,
            float temperature = 0.8f,
            int topK = 40,
            float topP = 0.9f,
            float repeatPenalty = 1.1f,
            float frequencyPenalty = 0.0f,
            float presencePenalty = 0.6f
        )
        {
            _maxTokens = maxTokens;
            _temperature = temperature;
            _topK = topK;
            _topP = topP;
            _repeatPenalty = repeatPenalty;
            _frequencyPenalty = frequencyPenalty;
            _presencePenalty = presencePenalty;

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = contextSize,
                GpuLayerCount = gpuLayerCount
            };
            var model = LLamaWeights.LoadFromFile(parameters);
            var context = model.CreateContext(parameters);
            _executor = new InteractiveExecutor(context);

            var chatHistory = new ChatHistory();
            chatHistory.AddMessage(AuthorRole.System,
                "You are Bob, a skilled programming assistant. You help users by providing clear, accurate, and concise coding advice, debugging help, and explanations. Always answer with programming best practices and helpful examples.");

            _session = new ChatSession(_executor, chatHistory);
        }

        public async IAsyncEnumerable<string> GetResponseStreamAsync(
      string userMessage,
      [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var samplingPipeline = new DefaultSamplingPipeline
            {
                Temperature = _temperature,       // Creativity
                TopK = _topK,                     // Top K sampling
                TopP = _topP,                     // Nucleus sampling
                RepeatPenalty = _repeatPenalty,   // Penalize repetition
                FrequencyPenalty = _frequencyPenalty, // Penalize frequent tokens
                PresencePenalty = _presencePenalty    // Encourage new topics
            };


            var inferenceParams = new InferenceParams()
            {
                MaxTokens = _maxTokens,
                AntiPrompts = new List<string> { "User:", "\n\n\n", },
                SamplingPipeline = samplingPipeline,
            };

            var fullResponse = string.Empty;

            await foreach (var text in _session.ChatAsync(
                new ChatHistory.Message(AuthorRole.User, userMessage),
                inferenceParams))
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                fullResponse += text;

                yield return text;
            }

            if (!string.IsNullOrWhiteSpace(fullResponse) && fullResponse.Length > 50 && !fullResponse.StartsWith("User:"))
                _session.History.AddMessage(AuthorRole.Assistant, fullResponse);
            else
            {
                yield return "⚠️ Model failed to respond properly.";
            }
        }


        public void Dispose()
        {
            // Dispose executor if needed
            //_executor?.Dispose();
        }

        public void AddAssistantMessageToHistory(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                _session.History.AddMessage(AuthorRole.Assistant, text);
            }
        }
    }
}
