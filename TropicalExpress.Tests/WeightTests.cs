using TropicalExpress.Domain;
using Xunit;
using Xunit.Abstractions;

namespace TropicalExpress.Tests;

public class WeightTests
{
    private readonly ITestOutputHelper _output;

    public WeightTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public void Constructor_ValidInput_CreatesWeight()
    {
        var weight = new Weight(10.5m, WeightUnit.Kg);
        Assert.Equal(10.5m, weight.Value);
        Assert.Equal(WeightUnit.Kg, weight.Unit);
    }

    [Fact]
    public void Constructor_NegativeValue_ThrowsException()
    {
        Assert.Throws<WeightCannotBeNegativeException>(() => new Weight(-1m, WeightUnit.G));
    }

    [Fact]
    public void Constructor_MoreThanTwoDecimalPlaces_ThrowsException()
    {
        Assert.Throws<MoreThanTwoDecimalPlacesInWeightValueException>(() => new Weight(10.123m, WeightUnit.Kg));
    }
    
   [Theory]
    [InlineData(1000, WeightUnit.G, WeightUnit.Kg, 1)]
    [InlineData(1, WeightUnit.Kg, WeightUnit.G, 1000)]
    [InlineData(1, WeightUnit.Lb, WeightUnit.G, 453.59)]
    [InlineData(454, WeightUnit.G, WeightUnit.Lb, 1)]
    [InlineData(1000, WeightUnit.G, WeightUnit.Lb, 2.20)]
    [InlineData(1, WeightUnit.Kg, WeightUnit.Lb, 2.20)]
    [InlineData(1, WeightUnit.Lb, WeightUnit.Kg, 0.45)]
    public void ConvertToUnit_ConvertsCorrectly(decimal initialValue, WeightUnit initialWeightUnit, WeightUnit targetWeightUnit, decimal expectedValue)
    {
        var initialWeight = new Weight(initialValue, initialWeightUnit);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(initialWeight, targetWeightUnit);
        _output.WriteLine(initialWeight.Value.ToString());
        _output.WriteLine(convertedWeight.Value.ToString());
        Assert.Equal(targetWeightUnit, convertedWeight.Unit);
        Assert.Equal(expectedValue, convertedWeight.Value);
    }

    [Fact]
    public void ConvertToUnit_SameUnit_ReturnsOriginalWeight()
    {
        var weight = new Weight(10, WeightUnit.Kg);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, WeightUnit.Kg);

        Assert.Equal(weight, convertedWeight);
    }

    [Fact]
    public void ConvertToUnit_RoundTrip_ReturnsExactOriginalValue()
    {
        var originalWeight = new Weight(100, WeightUnit.G);
        var convertedToKg = Weight.ConvertToUnitWithTwoDecimalPlaces(originalWeight, WeightUnit.Kg);
        var convertedBackToGrams = Weight.ConvertToUnitWithTwoDecimalPlaces(convertedToKg, WeightUnit.G);

        Assert.Equal(originalWeight.Value, convertedBackToGrams.Value);
    }

    [Fact]
    public void ConvertToUnit_ResultHasExactlyTwoDecimalPlaces_DoesNotThrowException()
    {
        var weight = new Weight(1, WeightUnit.Lb);
        var result = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, WeightUnit.Kg);
        Assert.Equal(0.45m, result.Value);
    }
    
    [Fact]
    public void FromGrams_ValidInput_CreatesWeightInGrams()
    {
        var weight = Weight.FromGrams(500m);
        Assert.Equal(500m, weight.Value);
        Assert.Equal(WeightUnit.G, weight.Unit);
    }

    [Fact]
    public void FromKilograms_ValidInput_CreatesWeightInKilograms()
    {
        var weight = Weight.FromKilograms(2.5m);
        Assert.Equal(2.5m, weight.Value);
        Assert.Equal(WeightUnit.Kg, weight.Unit);
    }

    [Fact]
    public void FromPounds_ValidInput_CreatesWeightInPounds()
    {
        var weight = Weight.FromPounds(3.3m);
        Assert.Equal(3.3m, weight.Value);
        Assert.Equal(WeightUnit.Lb, weight.Unit);
    }

    [Fact]
    public void Add_SameUnit_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(3m, WeightUnit.Kg);
        var result = weight1.Add(weight2);
        Assert.Equal(5m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void Add_DifferentUnits_ReturnsCorrectSum()
    {
        var weight1 = new Weight(1m, WeightUnit.Kg);
        var weight2 = new Weight(500m, WeightUnit.G);
        var result = weight1.Add(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void Subtract_SameUnit_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, WeightUnit.Kg);
        var weight2 = new Weight(2m, WeightUnit.Kg);
        var result = weight1.Subtract(weight2);
        Assert.Equal(3m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void Subtract_DifferentUnits_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(500m, WeightUnit.G);
        var result = weight1.Subtract(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void AddOperator_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(3m, WeightUnit.Kg);
        var result = weight1 + weight2;
        Assert.Equal(5m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void SubtractOperator_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, WeightUnit.Kg);
        var weight2 = new Weight(2m, WeightUnit.Kg);
        var result = weight1 - weight2;
        Assert.Equal(3m, result.Value);
        Assert.Equal(WeightUnit.Kg, result.Unit);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(2m, WeightUnit.Kg);
        Assert.True(weight1.Equals(weight2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(3m, WeightUnit.Kg);
        Assert.False(weight1.Equals(weight2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var weight1 = new Weight(2m, WeightUnit.Kg);
        var weight2 = new Weight(2m, WeightUnit.Kg);
        Assert.Equal(weight1.GetHashCode(), weight2.GetHashCode());
    }
}