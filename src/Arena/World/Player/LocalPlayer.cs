/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Arena.Unity.Structures;

namespace LoneArenaDmaRadar.Arena.World.Player
{
    public sealed class LocalPlayer : ClientPlayer
    {
        /// <summary>
        /// Player name.
        /// </summary>
        public override string Name
        {
            get => "localPlayer";
            protected set { }
        }
        public override bool IsHuman
        {
            get => true;
            protected set { }
        }

        public LocalPlayer(ulong playerBase) : base(playerBase)
        {
            string classType = ObjectClass.ReadName(this);
            if (classType != "ArenaClientPlayer")
                throw new ArgumentException(nameof(classType));
        }
    }
}
