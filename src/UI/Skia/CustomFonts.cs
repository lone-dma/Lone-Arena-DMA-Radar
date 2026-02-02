/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Misc;

namespace LoneArenaDmaRadar.UI.Skia
{
    internal static class CustomFonts
    {
        /// <summary>
        /// Neo Sans Std Regular
        /// </summary>
        public static SKTypeface NeoSansStdRegular { get; }

        static CustomFonts()
        {
            try
            {
                byte[] neoSansStdRegular;
                using (var stream = Utilities.OpenResource("LoneArenaDmaRadar.Resources.NeoSansStdRegular.otf"))
                {
                    neoSansStdRegular = new byte[stream!.Length];
                    stream.ReadExactly(neoSansStdRegular);
                }
                NeoSansStdRegular = SKTypeface.FromStream(new MemoryStream(neoSansStdRegular, false));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ERROR Loading Custom Fonts!", ex);
            }
        }
    }
}
