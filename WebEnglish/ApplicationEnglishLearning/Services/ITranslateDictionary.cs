using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;

namespace ApplicationEnglishLearning.Services
{
    public interface ITranslateDictionary<TKey, TValue>
    {
        bool TryAdd(TKey key, TValue tValue);
        bool TryRemove(TKey key, TValue tValue);
        bool TryGetValue(TKey key, out TValue tValue);
        IReadOnlyDictionary<TKey, TValue> Get();
    }
    

    public class TranslateCollection : ITranslateDictionary<string, string>
    {

        private readonly ConcurrentDictionary<string, string> _summaries = new(
            new Dictionary<string, string>()
            {
                {"Freezing", "Обледенение"}, {"Bracing","Укрепление"}, { "Chilly", "Прохладно" }, { "Cool", "Холод" }, { "Mild", "Мягкий" }, { "Warm", "Теплый" }, { "Balmy", "Нежный" }, { "Hot", "Горячий" }, { "Sweltering", "Изнуряющий" }, { "Scorching", "Палящий" }
            });


        public bool TryAdd(string key, string tValue)
        {
            return _summaries.TryAdd(key, tValue);
        }

        public bool TryRemove(string key, string tValue)
        {
            return _summaries.TryRemove(new KeyValuePair<string, string>(key, tValue));
        }

        public bool TryGetValue(string key, out string tValue)
        {
            bool resultGet  = _summaries.TryGetValue(key, out var value);
            tValue = value ?? string.Empty;
            return resultGet;
        }

        public IReadOnlyDictionary<string, string> Get()
        {
            return _summaries;
        }
    }
}
