namespace EasyCards.Models.Templates
{
    using RogueGenesia.Data;

    public class CardTemplate
    {
        // FIXME: Find a way to make the schema-file work again
        // private const string SchemaFileName = "schema.json";
        // [JsonPropertyName("$schema")]
        // public string Schema
        // {
        //     get => SchemaFileName;
        //     // ReSharper disable once ValueParameterNotUsed - This is only here to appease the schema generation gods.
        //     set { }
        // }

        public string Name { get; set; }
        public string? TexturePath { get; set; }

        public CardRarity Rarity { get; set; }

        public List<CardTag> CardTags { get; set; } = new();
        public float DropWeight { get; set; }
        public float LevelUpWeight { get; set; }
        public int MaxLevel { get; set; }
        public List<ModifierTemplate> Modifiers { get; set; } = new();
        public Dictionary<string, string> NameLocalization { get; set; } = new();
        public Dictionary<string, string> DescriptionLocalization { get; set; } = new();

        public List<string> BanishesCardsByName { get; set; } = new();
        public List<string> BanishesCardsWithStatsOfType { get; set; } = new();
        public List<string> RemovesCards { get; set; } = new();
        public RequirementTemplate? RequiresAny { get; set; }
        public RequirementTemplate? RequiresAll { get; set; }
        public DisabledInMode? DisabledInMode { get; set; }

        public List<ConfigurableEffect> Effects { get; set; } = new();

        public override string ToString()
        {
            return
                $"{nameof(Name)}: {Name}, {nameof(TexturePath)}: {TexturePath}, {nameof(Rarity)}: {Rarity}, {nameof(this.CardTags)}: {this.CardTags}, {nameof(DropWeight)}: {DropWeight}, {nameof(LevelUpWeight)}: {LevelUpWeight}, {nameof(MaxLevel)}: {MaxLevel}, {nameof(Modifiers)}: {Modifiers}";
        }
    }
}
