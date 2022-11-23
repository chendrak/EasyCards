namespace EasyCards.Models.Templates;

public class EffectCardTemplate : CardTemplate
{
    public List<ConfigurableEffect> Effects { get; set; } = new();
}
