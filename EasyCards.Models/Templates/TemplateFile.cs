namespace EasyCards.Models.Templates;

public class TemplateFile
{
    public string? ModSource { get; set; }
    public List<CardTemplate> Stats { get; set; } = new();
    public List<EffectCardTemplate> Effects { get; set; } = new();
}
