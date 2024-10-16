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
    public Fruit Fruit { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class with a new <see cref="OrderId"/>.
    /// Used by EF Core.
    /// </summary>
    public Order() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class with the specified fruits.
    /// </summary>
    /// <param name="fruits">The initial list of fruits for this order.</param>
    public Order(Fruit fruit)
    {
        Fruit = fruit;
    }
    
    /// <summary>
    /// Updates the fruit associated with this order.
    /// </summary>
    /// <param name="newFruit">The new fruit to associate with this order.</param>
    /// <exception cref="ArgumentNullException">Thrown if newFruit is null.</exception>
    public void UpdateFruit(Fruit newFruit)
    {
        Fruit = newFruit;
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