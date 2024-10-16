using System.ComponentModel.DataAnnotations.Schema;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents the net weight of an object.
/// </summary>
/// <param name="weight">The weight value.</param>
public class NetWeight : Weight
{
    // private NetWeight() : base() { } // EF Core needs this
    
    public NetWeight(Weight weight) : base(weight.Value, weight.Unit) {}

    public static string NetWeightToString(NetWeight netWeight)
    {
        return $"NetWeight: {Weight.WeightToString(netWeight)}";
    }
    
    public static NetWeight StringToNetWeight(string netWeight)
    {
        var parts = netWeight.Split("NetWeight:");
        return new NetWeight(Weight.StringToWeight(parts[1].Trim()));
    }
}

/// <summary>
/// Represents the tare weight of an object (the weight of an empty container).
/// </summary>
/// <param name="weight">The weight value.</param>
public class TareWeight : Weight
{
    // private TareWeight() { } // EF Core needs this

    public TareWeight(Weight weight) : base(weight.Value, weight.Unit) {}
    
    public static string TareWeightToString(TareWeight tareWeight)
    {
        return $"TareWeight: {Weight.WeightToString(tareWeight)}";
    }
    
    public static TareWeight StringToTareWeight(string tareWeight)
    {
        var parts = tareWeight.Split("TareWeight: ");
        return new TareWeight(Weight.StringToWeight(parts[1].Trim()));
    }
}

/// <summary>
/// Represents the gross weight of an object (total weight including container).
/// </summary>
/// <param name="weight">The weight value.</param>
public class GrossWeight : Weight
{
    // private GrossWeight() { } // EF Core needs this

    public GrossWeight(Weight weight) : base(weight.Value, weight.Unit) {}
    
    public static string GrossWeightToString(GrossWeight grossWeight)
    {
        return $"GrossWeight: {Weight.WeightToString(grossWeight)}";
    }
    
    public static GrossWeight StringToGrossWeight(string grossWeight)
    {
        var parts = grossWeight.Split("GrossWeight: ");
        return new GrossWeight(Weight.StringToWeight(parts[1].Trim()));
    }
}