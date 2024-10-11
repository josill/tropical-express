using System.ComponentModel.DataAnnotations.Schema;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents the net weight of an object.
/// </summary>
/// <param name="weight">The weight value.</param>
public class NetWeight : ValueObject<NetWeight>
{
    /// <summary>
    /// Gets the weight value.
    /// </summary>
    public  Weight Weight { get; private set; }
    
    private NetWeight() { } // EF Core needs this
    
    public NetWeight(Weight weight)
    {
        Weight = weight;
    }

    /// <summary>
    /// Provides the components of this object used for equality comparison.
    /// </summary>
    /// <returns>An enumeration of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Weight;
    }
}

/// <summary>
/// Represents the tare weight of an object (the weight of an empty container).
/// </summary>
/// <param name="weight">The weight value.</param>
public class TareWeight : ValueObject<TareWeight>
{
    /// <summary>
    /// Gets the weight value.
    /// </summary>
    public Weight Weight { get; private set; }

    private TareWeight() { } // EF Core needs this

    public TareWeight(Weight weight)
    {
        Weight = weight;
    }

    /// <summary>
    /// Provides the components of this object used for equality comparison.
    /// </summary>
    /// <returns>An enumeration of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Weight;
    }
}

/// <summary>
/// Represents the gross weight of an object (total weight including container).
/// </summary>
/// <param name="weight">The weight value.</param>
public class GrossWeight : ValueObject<GrossWeight>
{
    /// <summary>
    /// Gets the weight value.
    /// </summary>
    public Weight Weight { get; private set; }

    private GrossWeight() { } // EF Core needs this

    public GrossWeight(Weight weight)
    {
        Weight = weight;
    }

    /// <summary>
    /// Provides the components of this object used for equality comparison.
    /// </summary>
    /// <returns>An enumeration of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Weight;
    }
}