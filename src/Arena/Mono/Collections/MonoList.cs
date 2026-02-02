/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using Collections.Pooled;

namespace LoneArenaDmaRadar.Arena.Mono.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# List
    /// Must initialize before use. Must dispose after use.
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    public sealed class MonoList<T> : PooledMemory<T>
        where T : unmanaged
    {
        public const uint CountOffset = 0x18;
        public const uint ArrOffset = 0x10;
        public const uint ArrStartOffset = 0x20;

        private MonoList() : base(0) { }
        private MonoList(int count) : base(count) { }

        /// <summary>
        /// Factory method to create a new <see cref="MonoList{T}"/> instance from a memory address.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static MonoList<T> Create(ulong addr, bool useCache = true)
        {
            var count = LoneArenaDmaRadar.DMA.Memory.ReadValue<int>(addr + CountOffset, useCache);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 16384, nameof(count));
            var list = new MonoList<T>(count);
            try
            {
                if (count == 0)
                {
                    return list;
                }
                var listBase = LoneArenaDmaRadar.DMA.Memory.ReadPtr(addr + ArrOffset, useCache) + ArrStartOffset;
                LoneArenaDmaRadar.DMA.Memory.ReadSpan(listBase, list.Span, useCache);
                return list;
            }
            catch
            {
                list.Dispose();
                throw;
            }
        }
    }
}
