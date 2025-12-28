using System.Collections.Concurrent;

namespace FoodDelivery.Common;

public class InMemoryCrudService<T> : ICrudService<T> where T : class, IEntity
{
    private static readonly ConcurrentDictionary<Guid, T> _storage;

    static InMemoryCrudService()
    {
        _storage = new ConcurrentDictionary<Guid, T>();
    }

    public void Create(T element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));

        if (element.Id == Guid.Empty)
            element.Id = Guid.NewGuid();

        if (!_storage.TryAdd(element.Id, element))
            throw new InvalidOperationException("Елемент з таким Id вже існує.");
    }

    public T Read(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id не може бути порожнім.", nameof(id));

        if (_storage.TryGetValue(id, out var element))
            return element;

        throw new KeyNotFoundException("Елемент не знайдено.");
    }

    public IEnumerable<T> ReadAll()
    {
        return _storage.Values;
    }

    public void Update(T element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (element.Id == Guid.Empty) throw new ArgumentException("Id не може бути порожнім.", nameof(element));

        if (!_storage.ContainsKey(element.Id))
            throw new KeyNotFoundException("Елемент для оновлення не знайдено.");

        _storage[element.Id] = element;
    }

    public void Remove(T element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (element.Id == Guid.Empty) throw new ArgumentException("Id не може бути порожнім.", nameof(element));

        _storage.TryRemove(element.Id, out _);
    }
}
