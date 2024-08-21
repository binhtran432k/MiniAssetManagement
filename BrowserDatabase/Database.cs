using Blazored.LocalStorage;
namespace BrowserDatabase;

public class Database<TKey, TValue>(
    ISyncLocalStorageService localStorage,
    string databaseKey
) where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _database
        = localStorage.GetItem<Dictionary<TKey, TValue>>(databaseKey)
          ?? [];

    public List<TValue> ReadAll()
    {
        return [.. _database.Values];
    }

    public int Count()
    {
        return _database.Count;
    }

    public bool TryCreate(TKey key, TValue value)
    {
        if (_database.ContainsKey(key))
            return false;
        _database.Add(key, value);
        Save();
        return true;
    }

    public TValue? Read(TKey key)
    {
        return _database.GetValueOrDefault(key);
    }

    public bool TryUpdate(TKey key, TValue value)
    {
        if (!_database.ContainsKey(key))
            return false;
        _database.Add(key, value);
        Save();
        return true;
    }

    public bool TryRemove(TKey key)
    {
        if (!_database.ContainsKey(key))
            return false;
        _database.Remove(key);
        Save();
        return true;
    }

    private void Save()
    {
        localStorage.SetItem(databaseKey, _database);
    }
}
