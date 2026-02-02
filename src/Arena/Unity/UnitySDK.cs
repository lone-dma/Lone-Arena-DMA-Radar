/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

namespace LoneArenaDmaRadar.Arena.Unity
{
    public readonly struct UnitySDK
    {
        public readonly struct ModuleBase
        {
            public const uint GameObjectManager = 0x1CF93E0; // to eft_dma_radar.GameObjectManager
        }
        public readonly struct TransformInternal
        {
            public const uint TransformAccess = 0x38; // to TransformHierarchy
        }
        public readonly struct TransformAccess
        {
            public const uint Vertices = 0x18; // MemList<TrsX>
            public const uint Indices = 0x20; // MemList<int>
        }
    }
}
