/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Arena.Unity;
using LoneArenaDmaRadar.Arena.Unity.Structures;
using LoneArenaDmaRadar.Arena.World.Player;
using LoneArenaDmaRadar.Misc;
using LoneArenaDmaRadar.UI.Maps;
using LoneArenaDmaRadar.UI.Skia;
using VmmSharpEx.Extensions;
using VmmSharpEx.Scatter;

namespace LoneArenaDmaRadar.Arena.World.Explosives
{
    /// <summary>
    /// Represents a 'Hot' grenade in Local Game World.
    /// </summary>
    public sealed class Grenade : IExplosiveItem, IWorldEntity, IMapEntity
    {
        public static implicit operator ulong(Grenade x) => x.Addr;
        private static readonly uint[] _toPosChain =
            ObjectClass.To_GameObject.Concat(new uint[] { GameObject.ComponentsOffset, 0x8, 0x38 }).ToArray();
        private readonly ConcurrentDictionary<ulong, IExplosiveItem> _parent;
        private readonly bool _isSmoke;
        private readonly ulong _posAddr;

        /// <summary>
        /// Base Address of Grenade Object.
        /// </summary>
        public ulong Addr { get; }

        public Grenade(ulong baseAddr, ConcurrentDictionary<ulong, IExplosiveItem> parent)
        {
            baseAddr.ThrowIfInvalidUserVA(nameof(baseAddr));
            Addr = baseAddr;
            _parent = parent;
            var type = ObjectClass.ReadName(baseAddr, 64, false);
            if (type.Contains("SmokeGrenade"))
            {
                _isSmoke = true;
                return;
            }
            _posAddr = Memory.ReadPtrChain(baseAddr, false, _toPosChain) + 0x90;
        }

        /// <summary>
        /// Get the updated Position of this Grenade.
        /// </summary>
        public void OnRefresh(VmmScatterManaged scatter)
        {
            if (_isSmoke)
            {
                // Smokes never leave the list, don't remove
                return;
            }
            scatter.PrepareReadValue<Vector3>(_posAddr);
            scatter.PrepareReadValue<bool>(this + Offsets.Grenade.IsDestroyed);
            scatter.Completed += (sender, x1) =>
            {
                if (x1.ReadValue(_posAddr, out Vector3 pos) && pos.IsNormal())
                {
                    _position = pos;
                }
                if (x1.ReadValue(this + Offsets.Grenade.IsDestroyed, out bool isDestroyed) && isDestroyed)
                {
                    _parent.TryRemove(this, out _);
                }
            };
        }

        #region Interfaces

        private Vector3 _position;
        public ref readonly Vector3 Position => ref _position;

        public void Draw(SKCanvas canvas, EftMapParams mapParams, LocalPlayer localPlayer)
        {
            if (_isSmoke)
                return;
            var circlePosition = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            const float size = 5f;
            SKPaints.ShapeOutline.StrokeWidth = SKPaints.PaintExplosives.StrokeWidth + 2f;
            canvas.DrawCircle(circlePosition, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(circlePosition, size, SKPaints.PaintExplosives); // draw LocalPlayer marker
        }

        #endregion
    }
}
