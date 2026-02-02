/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using Collections.Pooled;

namespace LoneArenaDmaRadar.Arena.Mono.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# Dictionary
    /// Must initialize before use. Must dispose after use.
    /// </summary>
    /// <typeparam name="TKey">Key Type between 1-8 bytes.</typeparam>
    /// <typeparam name="TValue">Value Type between 1-8 bytes.</typeparam>
    public sealed class MonoDictionary<TKey, TValue> : PooledMemory<MonoDictionary<TKey, TValue>.MemDictEntry>
        where TKey : unmanaged
        where TValue : unmanaged
    {
        public const uint CountOffset = 0x40;
        public const uint EntriesOffset = 0x18;
        public const uint EntriesStartOffset = 0x20;

        private MonoDictionary() : base(0) { }
        private MonoDictionary(int count) : base(count) { }

        /// <summary>
        /// Factory method to create a new <see cref="MonoDictionary{TKey, TValue}"/> instance from a memory address.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static MonoDictionary<TKey, TValue> Create(ulong addr, bool useCache = true)
        {
            var count = LoneArenaDmaRadar.DMA.Memory.ReadValue<int>(addr + CountOffset, useCache);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 16384, nameof(count));
            var dict = new MonoDictionary<TKey, TValue>(count);
            try
            {
                if (count == 0)
                {
                    return dict;
                }
                var dictBase = LoneArenaDmaRadar.DMA.Memory.ReadPtr(addr + EntriesOffset, useCache) + EntriesStartOffset;
                LoneArenaDmaRadar.DMA.Memory.ReadSpan(dictBase, dict.Span, useCache); // Single read into mem buffer
                return dict;
            }
            catch
            {
                dict.Dispose();
                throw;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public readonly struct MemDictEntry
        {
            private readonly ulong _pad00;
            public readonly TKey Key;
            public readonly TValue Value;
        }
    }
}
