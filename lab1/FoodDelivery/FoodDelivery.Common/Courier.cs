namespace FoodDelivery.Common;

public class Courier : Person
{
    public string TransportType { get; set; }
    public bool IsAvailable { get; set; }
    public double Rating { get; set; }

    public delegate void CourierStatusChanged(Courier courier);

    public event CourierStatusChanged? StatusChanged;

    public Courier()
    {
        IsAvailable = true;
        Rating = 5.0;
    }

    public void ChangeAvailability(bool available)
    {
        IsAvailable = available;
        StatusChanged?.Invoke(this);
    }
}
