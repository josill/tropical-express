namespace TropicalExpress.Domain;

public class Fruit
{
    public FruitId Id { get; private set; } = new();

    public FruitWeightProfile FruitWeightProfile { get; private set; }
    // public FruitWeightProfile Weights { get; private set; }
    
    private Fruit() {}

    public Fruit(FruitWeightProfile weightProfile)
    {
        FruitWeightProfile = weightProfile;
    }
}

public readonly record struct FruitId(Guid Value)
{
    public FruitId() : this(Guid.NewGuid()) {}
}