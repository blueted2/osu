// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Configuration;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Skinning;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Osu.Skinning.Default
{
    public abstract class PlaySliderBody : SnakingSliderBody
    {
        private IBindable<float> scaleBindable;
        private IBindable<int> pathVersion;
        private IBindable<Color4> accentColour;

        [Resolved(CanBeNull = true)]
        private OsuRulesetConfigManager config { get; set; }

        private readonly Bindable<bool> snakingOut = new Bindable<bool>();

        [BackgroundDependencyLoader]
        private void load(ISkinSource skin, DrawableHitObject drawableObject)
        {
            var drawableSlider = (DrawableSlider)drawableObject;

            scaleBindable = drawableSlider.ScaleBindable.GetBoundCopy();
            scaleBindable.BindValueChanged(scale => PathRadius = OsuHitObject.OBJECT_RADIUS * scale.NewValue, true);

            pathVersion = drawableSlider.PathVersion.GetBoundCopy();
            pathVersion.BindValueChanged(_ => Refresh());

            accentColour = drawableObject.AccentColour.GetBoundCopy();
            accentColour.BindValueChanged(accent => updateAccentColour(skin, accent.NewValue), true);

            SnakingOut.BindTo(snakingOut);
            config?.BindWith(OsuRulesetSetting.SnakingInSliders, SnakingIn);
            config?.BindWith(OsuRulesetSetting.SnakingOutSliders, snakingOut);

            BorderSize = skin.GetConfig<OsuSkinConfiguration, float>(OsuSkinConfiguration.SliderBorderSize)?.Value ?? 1;
            BorderColour = skin.GetConfig<OsuSkinColour, Color4>(OsuSkinColour.SliderBorder)?.Value ?? Color4.White;

            drawableObject.HitObjectApplied += onHitObjectApplied;
            onHitObjectApplied(drawableObject);
        }

        private void onHitObjectApplied(DrawableHitObject obj)
        {
            var drawableSlider = (DrawableSlider)obj;
            if (drawableSlider.HitObject == null)
                return;

            if (!drawableSlider.HitObject.HeadCircle.TrackFollowCircle)
            {
                // When not tracking the follow circle, force the path to not snake out as it looks better that way.
                SnakingOut.UnbindFrom(snakingOut);
                SnakingOut.Value = false;
            }
        }

        private void updateAccentColour(ISkinSource skin, Color4 defaultAccentColour)
            => AccentColour = skin.GetConfig<OsuSkinColour, Color4>(OsuSkinColour.SliderTrackOverride)?.Value ?? defaultAccentColour;
    }
}
