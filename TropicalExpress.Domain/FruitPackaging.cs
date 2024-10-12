namespace TropicalExpress.Domain;

public class FruitPackaging : ValueObject<FruitPackaging>
{
    public readonly TareWeight TareWeight;
    
    private FruitPackaging() {}

    public FruitPackaging(TareWeight tareWeight)
    {
        TareWeight = tareWeight;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TareWeight;
    }
}