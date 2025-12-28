using FoodDelivery.Common;

Console.WriteLine("=== CRUD demo: FoodDelivery ===");

var service = new InMemoryCrudService<Order>();

var order1 = new Order();
var order2 = new Order();
order2.ChangeStatus("In delivery");

service.Create(order1);
service.Create(order2);

Console.WriteLine("\nCREATE: Додано 2 замовлення");

Console.WriteLine("\nREADALL: Список замовлень:");
foreach (var order in service.ReadAll())
{
    Console.WriteLine($"- Id: {order.Id} | CreatedAt: {order.CreatedAt} | Status: {order.Status}");
}

Console.WriteLine("\nREAD: Отримуємо перше замовлення:");
var readOrder = service.Read(order1.Id);
Console.WriteLine($"Знайдено: {readOrder.Id} | Status: {readOrder.Status}");

Console.WriteLine("\nUPDATE: Змінюємо статус першого замовлення на Delivered");
readOrder.ChangeStatus("Delivered");
service.Update(readOrder);

Console.WriteLine($"Після оновлення: {service.Read(order1.Id).Status}");

Console.WriteLine("\nREMOVE: Видаляємо друге замовлення");
service.Remove(order2);

Console.WriteLine("\nREADALL після видалення:");
foreach (var order in service.ReadAll())
{
    Console.WriteLine($"- Id: {order.Id} | Status: {order.Status}");
}

Console.WriteLine("\n=== CRUD demo finished ===");
Console.ReadKey();
