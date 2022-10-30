using System.Collections.Generic;
using EasyCards.Models.Templates;

namespace EasyCards.Bootstrap;

public interface ICardLoader : IModuleBootstrap
{
    public Dictionary<string, CardTemplate> GetLoadedCards();
}
