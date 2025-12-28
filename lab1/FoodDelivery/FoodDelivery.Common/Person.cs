namespace FoodDelivery.Common;

public abstract class Person : IEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
}
