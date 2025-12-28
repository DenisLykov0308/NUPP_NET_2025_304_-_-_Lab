using System.Collections.Concurrent;

namespace FoodDelivery.Common;

public class InMemoryCrudService<T> : ICrudService<T>
    where T : class, IEntity
{

    private static readonly ConcurrentDictionary<Guid, T> Storage;

    static InMemoryCrudService()
    {
        Storage = new ConcurrentDictionary<Guid, T>();
    }

    public void Create(T element)
    {
        if (element.Id == Guid.Empty)
            element.Id = Guid.NewGuid();

        Storage[element.Id] = element;
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
}
