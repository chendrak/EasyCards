namespace EasyCards.Services;

using UnityEngine;
using Validation;

public sealed class SpriteTextureValidator : ValidatorBase<Texture2D>
{
    protected override ResultOrError<Texture2D> Validate(Texture2D? item)
    {
        if (item is null)
            return new ResultOrError<Texture2D>("Error: Texture was not loaded.");
        return item;
    }
}
