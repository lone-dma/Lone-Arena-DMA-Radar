/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Arena.Mono.Collections;

namespace LoneArenaDmaRadar.Arena.World.Explosives
{
    public sealed class ExplosivesManager : IReadOnlyCollection<IExplosiveItem>
    {
        private readonly ulong _localGameWorld;
        private readonly ConcurrentDictionary<ulong, IExplosiveItem> _explosives = new();

        public ExplosivesManager(ulong localGameWorld)
        {
            _localGameWorld = localGameWorld;
        }

        /// <summary>
        /// Check for "hot" explosives in World if due.
        /// </summary>
        public void Refresh(CancellationToken ct)
        {
            GetGrenades(ct);
            var explosives = _explosives.Values;
            if (explosives.Count == 0)
            {
                return;
            }
            using var scatter = Memory.CreateScatter(VmmSharpEx.Options.VmmFlags.NOCACHE);
            foreach (var explosive in explosives)
            {
                ct.ThrowIfCancellationRequested();
                try
                {
                    explosive.OnRefresh(scatter);
                }
                catch (Exception ex)
                {
                    Logging.WriteLine($"Error Refreshing Explosive @ 0x{explosive.Addr.ToString("X")}: {ex}");
                }
            }
            scatter.Execute();
        }

        private void GetGrenades(CancellationToken ct)
        {
            try
            {
                var grenades = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.Grenades);
                var grenadesListPtr = Memory.ReadPtr(grenades + 0x18);
                using var grenadesList = MonoList<ulong>.Create(grenadesListPtr, false);
                foreach (var grenade in grenadesList)
                {
                    ct.ThrowIfCancellationRequested();
                    try
                    {
                        _ = _explosives.GetOrAdd(
                            grenade,
                            addr => new Grenade(addr, _explosives));
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLine($"Error Processing Grenade @ 0x{grenade.ToString("X")}: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLine($"Grenades Error: {ex}");
            }
        }

        #region IReadOnlyCollection

        public int Count => _explosives.Values.Count;
        public IEnumerator<IExplosiveItem> GetEnumerator() => _explosives.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}