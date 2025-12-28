using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace FoodDelivery.Common;

public sealed class FileBackedCrudServiceAsync<T> : ICrudServiceAsync<T>
    where T : class, IEntity
{
    private readonly ConcurrentDictionary<Guid, T> _storage = new();
    private readonly SemaphoreSlim _fileGate = new(1, 1);

    public string FilePath { get; }

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public FileBackedCrudServiceAsync(string filePath)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        LoadFromFileAsync().GetAwaiter().GetResult();
    }

    public Task<bool> CreateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        if (element.Id == Guid.Empty)
            element.Id = Guid.NewGuid();

        var added = _storage.TryAdd(element.Id, element);
        return Task.FromResult(added);
    }

    public Task<T> ReadAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id не може бути порожнім.", nameof(id));

        if (_storage.TryGetValue(id, out var element))
            return Task.FromResult(element);

        throw new KeyNotFoundException("Елемент не знайдено.");
    }

    public Task<IEnumerable<T>> ReadAllAsync()
    {
        var snapshot = _storage.Values.ToList();
        return Task.FromResult<IEnumerable<T>>(snapshot);
    }

    public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page має бути >= 1.");
        if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Amount має бути >= 1.");

        var skip = (page - 1) * amount;

        var pageItems = _storage
            .OrderBy(kvp => kvp.Key)
            .Skip(skip)
            .Take(amount)
            .Select(kvp => kvp.Value)
            .ToList();

        return Task.FromResult<IEnumerable<T>>(pageItems);
    }

    public Task<bool> UpdateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        if (element.Id == Guid.Empty) throw new ArgumentException("Id не може бути порожнім.", nameof(element));

        if (!_storage.ContainsKey(element.Id))
            return Task.FromResult(false);

        _storage[element.Id] = element;
        return Task.FromResult(true);
    }

    public Task<bool> RemoveAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        if (element.Id == Guid.Empty) throw new ArgumentException("Id не може бути порожнім.", nameof(element));

        var removed = _storage.TryRemove(element.Id, out _);
        return Task.FromResult(removed);
    }

    public async Task<bool> SaveAsync()
    {
        await _fileGate.WaitAsync();
        try
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            var snapshot = _storage.Values.ToList();
            var json = JsonSerializer.Serialize(snapshot, _jsonOptions);
            await File.WriteAllTextAsync(FilePath, json);

            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            _fileGate.Release();
        }
    }

    private async Task LoadFromFileAsync()
    {
        await _fileGate.WaitAsync();
        try
        {
            if (!File.Exists(FilePath))
                return;

            var json = await File.ReadAllTextAsync(FilePath);
            if (string.IsNullOrWhiteSpace(json))
                return;

            var items = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            if (items is null) return;

            _storage.Clear();
            foreach (var item in items)
            {
                if (item is null) continue;
                if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
                _storage[item.Id] = item;
            }
        }
        finally
        {
            _fileGate.Release();
        }
    }

    public IEnumerator<T> GetEnumerator()
        => _storage.Values.ToList().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
