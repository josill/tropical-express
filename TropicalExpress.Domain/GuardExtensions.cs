using Ardalis.GuardClauses;

namespace TropicalExpress.Domain;

/// <summary>
/// Provides extension methods for IGuardClause to enhance input validation.
/// </summary>
public static class GuardExtensions
{
    /// <summary>
    /// Ensures that a specified condition is true. If the condition is false, throws an exception of type TException.
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw if the condition is false. Must be a class that derives from Exception and has a parameterless constructor.</typeparam>
    /// <param name="guardClause">The IGuardClause instance. This parameter is not used in the method body and is only present to allow for extension method syntax.</param>
    /// <param name="condition">The condition to test.</param>
    /// <exception cref="TException">Thrown when the specified condition is false.</exception>
    public static void Requires<TException>(this IGuardClause guardClause, bool condition) where TException : Exception, new()
    {
        if (!condition)
            throw new TException();
    }
}
