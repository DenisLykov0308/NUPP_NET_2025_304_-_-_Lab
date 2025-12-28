namespace FoodDelivery.Common;

public class Order : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    public Order()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        Status = "Created";
    }

    public void ChangeStatus(string status)
    {
        Status = status;
    }

    public static Order CreateNew()
    {
        return new Order();
    }
}
