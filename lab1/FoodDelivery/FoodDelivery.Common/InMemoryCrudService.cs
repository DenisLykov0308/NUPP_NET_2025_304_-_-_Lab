using System.Collections.Concurrent;
using System.Threading;

namespace FoodDelivery.Common;

public class InMemoryCrudService<T> : ICrudService<T>
    where T : class, IEntity
{
    private static readonly ConcurrentDictionary<Guid, T> Storage;
    private static readonly object _lock = new();
    private static int _createdCount;

    static InMemoryCrudService()
    {
        Storage = new ConcurrentDictionary<Guid, T>();
    }

    public void Create(T element)
    {
        if (element.Id == Guid.Empty)
            element.Id = Guid.NewGuid();

        Storage[element.Id] = element;

        Interlocked.Increment(ref _createdCount);

        lock (_lock)
        {
           
        }
    }

    public T Read(Guid id)
    {
        return Storage[id];
    }

    public IEnumerable<T> ReadAll()
    {
        return Storage.Values;
    }

    public void Update(T element)
    {
        Storage[element.Id] = element;
    }

    public void Remove(T element)
    {
        Storage.TryRemove(element.Id, out _);
    }

    public int GetCreatedCount() => _createdCount;
}
