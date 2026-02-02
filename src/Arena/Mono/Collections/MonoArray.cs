/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using Collections.Pooled;

namespace LoneArenaDmaRadar.Arena.Mono.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# Array
    /// Must initialize before use. Must dispose after use.
    /// </summary>
    /// <typeparam name="T">Array Type</typeparam>
    public sealed class MonoArray<T> : PooledMemory<T>
        where T : unmanaged
    {
        public const uint CountOffset = 0x18;
        public const uint ArrBaseOffset = 0x20;

        private MonoArray() : base(0) { }
        private MonoArray(int count) : base(count) { }

        /// <summary>
        /// Factory method to create a new <see cref="MonoArray{T}"/> instance from a memory address.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public static MonoArray<T> Create(ulong addr, bool useCache = true)
        {
            var count = LoneArenaDmaRadar.DMA.Memory.ReadValue<int>(addr + CountOffset, useCache);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, 16384, nameof(count));
            var array = new MonoArray<T>(count);
            try
            {
                if (count == 0)
                {
                    return array;
                }
                LoneArenaDmaRadar.DMA.Memory.ReadSpan(addr + ArrBaseOffset, array.Span, useCache);
                return array;
            }
            catch
            {
                array.Dispose();
                throw;
            }
        }
    }
}
