namespace FoodDelivery.Common;

public abstract class Person
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }

    public static int PeopleCount;

    static Person()
    {
        PeopleCount = 0;
    }

    protected Person()
    {
        Id = Guid.NewGuid();
        PeopleCount++;
    }

    public string GetContactInfo()
    {
        return $"{FullName}, {Phone}";
    }
}
