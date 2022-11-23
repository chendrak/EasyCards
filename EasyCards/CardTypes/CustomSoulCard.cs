namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using BepInEx.Logging;
using Common.Helpers;
using Extensions;
using Helpers;
using ModGenesia;
using RogueGenesia.Data;

public abstract class CustomSoulCard : SoulCard
{
    protected ManualLogSource Log = EasyCards.Instance.Log;

    public string Name => this.GetType().Name;
    protected virtual string ModSource => MyPluginInfo.PLUGIN_NAME;
    protected virtual float DropWeight => throw new NotImplementedException();
    protected virtual float LevelUpWeight => throw new NotImplementedException();
    protected virtual CardRarity Rarity => throw new NotImplementedException();

    protected virtual CardTag Tags => throw new NotImplementedException();
    protected virtual int MaxLevel => throw new NotImplementedException();
    protected virtual Dictionary<string, string> LocalizedNames => throw new NotImplementedException();
    public virtual Dictionary<string, string> LocalizedDescriptions => throw new NotImplementedException();
    protected virtual string TexturePath => throw new NotImplementedException();

    protected CustomSoulCard(IntPtr ptr) : base(ptr) { }

    public SoulCardCreationData GetSoulCardCreationData()
    {
        var result = new SoulCardCreationData();
        result.ModSource = this.ModSource;
        result.DropWeight = this.DropWeight;
        result.LevelUpWeight = this.LevelUpWeight;
        result.Rarity = this.Rarity;
        result.Tags = this.Tags;
        result.Texture = SpriteHelper.LoadSprite(this.TexturePath);
        result.MaxLevel = this.MaxLevel;
        result.NameOverride = Localization.GetTranslations(this.LocalizedNames).ToIl2CppList();
        result.DescriptionOverride = Localization.GetTranslations(this.LocalizedDescriptions).ToIl2CppList();
        result.ModifyPlayerStat = false;
        return result;
    }
}
