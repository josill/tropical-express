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
        var weight = new Weight(10.5m, WeightUnit.Kilograms);
        Assert.Equal(10.5m, weight.Value);
        Assert.Equal(WeightUnit.Kilograms, weight.WeightUnit);
    }

    [Fact]
    public void Constructor_NegativeValue_ThrowsException()
    {
        Assert.Throws<WeightCannotBeNegativeException>(() => new Weight(-1m, WeightUnit.Grams));
    }

    [Fact]
    public void Constructor_ZeroValue_ThrowsException()
    {
        Assert.Throws<WeightCannotBeZeroException>(() => new Weight(0m, WeightUnit.Pounds));
    }

    [Fact]
    public void Constructor_MoreThanTwoDecimalPlaces_ThrowsException()
    {
        Assert.Throws<MoreThanTwoDecimalPlacesInWeightValueException>(() => new Weight(10.123m, WeightUnit.Kilograms));
    }
    
   [Theory]
    [InlineData(1000, WeightUnit.Grams, WeightUnit.Kilograms, 1)]
    [InlineData(1, WeightUnit.Kilograms, WeightUnit.Grams, 1000)]
    [InlineData(1, WeightUnit.Pounds, WeightUnit.Grams, 453.59)]
    [InlineData(454, WeightUnit.Grams, WeightUnit.Pounds, 1)]
    [InlineData(1000, WeightUnit.Grams, WeightUnit.Pounds, 2.20)]
    [InlineData(1, WeightUnit.Kilograms, WeightUnit.Pounds, 2.20)]
    [InlineData(1, WeightUnit.Pounds, WeightUnit.Kilograms, 0.45)]
    public void ConvertToUnit_ConvertsCorrectly(decimal initialValue, WeightUnit initialWeightUnit, WeightUnit targetWeightUnit, decimal expectedValue)
    {
        var initialWeight = new Weight(initialValue, initialWeightUnit);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(initialWeight, targetWeightUnit);
        _output.WriteLine(initialWeight.Value.ToString());
        _output.WriteLine(convertedWeight.Value.ToString());
        Assert.Equal(targetWeightUnit, convertedWeight.WeightUnit);
        Assert.Equal(expectedValue, convertedWeight.Value);
    }

    [Fact]
    public void ConvertToUnit_SameUnit_ReturnsOriginalWeight()
    {
        var weight = new Weight(10, WeightUnit.Kilograms);
        var convertedWeight = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, WeightUnit.Kilograms);

        Assert.Equal(weight, convertedWeight);
    }

    [Fact]
    public void ConvertToUnit_RoundTrip_ReturnsExactOriginalValue()
    {
        var originalWeight = new Weight(100, WeightUnit.Grams);
        var convertedToKg = Weight.ConvertToUnitWithTwoDecimalPlaces(originalWeight, WeightUnit.Kilograms);
        var convertedBackToGrams = Weight.ConvertToUnitWithTwoDecimalPlaces(convertedToKg, WeightUnit.Grams);

        Assert.Equal(originalWeight.Value, convertedBackToGrams.Value);
    }

    [Fact]
    public void ConvertToUnit_ResultHasExactlyTwoDecimalPlaces_DoesNotThrowException()
    {
        var weight = new Weight(1, WeightUnit.Pounds);
        var result = Weight.ConvertToUnitWithTwoDecimalPlaces(weight, WeightUnit.Kilograms);
        Assert.Equal(0.45m, result.Value);
    }
    
    [Fact]
    public void FromGrams_ValidInput_CreatesWeightInGrams()
    {
        var weight = Weight.FromGrams(500m);
        Assert.Equal(500m, weight.Value);
        Assert.Equal(WeightUnit.Grams, weight.WeightUnit);
    }

    [Fact]
    public void FromKilograms_ValidInput_CreatesWeightInKilograms()
    {
        var weight = Weight.FromKilograms(2.5m);
        Assert.Equal(2.5m, weight.Value);
        Assert.Equal(WeightUnit.Kilograms, weight.WeightUnit);
    }

    [Fact]
    public void FromPounds_ValidInput_CreatesWeightInPounds()
    {
        var weight = Weight.FromPounds(3.3m);
        Assert.Equal(3.3m, weight.Value);
        Assert.Equal(WeightUnit.Pounds, weight.WeightUnit);
    }

    [Fact]
    public void Add_SameUnit_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(3m, WeightUnit.Kilograms);
        var result = weight1.Add(weight2);
        Assert.Equal(5m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void Add_DifferentUnits_ReturnsCorrectSum()
    {
        var weight1 = new Weight(1m, WeightUnit.Kilograms);
        var weight2 = new Weight(500m, WeightUnit.Grams);
        var result = weight1.Add(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void Subtract_SameUnit_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, WeightUnit.Kilograms);
        var weight2 = new Weight(2m, WeightUnit.Kilograms);
        var result = weight1.Subtract(weight2);
        Assert.Equal(3m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void Subtract_DifferentUnits_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(500m, WeightUnit.Grams);
        var result = weight1.Subtract(weight2);
        Assert.Equal(1.5m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void AddOperator_ReturnsCorrectSum()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(3m, WeightUnit.Kilograms);
        var result = weight1 + weight2;
        Assert.Equal(5m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void SubtractOperator_ReturnsCorrectDifference()
    {
        var weight1 = new Weight(5m, WeightUnit.Kilograms);
        var weight2 = new Weight(2m, WeightUnit.Kilograms);
        var result = weight1 - weight2;
        Assert.Equal(3m, result.Value);
        Assert.Equal(WeightUnit.Kilograms, result.WeightUnit);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(2m, WeightUnit.Kilograms);
        Assert.True(weight1.Equals(weight2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(3m, WeightUnit.Kilograms);
        Assert.False(weight1.Equals(weight2));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var weight1 = new Weight(2m, WeightUnit.Kilograms);
        var weight2 = new Weight(2m, WeightUnit.Kilograms);
        Assert.Equal(weight1.GetHashCode(), weight2.GetHashCode());
    }
}