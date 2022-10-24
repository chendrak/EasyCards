using ModGenesia;

namespace EasyCards.Templates;

public class CardRequirementTemplate
{
    public string Name { get; set; }
    public int Level { get; set; }

    public ModCardRequirement ToModCardRequirement()
    {
        var requirement = new ModCardRequirement();
        
        requirement.cardName = Name;
        requirement.requiredLevel = Level;

        return requirement;
    }
}