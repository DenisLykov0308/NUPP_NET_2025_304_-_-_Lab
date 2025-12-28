namespace FoodDelivery.Common;

public class Customer : Person
{
    public string Address { get; set; }
    public string Email { get; set; }
    public DateTime RegisteredAt { get; set; }

    public Customer()
    {
        RegisteredAt = DateTime.Now;
    }

    public void UpdateAddress(string newAddress)
    {
        Address = newAddress;
    }
}
