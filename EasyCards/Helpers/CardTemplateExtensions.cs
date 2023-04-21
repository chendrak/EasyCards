using System;
using System.Collections.Generic;
using EasyCards.Extensions;
using EasyCards.Models.Templates;
using ModGenesia;
using RogueGenesia.Data;

namespace EasyCards.Helpers;

using Common.Logging;

public static class CardTemplateExtensions
{
    public static List<StatRequirement>? ToStatRequirementList(this StatRequirementTemplate template, bool isMinRequirement)
    {
        if (Enum.TryParse<StatRequirementType>(template.RequirementType, true, out _))
        {
            var statsRequirements = new List<StatRequirement>();

            foreach (var requiredStat in template.StatRequirements)
            {
                var convertedRequirement = requiredStat.ToStatRequirement(isMinRequirement);
                if (convertedRequirement != null)
                {
                    statsRequirements.Add(convertedRequirement);
                }
            }
            return statsRequirements;
        }

        Log.Warn($"{template.RequirementType} is not a valid requirement type! Valid options are: [{EnumToStringHelper.NamesToString(typeof(StatRequirementType))}]");
        return null;
    }

    public static StatRequirement? ToStatRequirement(this RequiredStat template, bool isMinRequirement)
    {
        if (Enum.TryParse<StatsType>(template.Name, true, out _))
        {
            var statReq = new StatRequirement
            {
                Key = template.Name,
                Value = template.Value,
                ComparisionType = isMinRequirement
                    ? StatRequirement.EComparisionType.GreaterOrEqual
                    : StatRequirement.EComparisionType.LesserOrEqual
            };

            return statReq;
        }

        Log.Warn($"{template.Name} is not a valid stat name! Valid options are: [{EnumToStringHelper.NamesToString(typeof(StatsType))}]");
        return null;
    }

    public static SCSORequirementList ToRequirementList(this RequirementTemplate template)
    {

        List<ModCardRequirement> cardRequirements = new();

        if (template.Cards != null)
        {
            cardRequirements = template.Cards.ConvertAll(template => template.ToModCardRequirement());
        }

        List<StatRequirement> statRequirements = new();

        var isMinRequirement = true;

        if (template.Stats != null)
        {
            statRequirements = template.Stats.ToStatRequirementList(template.Stats.IsMinRequirement());
        }

        var requirementList = CardAPI.MakeCardRequirement(cardRequirements.ToArray(), statRequirements.ToArray());
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

        // Add empty modifier so the game doesn't complain
        if (cardTemplate.Modifiers.Count == 0)
        {
            var singMod = new SingularModifier { Value = 0.0f, ModifierType = ModifierType.Additional };
            var statModifier = new StatModifier { Value = singMod, Key = StatsType.Damage.ToString() };
            statsMod.ModifiersList.Add(statModifier);
        }
        else
        {
            foreach (var modifier in cardTemplate.Modifiers)
            {
                statsMod.ModifiersList.Add(modifier.CreateStatModifier());
            }
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
            ModifierType = modifierTemplate.ModifierType
        };

        var statModifier = new StatModifier
        {
            Value = singMod,
            Key = modifierTemplate.Stat.ToString()
        };

        return statModifier;
    }
}
