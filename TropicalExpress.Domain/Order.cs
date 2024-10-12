namespace TropicalExpress.Domain;

public class Order(List<Fruit> fruits)
{
    public readonly OrderId Id = new();
    public List<Fruit> Fruits { get; private set; } = fruits;

    public Order() : this([]) { }
}

public readonly record struct OrderId(Guid Value)
{
    public OrderId() : this(Guid.NewGuid()) {}
}