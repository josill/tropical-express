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

    /// <summary>
    /// Gets the tare weight of the packaging, if any.
    /// </summary>
    public TareWeight? TareWeight { get; }

    /// <summary>
    /// Gets the gross weight of the fruit, including packaging if any.
    /// </summary>
    public GrossWeight GrossWeight { get; }

    /// <summary>
    /// Gets the packaging of the fruit, if any.
    /// </summary>
    public FruitPackaging? FruitPackaging { get; }
    
    private Fruit() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="Fruit"/> class without packaging.
    /// </summary>
    /// <param name="fruitType">The type of fruit.</param>
    /// <param name="netWeight">The net weight of the fruit.</param>
    public Fruit(FruitType fruitType, NetWeight netWeight)
    {
        FruitType = fruitType;
        NetWeight = netWeight;
        TareWeight = null;
        FruitPackaging = null;
        GrossWeight = new GrossWeight(netWeight.Weight);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Fruit"/> class with packaging.
    /// </summary>
    /// <param name="fruitType">The type of fruit.</param>
    /// <param name="netWeight">The net weight of the fruit.</param>
    /// <param name="fruitPackaging">The packaging of the fruit.</param>
    public Fruit(FruitType fruitType, NetWeight netWeight, FruitPackaging fruitPackaging)
    {
        FruitType = fruitType;
        NetWeight = netWeight;
        FruitPackaging = fruitPackaging;
        TareWeight = fruitPackaging.TareWeight;
        GrossWeight = new GrossWeight(netWeight.Weight + TareWeight.Weight);
    }

    /// <summary>
    /// Gets the components of the fruit used for equality comparison.
    /// </summary>
    /// <returns>An enumerable collection of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FruitType;
        yield return NetWeight;
        yield return GrossWeight;
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