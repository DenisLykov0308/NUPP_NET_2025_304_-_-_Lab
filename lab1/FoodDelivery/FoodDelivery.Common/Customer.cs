namespace FoodDelivery.Common;

public class Customer : Person
{
    public string Address { get; set; } = "";

    public static Customer CreateNew()
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            FullName = "Test Customer",
            Phone = "+380000000000",
            Email = "customer@test.com",
            Address = "Kyiv"
        };
    }
}
