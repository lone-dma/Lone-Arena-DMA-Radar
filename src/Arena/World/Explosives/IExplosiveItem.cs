/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Arena.Unity;
using LoneArenaDmaRadar.UI.Maps;
using VmmSharpEx.Scatter;

namespace LoneArenaDmaRadar.Arena.World.Explosives
{
    public interface IExplosiveItem : IWorldEntity, IMapEntity
    {
        /// <summary>
        /// Base address of the explosive item.
        /// </summary>
        ulong Addr { get; }
        /// <summary>
        /// Refresh the state of the explosive item.
        /// </summary>
        void OnRefresh(VmmScatter scatter);
    }
}
