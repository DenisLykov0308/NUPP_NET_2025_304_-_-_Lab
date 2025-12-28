namespace FoodDelivery.Common;

public abstract class Person : IEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

    protected Person()
    {
        Id = Guid.NewGuid();
    }

    public string GetInfo()
    {
        return $"{FullName} ({Phone})";
    }
}
