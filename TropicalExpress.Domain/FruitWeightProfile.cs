using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents a profile of weights associated with fruit, including net, tare, and gross weights.
/// </summary>
public class FruitWeightProfile : ValueObject<FruitWeightProfile>
{
    /// <summary>
    /// Gets the net weight of the fruit.
    /// </summary>
    public readonly NetWeight NetWeight;
    
    /// <summary>
    /// Gets the tare weight (weight of the container).
    /// </summary>
    public readonly TareWeight TareWeight;
    
    /// <summary>
    /// Gets the gross weight (total weight including container).
    /// </summary>
    public readonly GrossWeight GrossWeight;
    
    private FruitWeightProfile() { } // EF Core needs this

    /// <summary>
    /// Initializes a new instance of the <see cref="FruitWeightProfile"/> class.
    /// </summary>
    /// <param name="netWeight">The net weight of the fruit.</param>
    /// <param name="tareWeight">The tare weight of the container.</param>
    /// <exception cref="InconsistentWeightUnitsException">Thrown when net weight and tare weight use different units.</exception>
    public FruitWeightProfile(NetWeight netWeight, TareWeight tareWeight)
    {
        Validate(netWeight, tareWeight);
        
        NetWeight = netWeight;
        TareWeight = tareWeight;
        GrossWeight = new GrossWeight(netWeight.Weight + tareWeight.Weight);
    }
    
    /// <summary>
    /// Validates that the net weight and tare weight use the same units.
    /// </summary>
    /// <param name="netWeight">The net weight to validate.</param>
    /// <param name="tareWeight">The tare weight to validate.</param>
    /// <exception cref="InconsistentWeightUnitsException">Thrown when net weight and tare weight use different units.</exception>
    private static void Validate(NetWeight netWeight, TareWeight tareWeight)
    {
        Guard.Against.Requires<InconsistentWeightUnitsException>(netWeight.Weight.Unit != tareWeight.Weight.Unit);
    }

    /// <summary>
    /// Provides the components of this object used for equality comparison.
    /// </summary>
    /// <returns>An enumeration of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NetWeight;
        yield return GrossWeight;
        yield return TareWeight;
    }
}

/// <summary>
/// Exception thrown when weight units are inconsistent within a weight profile.
/// </summary>
public class InconsistentWeightUnitsException() : Exception("All weights must use the same unit.");