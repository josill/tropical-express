using System;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents an order in the Tropical Express system.
/// </summary>
public class Order
{
    
    /// <summary>
    /// Gets the unique identifier for this order.
    /// </summary>
    public readonly OrderId Id = new();
    
    /// <summary>
    /// Gets or sets the list of fruits in this order.
    /// </summary>
    public readonly List<Fruit> Fruits = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class with a new <see cref="OrderId"/>.
    /// Used by EF Core.
    /// </summary>
    public Order() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class with the specified fruits.
    /// </summary>
    /// <param name="fruits">The initial list of fruits for this order.</param>
    public Order(List<Fruit> fruits)
    {
        Fruits = fruits;
    }
    
    /// <summary>
    /// Adds a new fruit to the order.
    /// </summary>
    /// <param name="fruitToAdd">The fruit to be added to the order.</param>
    public void AddFruit(Fruit fruitToAdd)
    {
        Fruits.Add(fruitToAdd);
    }

    /// <summary>
    /// Removes a fruit from the order.
    /// </summary>
    /// <param name="fruitToRemove">The fruit to be removed from the order.</param>
    public void RemoveFruit(Fruit fruitToRemove)
    {
        Fruits.Remove(fruitToRemove);
    }
}

/// <summary>
/// Represents a unique identifier for an order.
/// </summary>
/// <param name="Value">The underlying GUID value of the order ID.</param>
public readonly record struct OrderId(Guid Value)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderId"/> struct with a new GUID.
    /// </summary>
    public OrderId() : this(Guid.NewGuid()) {}
}