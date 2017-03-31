﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Game.Graphics.Transforms;

namespace osu.Game.Graphics
{
    /// <summary>
    /// A type of drawable that has an accent colour.
    /// The accent colour is used to colorize various objects inside a drawable
    /// without colorizing the drawable itself.
    /// </summary>
    public interface IAccented : IDrawable
    {
        Color4 AccentColour { get; set; }
    }

    public static class AccentedExtensions
    {
        /// <summary>
        /// Tweens the accent colour of a drawable to another colour.
        /// </summary>
        /// <typeparam name="TDrawable">The type of drawable.</typeparam>
        /// <param name="drawable">The drawable to apply the accent colour to.</param>
        /// <param name="newColour">The new accent colour.</param>
        /// <param name="duration">The tween duration.</param>
        /// <param name="easing">The tween easing.</param>
        public static void FadeAccent<TDrawable>(this TDrawable drawable, Color4 newColour, double duration = 0, EasingTypes easing = EasingTypes.None)
            where TDrawable : Drawable, IAccented
        {
            drawable.TransformTo(drawable.AccentColour, newColour, duration, easing, new TransformAccent());
        }
    }
}
