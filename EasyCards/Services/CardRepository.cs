using RogueGenesia.Data;

namespace EasyCards.Services;

using System.Collections.Generic;
using System.Linq;

public static class CardRepository
{
    public static List<SoulCardScriptableObject> GetAllCards() => GameDataGetter.GetAllCards().ToList();
}
