namespace EasyCards.Effects;

using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public interface IOnKill
{
    public void OnKill(PlayerData owner, IEntity killedEntity);
}
