using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;

namespace TropicalExpress.Domain;

/// <summary>
/// Represents a weight value with a specific unit of measurement.
/// </summary>
public class Weight : ValueObject<Weight>
{
    /// <summary>
    /// Gets the numeric value of the weight.
    /// </summary>
    /// <remarks>
    /// This value is always positive, non-zero, and has no more than two decimal places.
    /// </remarks>
    public decimal Value { get; }

    /// <summary>
    /// Gets the unit of measurement for the weight.
    /// </summary>
    public WeightUnit Unit { get; }

    private Weight() {} // EF Core needs this

    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> class.
    /// </summary>
    /// <param name="value">The numeric value of the weight.</param>
    /// <param name="unit">The unit of measurement for the weight.</param>
    /// <exception cref="MoreThanTwoDecimalPlacesInWeightValueException">Thrown when the value has more than two decimal places.</exception>
    /// <exception cref="WeightCannotBeNegativeException">Thrown when the value is negative.</exception>
    /// <exception cref="WeightCannotBeZeroException">Thrown when the value is zero.</exception>
    public Weight(decimal value, WeightUnit unit)
    {
        Validate(value);

        Value = value;
        Unit = unit;
    }

    /// <summary>
    /// Validates the weight value using guard clauses.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <exception cref="MoreThanTwoDecimalPlacesInWeightValueException">Thrown when the value has more than two decimal places.</exception>
    /// <exception cref="WeightCannotBeNegativeException">Thrown when the value is negative.</exception>
    private static void Validate(decimal value)
    {
        Guard.Against.Requires<MoreThanTwoDecimalPlacesInWeightValueException>(value % 0.01m == 0);
        Guard.Against.Requires<WeightCannotBeNegativeException>(value >= 0);
    }
    
    /// <summary>
    /// Gets the components used for equality comparison.
    /// </summary>
    /// <returns>A collection of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Unit;
    }

    /// <summary>
    /// Creates a new Weight instance with the specified value in grams.
    /// </summary>
    /// <param name="value">The weight value in grams.</param>
    /// <returns>A new Weight instance.</returns>
    public static Weight FromGrams(decimal value) => new Weight(value, WeightUnit.G); 
    
    /// <summary>
    /// Creates a new Weight instance with the specified value in kilograms.
    /// </summary>
    /// <param name="value">The weight value in kilograms.</param>
    /// <returns>A new Weight instance.</returns>
    public static Weight FromKilograms(decimal value) => new Weight(value, WeightUnit.Kg); 
    
    /// <summary>
    /// Creates a new Weight instance with the specified value in pounds.
    /// </summary>
    /// <param name="value">The weight value in pounds.</param>
    /// <returns>A new Weight instance.</returns>
    public static Weight FromPounds(decimal value) => new Weight(value, WeightUnit.Lb); 

    /// <summary>
    /// Adds another Weight to this Weight.
    /// </summary>
    /// <param name="weight">The Weight to add.</param>
    /// <returns>A new Weight instance representing the sum.</returns>
    public Weight Add(Weight weight)
    {
        var convertedWeight = ConvertToUnitWithTwoDecimalPlaces(weight, this.Unit);
        return new Weight(this.Value + convertedWeight.Value, this.Unit);
    }

    /// <summary>
    /// Subtracts another Weight from this Weight.
    /// </summary>
    /// <param name="weight">The Weight to subtract.</param>
    /// <returns>A new Weight instance representing the difference.</returns>
    public Weight Subtract(Weight weight)
    {
        var convertedWeight = ConvertToUnitWithTwoDecimalPlaces(weight, this.Unit);
        return new Weight(this.Value - convertedWeight.Value, this.Unit);
    }

    /// <summary>
    /// Adds two Weight instances.
    /// </summary>
    /// <param name="left">The first Weight.</param>
    /// <param name="right">The second Weight.</param>
    /// <returns>A new Weight instance representing the sum.</returns>
    public static Weight operator +(Weight left, Weight right)
    {
        return left.Add(right);
    }

    /// <summary>
    /// Subtracts one Weight instance from another.
    /// </summary>
    /// <param name="left">The Weight to subtract from.</param>
    /// <param name="right">The Weight to subtract.</param>
    /// <returns>A new Weight instance representing the difference.</returns>
    public static Weight operator -(Weight left, Weight right)
    {
        return left.Subtract(right);
    }
    
    /// <summary>
    /// Converts a Weight object to a specified unit of measurement.
    /// </summary>
    /// <param name="weight">The Weight object to convert.</param>
    /// <param name="targetWeightUnit">The target Unit to convert the weight to.</param>
    /// <returns>A new Weight object with the value converted to the target unit.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. If the current unit is the same as the target unit, it returns the original weight.
    /// 2. Converts the weight to grams as an intermediate step.
    /// 3. Converts from grams to the target unit.
    /// 4. Rounds the result to two decimal places.
    /// 5. Checks if the rounded result has more than two decimal places and throws an exception if it does.
    /// </remarks>
    /// <exception cref="MoreThanTwoDecimalPlacesInWeightValueException">
    /// Thrown when the conversion result, after rounding to two decimal places, still has more than two decimal places.
    /// This can happen due to floating-point arithmetic limitations.
    /// </exception>
    internal static Weight ConvertToUnitWithTwoDecimalPlaces(Weight weight, WeightUnit targetWeightUnit)
    {
        if (weight.Unit == targetWeightUnit)
            return weight;

        var convertedValue = weight.Value;

        // Convert to grams first
        switch (weight.Unit)
        {
            case WeightUnit.Kg:
                convertedValue *= 1000;
                break;
            case WeightUnit.Lb:
                convertedValue *= 453.592m;
                break;
        }

        // Then convert to target unit
        switch (targetWeightUnit)
        {
            case WeightUnit.Kg:
                convertedValue /= 1000;
                break;
            case WeightUnit.Lb:
                convertedValue /= 453.592m;
                break;
        }

        // Round to two decimal places
        convertedValue = Math.Round(convertedValue, 2, MidpointRounding.AwayFromZero);

        return new Weight(convertedValue, targetWeightUnit);
    }

    public static string WeightToString(Weight weight)
    {
        return $"{weight.Value} {weight.Unit}";
    }

    public static Weight StringToWeight(string weight)  {
        var parts = weight.Trim().Split(' ');
        return new Weight(decimal.Parse(parts[0]), Enum.Parse<WeightUnit>(parts[1]));
    }
}

/// <summary>
/// Represents the units of measurement for weight.
/// </summary>
public enum WeightUnit
{
    /// <summary>
    /// Weight in grams.
    /// </summary>
    G,

    /// <summary>
    /// Weight in kilograms.
    /// </summary>
    Kg,

    /// <summary>
    /// Weight in pounds.
    /// </summary>
    Lb
}

/// <summary>
/// Exception thrown when a weight value has more than two decimal places.
/// </summary>
public class MoreThanTwoDecimalPlacesInWeightValueException()
    : Exception("Weight value cannot have more than two decimal places.");

/// <summary>
/// Exception thrown when a weight value is negative.
/// </summary>
public class WeightCannotBeNegativeException() : Exception("Weight cannot be a negative value.");