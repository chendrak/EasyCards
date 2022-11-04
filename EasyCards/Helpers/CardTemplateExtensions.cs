using System;
using System.Collections.Generic;
using EasyCards.Extensions;
using EasyCards.Models.Templates;
using EasyCards.Models.Templates.Generated;
using ModGenesia;
using RogueGenesia.Data;

namespace EasyCards.Helpers;

public static class CardTemplateExtensions
{
    public static StatsModifier ToStatsModifier(this StatRequirementTemplate template)
    {
        if (Enum.TryParse<StatRequirementType>(template.RequirementType, true, out _))
        {
            var statsModifier = new StatsModifier();

            var statModifiers = new List<StatModifier>();
            foreach (var statRequirement in template.StatRequirements)
            {
                var convertedRequirement = statRequirement.ToStatModifier();
                if (convertedRequirement != null)
                {
                    statModifiers.Add(convertedRequirement);
                }
            }
            foreach (var statModifier in statModifiers)
            {
                statsModifier.ModifiersList.Add(statModifier);
            }

            return statsModifier;
        }

        EasyCards.Instance.Log.LogWarning($"{template.RequirementType} is not a valid requirement type! Valid options are: [{EnumToStringHelper.NamesToString(typeof(StatRequirementType))}]");
        return null;
    }
    public static StatModifier ToStatModifier(this StatRequirement template)
    {
        if (Enum.TryParse<StatsType>(template.Name, true, out _))
        {
            var statModifier = new StatModifier();

            var singularModifier = new SingularModifier();

            // This is just a placeholder, as it is not used in the comparison
            singularModifier.ModifierType = ModifierType.Additional;
            singularModifier.Value = template.Value;

            statModifier.Key = template.Name;
            statModifier.Value = singularModifier;

            return statModifier;
        }

        EasyCards.Instance.Log.LogWarning($"{template.Name} is not a valid stat name! Valid options are: [{EnumToStringHelper.NamesToString(typeof(StatsType))}]");
        return null;
    }

    public static SCSORequirementList ToRequirementList(this RequirementTemplate template)
    {

        List<ModCardRequirement> cardRequirements = new();

        if (template.Cards != null)
        {
            cardRequirements = template.Cards.ConvertAll(template => template.ToModCardRequirement());
        }

        StatsModifier statRequirements = null;

        var isMinRequirement = true;

        if (template.Stats != null)
        {
            statRequirements = template.Stats.ToStatsModifier();
            isMinRequirement = template.Stats.IsMinRequirement();
        }

        var requirementList = ModGenesia.ModGenesia.MakeCardRequirement(cardRequirements.ToIl2CppReferenceArray(), statRequirements, isMinRequirement);

        return requirementList;
    }
    public static ModCardRequirement ToModCardRequirement(this CardRequirementTemplate template)
    {
        var requirement = new ModCardRequirement()
        {
            cardName = template.Name,
            requiredLevel = template.Level
        };

        return requirement;
    }
    public static StatsModifier CreateStatsModifier(this CardTemplate cardTemplate)
    {
        var statsMod = new StatsModifier();

        foreach (var modifier in cardTemplate.Modifiers)
        {
            statsMod.ModifiersList.Add(modifier.CreateStatModifier());
        }

        return statsMod;
    }

    /// <summary>
    /// Converts this ModifierTemplate into something the game can understand.
    /// </summary>
    /// <returns>A <c>StatModifier</c> based on this <c>ModifierTemplate</c></returns>
    public static StatModifier CreateStatModifier(this ModifierTemplate modifierTemplate)
    {
        var singMod = new SingularModifier
        {
            Value = modifierTemplate.ModifierValue,
            ModifierType = modifierTemplate.ModifierType.CastTo<ModifierType>()
        };

        var statModifier = new StatModifier
        {
            Value = singMod,
            Key = modifierTemplate.Stat.ToString()
        };

        return statModifier;
    }
}
