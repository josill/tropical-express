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
        var weight = new Weight(10.5m, Unit.Kilograms);
        Assert.Equal(10.5m, weight.Value);
        Assert.Equal(Unit.Kilograms, weight.Unit);
    }

    [Fact]
    public void Constructor_NegativeValue_ThrowsException()
    {
        Assert.Throws<WeightCannotBeNegativeException>(() => new Weight(-1m, Unit.Grams));
    }

    [Fact]
    public void Constructor_ZeroValue_ThrowsException()
    {
        Assert.Throws<WeightCannotBeZeroException>(() => new Weight(0m, Unit.Pounds));
    }

    [Fact]
    public void Constructor_MoreThanTwoDecimalPlaces_ThrowsException()
    {
        Assert.Throws<MoreThanTwoDecimalPlacesInWeightValueException>(() => new Weight(10.123m, Unit.Kilograms));
    }
    
   [Theory]
    [InlineData(1000, Unit.Grams, Unit.Kilograms, 1)]
    [InlineData(1, Unit.Kilograms, Unit.Grams, 1000)]
    [InlineData(1, Unit.Pounds, Unit.Grams, 453.59)]
    [InlineData(454, Unit.Grams, Unit.Pounds, 1)]
    [InlineData(1000, Unit.Grams, Unit.Pounds, 2.20)]
    [InlineData(1, Unit.Kilograms, Unit.Pounds, 2.20)]
    [InlineData(1, Unit.Pounds, Unit.Kilograms, 0.45)]
    public void ConvertToUnit_ConvertsCorrectly(decimal initialValue, Unit initialUnit, Unit targetUnit, decimal expectedValue)
    {
        var initialWeight = new Weight(initialValue, initialUnit);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(initialWeight, targetUnit);
        _output.WriteLine(initialWeight.Value.ToString());
        _output.WriteLine(convertedWeight.Value.ToString());
        Assert.Equal(targetUnit, convertedWeight.Unit);
        Assert.Equal(expectedValue, convertedWeight.Value);
    }

    [Fact]
    public void ConvertToUnit_SameUnit_ReturnsOriginalWeight()
    {
        var weight = new Weight(10, Unit.Kilograms);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, Unit.Kilograms);

        Assert.Equal(weight, convertedWeight);
    }

    [Fact]
    public void ConvertToUnit_RoundTrip_ReturnsExactOriginalValue()
    {
        var originalWeight = new Weight(100, Unit.Grams);
        var convertedToKg = Weight.ConvertToUnitWithTwoDecimalPlaces(originalWeight, Unit.Kilograms);
        var convertedBackToGrams = Weight.ConvertToUnitWithTwoDecimalPlaces(convertedToKg, Unit.Grams);

        Assert.Equal(originalWeight.Value, convertedBackToGrams.Value);
    }

    [Fact]
    public void ConvertToUnit_ResultHasExactlyTwoDecimalPlaces_DoesNotThrowException()
    {
        var weight = new Weight(1, Unit.Pounds);
        var result = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, Unit.Kilograms);
        Assert.Equal(0.45m, result.Value);
    }
    
    [Fact]
    public void FromGrams_ValidInput_CreatesWeightInGrams()
    {
        var weight = Weight.FromGrams(500m);
        Assert.Equal(500m, weight.Value);
        Assert.Equal(Unit.Grams, weight.Unit);
    }

    [Fact]
    public void FromKilograms_ValidInput_CreatesWeightInKilograms()
    {
        var weight = Weight.FromKilograms(2.5m);
        Assert.Equal(2.5m, weight.Value);
        Assert.Equal(Unit.Kilograms, weight.Unit);
    }

    [Fact]
    public void FromPounds_ValidInput_CreatesWeightInPounds()
    {
        var weight = Weight.FromPounds(3.3m);
        Assert.Equal(3.3m, weight.Value);
        Assert.Equal(Unit.Pounds, weight.Unit);
    }

    [Fact]
    public void Add_SameUnit_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(3m, Unit.Kilograms);
        var result = weight1.Add(weight2);
        Assert.Equal(5m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void Add_DifferentUnits_ReturnsCorrectSum()
    {
        var weight1 = new Weight(1m, Unit.Kilograms);
        var weight2 = new Weight(500m, Unit.Grams);
        var result = weight1.Add(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void Subtract_SameUnit_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, Unit.Kilograms);
        var weight2 = new Weight(2m, Unit.Kilograms);
        var result = weight1.Subtract(weight2);
        Assert.Equal(3m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void Subtract_DifferentUnits_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(500m, Unit.Grams);
        var result = weight1.Subtract(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void AddOperator_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(3m, Unit.Kilograms);
        var result = weight1 + weight2;
        Assert.Equal(5m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void SubtractOperator_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, Unit.Kilograms);
        var weight2 = new Weight(2m, Unit.Kilograms);
        var result = weight1 - weight2;
        Assert.Equal(3m, result.Value);
        Assert.Equal(Unit.Kilograms, result.Unit);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(2m, Unit.Kilograms);
        Assert.True(weight1.Equals(weight2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(3m, Unit.Kilograms);
        Assert.False(weight1.Equals(weight2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var weight1 = new Weight(2m, Unit.Kilograms);
        var weight2 = new Weight(2m, Unit.Kilograms);
        Assert.Equal(weight1.GetHashCode(), weight2.GetHashCode());
    }
}