using System.Collections.Generic;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents the packaging used for fruit in the Tropical Express system.
/// </summary>
/// <param name="tareWeight">The tare weight of the packaging.</param>
public class FruitPackaging(TareWeight tareWeight) : ValueObject<FruitPackaging>
{
    /// <summary>
    /// Gets the tare weight of the packaging.
    /// </summary>
    /// <remarks>
    /// The tare weight is the weight of the empty packaging, which is subtracted from the gross weight to determine the net weight of the fruit.
    /// </remarks>
    public readonly TareWeight TareWeight = tareWeight;

    /// <summary>
    /// Provides the components of the <see cref="FruitPackaging"/> that are used to determine equality.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TareWeight;
    }
}