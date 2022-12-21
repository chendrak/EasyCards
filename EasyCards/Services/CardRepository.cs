using System.Collections.Immutable;
using RogueGenesia.Data;

namespace EasyCards.Services;

public static class CardRepository
{
    private static ImmutableArray<SoulCardScriptableObject>? _cardCache;

    public static ImmutableArray<SoulCardScriptableObject> GetAllCards() =>
        _cardCache ??= GameDataGetter.GetAllCards().ToImmutableArray();

}
