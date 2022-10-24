using System.Collections.Generic;
using EasyCards.Extensions;
using ModGenesia;
using RogueGenesia.Data;

namespace EasyCards.Templates;

public class RequirementTemplate
{
    public List<CardRequirementTemplate> Cards { get; set; }
    public StatRequirementTemplate Stats { get; set; }


    public SCSORequirementList ToRequirementList()
    {
        if (Cards == null && Stats == null)
        {
            return null;
        }

        List<ModCardRequirement> cardRequirements = new();

        if (Cards != null)
        {
            cardRequirements = Cards.ConvertAll(template => template.ToModCardRequirement());
        }

        StatsModifier statRequirements = null;

        var isMinRequirement = true;
        
        if (Stats != null)
        {
            statRequirements = Stats.ToStatsModifier();
            isMinRequirement = Stats.IsMinRequirement();
        }
        
        var requirementList = ModGenesia.ModGenesia.MakeCardRequirement(cardRequirements?.ToIl2CppReferenceArray(), statRequirements, isMinRequirement);
        
        return requirementList;
    }
}