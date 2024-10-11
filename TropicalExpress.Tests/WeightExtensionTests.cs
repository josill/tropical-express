using TropicalExpress.Domain;
using Xunit;

namespace TropicalExpress.Tests;

public class WeightExtensionTests
{
    [Fact]
    public void NetWeight_Creation_SetsWeightCorrectly()
    {
        var weight = new Weight(10, Unit.Kilograms);
        var netWeight = new NetWeight(weight);

        Assert.Equal(weight, netWeight.Weight);
    }

    [Fact]
    public void TareWeight_Creation_SetsWeightCorrectly()
    {
        var weight = new Weight(5, Unit.Pounds);
        var tareWeight = new TareWeight(weight);

        Assert.Equal(weight, tareWeight.Weight);
    }

    [Fact]
    public void GrossWeight_Creation_SetsWeightCorrectly()
    {
        var weight = new Weight(1000, Unit.Grams);
        var grossWeight = new GrossWeight(weight);

        Assert.Equal(weight, grossWeight.Weight);
    }

    [Fact]
    public void NetWeight_EqualityComparison_ReturnsTrue_ForEqualWeights()
    {
        var weight1 = new Weight(10, Unit.Kilograms);
        var weight2 = new Weight(10, Unit.Kilograms);
        var netWeight1 = new NetWeight(weight1);
        var netWeight2 = new NetWeight(weight2);

        Assert.Equal(netWeight1, netWeight2);
        Assert.True(netWeight1 == netWeight2);
        Assert.False(netWeight1 != netWeight2);
    }

    [Fact]
    public void TareWeight_EqualityComparison_ReturnsFalse_ForDifferentWeights()
    {
        var weight1 = new Weight(5, Unit.Pounds);
        var weight2 = new Weight(6, Unit.Pounds);
        var tareWeight1 = new TareWeight(weight1);
        var tareWeight2 = new TareWeight(weight2);

        Assert.NotEqual(tareWeight1, tareWeight2);
        Assert.False(tareWeight1 == tareWeight2);
        Assert.True(tareWeight1 != tareWeight2);
    }

    [Fact]
    public void TareWeight_GetHashCode_ReturnsConsistentValue()
    {
        var weight = new Weight(5, Unit.Pounds);
        var tareWeight1 = new TareWeight(weight);
        var tareWeight2 = new TareWeight(weight);

        Assert.Equal(tareWeight1.GetHashCode(), tareWeight2.GetHashCode());
    }

    [Fact]
    public void GrossWeight_GetHashCode_ReturnsDifferentValues_ForDifferentWeights()
    {
        var weight1 = new Weight(1000, Unit.Grams);
        var weight2 = new Weight(2000, Unit.Grams);
        var grossWeight1 = new GrossWeight(weight1);
        var grossWeight2 = new GrossWeight(weight2);

        Assert.NotEqual(grossWeight1.GetHashCode(), grossWeight2.GetHashCode());
    }
}