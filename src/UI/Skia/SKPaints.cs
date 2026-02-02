/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.UI.Widgets;

namespace LoneArenaDmaRadar.UI.Skia
{
    internal static class SKPaints
    {
        /// <summary>
        /// Gets an SKColorFilter that will reduce an image's brightness level.
        /// </summary>
        /// <param name="brightnessFactor">Adjust this value between 0 (black) and 1 (original brightness), where values less than 1 reduce brightness</param>
        /// <returns>SKColorFilter Object.</returns>
        public static SKColorFilter GetDarkModeColorFilter(float brightnessFactor)
        {
            float[] colorMatrix = {
                brightnessFactor, 0, 0, 0, 0, // Red channel
                0, brightnessFactor, 0, 0, 0, // Green channel
                0, 0, brightnessFactor, 0, 0, // Blue channel
                0, 0, 0, 1, 0, // Alpha channel
            };
            return SKColorFilter.CreateColorMatrix(colorMatrix);
        }

        #region Radar Paints

        public static SKPaint PaintBitmap { get; } = new()
        {
            IsAntialias = false
        };

        public static SKPaint PaintBitmapAlpha { get; } = new()
        {
            Color = SKColor.Empty.WithAlpha(127),
            IsAntialias = false,
        };

        public static SKPaint PaintLocalPlayer { get; } = new()
        {
            Color = SKColors.Green,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextLocalPlayer { get; } = new()
        {
            Color = SKColors.Green,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintTeammate { get; } = new()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextTeammate { get; } = new()
        {
            Color = SKColors.LimeGreen,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintPlayer { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextPlayer { get; } = new()
        {
            Color = SKColors.Red,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintStreamer { get; } = new()
        {
            Color = SKColors.MediumPurple,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextStreamer { get; } = new()
        {
            Color = SKColors.MediumPurple,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintBot { get; } = new()
        {
            Color = SKColors.Yellow,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextBot { get; } = new()
        {
            Color = SKColors.Yellow,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintFocused { get; } = new()
        {
            Color = SKColors.Coral,
            StrokeWidth = 1.66f,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            StrokeJoin = SKStrokeJoin.Round,
            StrokeCap = SKStrokeCap.Round
        };

        public static SKPaint TextFocused { get; } = new()
        {
            Color = SKColors.Coral,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint TextMouseover { get; } = new()
        {
            Color = SKColors.White,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintDeathMarker { get; } = new()
        {
            Color = SKColors.Black,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
        };

        #endregion

        #region Render/Misc Paints

        public static SKPaint PaintTransparentBacker { get; } = new()
        {
            Color = SKColors.Black.WithAlpha(0xBE), // Transparent backer
            StrokeWidth = 0.25f,
            Style = SKPaintStyle.Fill
        };

        public static SKPaint TextRadarStatus { get; } = new()
        {
            Color = SKColors.Red,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint TextStatusSmall { get; } = new SKPaint
        {
            Color = SKColors.Red,
            IsStroke = false,
            IsAntialias = true,
        };

        public static SKPaint PaintExplosives { get; } = new()
        {
            Color = SKColors.OrangeRed,
            StrokeWidth = 3,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
        };

        public static SKPaint TextOutline { get; } = new()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            IsStroke = true,
            StrokeWidth = 2f,
            Style = SKPaintStyle.Stroke,
        };

        /// <summary>
        /// Only utilize this paint on the Radar UI Thread. StrokeWidth is modified prior to each draw call.
        /// *NOT* Thread safe to use!
        /// </summary>
        public static SKPaint ShapeOutline { get; } = new()
        {
            Color = SKColors.Black,
            /*StrokeWidth = ??,*/ // Compute before use
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
        };

        #endregion

        #region ESP Widget Paints

        public static SKPaint PaintAimviewWidgetCrosshair { get; } = new()
        {
            Color = SKColors.White,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        public static SKPaint PaintAimviewWidgetLocalPlayer { get; } = new()
        {
            Color = SKColors.Green,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetPlayer { get; } = new()
        {
            Color = SKColors.Red,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetTeammate { get; } = new()
        {
            Color = SKColors.LimeGreen,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetStreamer { get; } = new()
        {
            Color = SKColors.MediumPurple,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetBot { get; } = new()
        {
            Color = SKColors.Yellow,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetRaider { get; } = new()
        {
            Color = SKColor.Parse("ffc70f"),
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        public static SKPaint PaintAimviewWidgetFocused { get; } = new()
        {
            Color = SKColors.Coral,
            StrokeWidth = AimviewWidget.AimviewBaseStrokeSize,
            Style = SKPaintStyle.Stroke
        };

        #endregion

    }
}
