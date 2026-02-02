/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using Collections.Pooled;

namespace LoneArenaDmaRadar.Arena.Mono.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# HashSet
    /// Must initialize before use. Must dispose after use.
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    public sealed class MonoHashSet<T> : PooledMemory<MonoHashSet<T>.MemHashEntry>
        where T : unmanaged
    {
        public const uint CountOffset = 0x3C;
        public const uint ArrOffset = 0x18;
        public const uint ArrStartOffset = 0x20;

        private MonoHashSet() : base(0) { }
        private MonoHashSet(int count) : base(count) { }

        /// <summary>
        /// Factory method to create a new <see cref="MonoHashSet{T}"/> instance from a memory address.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static MonoHashSet<T> Create(ulong addr, bool useCache = true)
        {
            var count = LoneArenaDmaRadar.DMA.Memory.ReadValue<int>(addr + CountOffset, useCache);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 16384, nameof(count));
            var hs = new MonoHashSet<T>(count);
            try
            {
                if (count == 0)
                {
                    return hs;
                }
                var hashSetBase = LoneArenaDmaRadar.DMA.Memory.ReadPtr(addr + ArrOffset, useCache) + ArrStartOffset;
                LoneArenaDmaRadar.DMA.Memory.ReadSpan(hashSetBase, hs.Span, useCache);
                return hs;
            }
            catch
            {
                hs.Dispose();
                throw;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public readonly struct MemHashEntry
        {
            public static implicit operator T(MemHashEntry x) => x.Value;

            private readonly int _hashCode;
            private readonly int _next;
            public readonly T Value;
        }
    }
}
