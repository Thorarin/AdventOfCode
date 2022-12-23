using System.Net.Http.Headers;

namespace Thorarin.AdventOfCode.Extensions;

public static class DictionaryExtensions
{
    public static TValue AddOrUpdate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        var newValue = dictionary.TryGetValue(key, out var value) ? updateValueFactory(key, value) : addValue;
        dictionary[key] = newValue;
        return newValue;
    }

    public static TValue AddOrUpdate<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key,
        TValue addValue, Func<TValue, TValue> updateValueFactory)
    {
        var newValue = dictionary.TryGetValue(key, out var value) ? updateValueFactory(value) : addValue;
        dictionary[key] = newValue;
        return newValue;
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue)
    {
        if (!dictionary.TryGetValue(key, out var value))
        {
            dictionary.Add(key, addValue);
            return addValue;
        }

        return value;
    }
    
    public static long Increment<TKey>(this IDictionary<TKey, long> dictionary, TKey key, long increment)
    {
        var newValue = dictionary.TryGetValue(key, out var value) ? value + increment : increment;
        dictionary[key] = newValue;
        return newValue;
    }
}    