namespace EasyCards.Effects;

using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using Models.Templates;

public static class EffectHolder
{
    private static ManualLogSource Log = EasyCards.Instance.Log;
    private static readonly Dictionary<string, List<ConfigurableEffect>> CardEffects = new();

    public static void AddEffect(string cardName, ConfigurableEffect effect)
    {
        Log.LogInfo($"AddEffect({cardName}, {effect})");
        InitializeListForCardIfNecessary(cardName);
        if (string.IsNullOrEmpty(effect.Name))
        {
            var numEffects = CardEffects[cardName].Count;
            effect.Name = $"{cardName}-{numEffects + 1}-{effect.Type}";
        }
        CardEffects[cardName].Add(effect);
    }

    public static void ResetEffects()
    {
        foreach (var effect in CardEffects.Values.SelectMany(effects => effects))
        {
            effect.Reset();
        }
    }

    public static void ClearEffects(string cardName)
    {
        Log.LogInfo($"ClearEffects({cardName})");
        if (CardEffects.ContainsKey(cardName))
        {
            CardEffects[cardName].Clear();
        }
    }

    public static List<ConfigurableEffect> GetEffects(string cardName)
    {
        Log.LogInfo($"GetEffects({cardName})");
        return CardEffects.TryGetValue(cardName, out var effects) ? effects : new List<ConfigurableEffect>();
    }

    private static void InitializeListForCardIfNecessary(string cardName)
    {
        Log.LogInfo($"InitializeListForCardIfNecessary({cardName})");
        if (!CardEffects.ContainsKey(cardName))
        {
            CardEffects[cardName] = new List<ConfigurableEffect>();
        }
    }
}
