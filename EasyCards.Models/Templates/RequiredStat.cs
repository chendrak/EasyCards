namespace EasyCards.Models.Templates;

public class RequiredStat
{
    public string Name { get; set; }
    public float Value { get; set; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Value)}: {Value}";
    }
}
