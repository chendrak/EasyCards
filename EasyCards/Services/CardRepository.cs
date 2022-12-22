using System.Collections.Immutable;
using RogueGenesia.Data;

namespace EasyCards.Services;

public static class CardRepository
{
    public static ImmutableArray<SoulCardScriptableObject> GetAllCards() => GameDataGetter.GetAllCards().ToImmutableArray();
}
