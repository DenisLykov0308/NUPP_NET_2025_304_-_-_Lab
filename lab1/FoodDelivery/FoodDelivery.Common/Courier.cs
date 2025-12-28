namespace FoodDelivery.Common;

public class Courier : Person
{
    public string TransportType { get; set; } = "";
    public bool IsAvailable { get; set; }
    public double Rating { get; set; }

    public delegate void CourierStatusChanged(Courier courier);

    public event CourierStatusChanged? StatusChanged;

    public Courier()
    {
        IsAvailable = true;
        Rating = 5.0;
        TransportType = "Bike";
    }

    public void ChangeAvailability(bool available)
    {
        IsAvailable = available;

        var handler = StatusChanged;
        handler?.Invoke(this);
    }

    public static Courier CreateNew()
    {
        return new Courier
        {
            Id = Guid.NewGuid(),
            FullName = "Test Courier",
            Phone = "+380111111111",
            Email = "courier@test.com",
            TransportType = "Bike",
            IsAvailable = true,
            Rating = 5.0
        };
    }
}
