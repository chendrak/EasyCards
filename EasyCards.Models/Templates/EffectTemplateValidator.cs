namespace EasyCards.Models.Templates;

public static class EffectTemplateValidator
{
    private static Dictionary<EffectAction, List<EffectType>> EffectActionTypeRestrictions => new()
    {
        [EffectAction.ChangeCharacterSprites] = new() { EffectType.OneTime },
        [EffectAction.AddBanishes] = new() { EffectType.OneTime },
        [EffectAction.AddRerolls] = new() { EffectType.OneTime },
        [EffectAction.AddRarityRerolls] = new() { EffectType.OneTime },
    };

    public static bool IsEffectTemplateValid(ConfigurableEffect template)
    {
        if (!PassesActionTriggerRestrictions(template))
        {
            return false;
        }

        switch (template.Type)
        {
            case EffectType.OneTime:
                // return CheckAllNotNull(template.Properties.)
                break;
            case EffectType.Duration:
                break;
            case EffectType.Interval:
                break;
            case EffectType.Trigger:
                break;
            default:
                return false;
        }

        return true;
    }

    private static bool PassesActionTriggerRestrictions(ConfigurableEffect template)
    {
        if (EffectActionTypeRestrictions.TryGetValue(template.Action, out var allowedTypes))
        {
            return allowedTypes.Contains(template.Type);
        }
        return true;
    }
}
