/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

namespace LoneArenaDmaRadar.Arena.Unity.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct ComponentArray
    {
        public readonly ulong ArrayBase; // To ComponentArrayEntry[]
        public readonly ulong MemLabelId;
        public readonly ulong Size;
        public readonly ulong Capacity;

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public readonly struct Entry
        {
            [FieldOffset(0x8)]
            public readonly ulong Component;
        }
    }
}
