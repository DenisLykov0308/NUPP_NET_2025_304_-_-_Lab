namespace FoodDelivery.Common;

public static class PersonExtensions
{
    public static string ToShortString(this Person person)
    {
        return $"{person.FullName} ({person.Phone})";
    }
}
