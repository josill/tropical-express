using TropicalExpress.Domain;
using Xunit;

namespace TropicalExpress.Tests;

// Concrete implementation of ValueObject<T> for testing
public class TestValueObject(int value1, string value2) : ValueObject<TestValueObject>
{
    private int Value1 { get; } = value1;
    private string Value2 { get; } = value2;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value1;
        yield return Value2;
    }
}

public class ValueObjectTests
{
    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(1, "test");

        Assert.True(obj1.Equals(obj2));
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(2, "test");

        Assert.False(obj1.Equals(obj2));
    }

    [Fact]
    public void Equals_NullObject_ReturnsFalse()
    {
        var obj = new TestValueObject(1, "test");

        Assert.False(obj.Equals(null));
    }

    [Fact]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var obj = new TestValueObject(1, "test");
        var differentObj = new object();

        Assert.False(obj.Equals(differentObj));
    }

    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(1, "test");

        Assert.Equal(obj1.GetHashCode(), obj2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCodes()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(2, "test");

        Assert.NotEqual(obj1.GetHashCode(), obj2.GetHashCode());
    }

    [Fact]
    public void EqualityOperator_SameValues_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(1, "test");

        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void EqualityOperator_DifferentValues_ReturnsFalse()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(2, "test");

        Assert.False(obj1 == obj2);
    }

    [Fact]
    public void InequalityOperator_SameValues_ReturnsFalse()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(1, "test");

        Assert.False(obj1 != obj2);
    }

    [Fact]
    public void InequalityOperator_DifferentValues_ReturnsTrue()
    {
        var obj1 = new TestValueObject(1, "test");
        var obj2 = new TestValueObject(2, "test");

        Assert.True(obj1 != obj2);
    }

    [Fact]
    public void EqualityOperator_NullObjects_ReturnsTrue()
    {
        TestValueObject obj1 = null;
        TestValueObject obj2 = null;

        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void EqualityOperator_OneNullObject_ReturnsFalse()
    {
        TestValueObject obj1 = null;
        var obj2 = new TestValueObject(1, "test");

        Assert.False(obj1 == obj2);
        Assert.False(obj2 == obj1);
    }

    [Fact]
    public void InequalityOperator_NullObjects_ReturnsFalse()
    {
        TestValueObject obj1 = null;
        TestValueObject obj2 = null;

        Assert.False(obj1 != obj2);
    }

    [Fact]
    public void InequalityOperator_OneNullObject_ReturnsTrue()
    {
        TestValueObject obj1 = null;
        var obj2 = new TestValueObject(1, "test");

        Assert.True(obj1 != obj2);
        Assert.True(obj2 != obj1);
    }
}
