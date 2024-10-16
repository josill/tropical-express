namespace TropicalExpress.Domain;

/// <summary>
/// Represents the weight data of a fruit, including net weight, tare weight, and gross weight.
/// </summary>
public class WeightData : ValueObject<WeightData>
{
    /// <summary>
    /// Gets the net weight of the fruit.
    /// </summary>
    public NetWeight NetWeight {get; }

    /// <summary>
    /// Gets the tare weight (weight of the packaging).
    /// </summary>
    public TareWeight TareWeight {get; }

    /// <summary>
    /// Gets the gross weight (total weight including packaging).
    /// </summary>
    public GrossWeight GrossWeight {get; }
    
    /// <summary>
    /// Private constructor for EF Core.
    /// </summary>
    private WeightData() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="WeightData"/> class with net weight and tare weight.
    /// </summary>
    /// <param name="netWeight">The net weight of the fruit.</param>
    /// <param name="tareWeight">The tare weight (weight of the packaging).</param>
    public WeightData(NetWeight netWeight, TareWeight tareWeight)
    {
        NetWeight = netWeight;
        TareWeight = tareWeight;
        GrossWeight = new GrossWeight(netWeight + tareWeight); // TODO: Convert Units
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeightData"/> class with only net weight.
    /// </summary>
    /// <param name="netWeight">The net weight of the fruit.</param>
    public WeightData(NetWeight netWeight)
    {
        NetWeight = netWeight;
        TareWeight = new TareWeight(new Weight(0, WeightUnit.Kg)); // TODO: make empty weight contructor or change logic
        GrossWeight = new GrossWeight(netWeight);
    }
    
    /// <summary>
    /// Gets the components of the weight data used for equality comparison.
    /// </summary>
    /// <returns>An enumerable collection of objects representing the equality components.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NetWeight;
        yield return TareWeight;
        yield return GrossWeight;
    }

    /// <summary>
    /// Converts a WeightData object to its string representation.
    /// </summary>
    /// <param name="weightData">The WeightData object to convert.</param>
    /// <returns>A string representation of the WeightData object.</returns>
    public static string WeighDataToString(WeightData weightData)
    {
        return $"{NetWeight.NetWeightToString(weightData.NetWeight)}\n" +
               $"{TareWeight.TareWeightToString(weightData.TareWeight)}\n" +
               $"{GrossWeight.GrossWeightToString(weightData.GrossWeight)}"; 
        // TODO:  we don't need the gross weight
    }

    /// <summary>
    /// Converts a string representation back to a WeightData object.
    /// </summary>
    /// <param name="weightData">The string representation of WeightData.</param>
    /// <returns>A WeightData object parsed from the string.</returns>
    public static WeightData StringToWeightData(string weightData)
    {
        var parts = weightData.Split("\n");
        return new WeightData(NetWeight.StringToNetWeight(parts[0]), TareWeight.StringToTareWeight(parts[1]));
    }
}