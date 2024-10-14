using Microsoft.EntityFrameworkCore;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents a fruit item, which can be packaged or unpackaged.
/// </summary>
public class Fruit : ValueObject<Fruit>
{
    /// <summary>
    /// Gets the type of fruit.
    /// </summary>
    public FruitType FruitType { get; }

    /// <summary>
    /// Gets the net weight of the fruit.
    /// </summary>
    public NetWeight NetWeight { get; }
    
    private Fruit() {} // EF Core needs this

    /// <summary>
    /// Initializes a new instance of the <see cref="Fruit"/> class without packaging.
    /// </summary>
    /// <param name="fruitType">The type of fruit.</param>
    /// <param name="netWeight">The net weight of the fruit.</param>
    public Fruit(FruitType fruitType, NetWeight netWeight)
    {
        FruitType = fruitType;
        NetWeight = netWeight;
    }

    /// <summary>
    /// Gets the components of the fruit used for equality comparison.
    /// </summary>
    /// <returns>An enumerable collection of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FruitType;
        yield return NetWeight;
    }
}

/// <summary>
/// Represents the types of fruits available.
/// </summary>
public enum FruitType
{
    /// <summary>
    /// Represents a banana.
    /// </summary>
    Banana,

    /// <summary>
    /// Represents an orange.
    /// </summary>
    Orange,

    /// <summary>
    /// Represents an apple.
    /// </summary>
    Apple
}