/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

namespace LoneArenaDmaRadar.Arena.World.Player
{
    /// <summary>
    /// Defines Player Unit Type (Player,PMC,Scav,etc.)
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// Default value if a type cannot be established.
        /// </summary>
        Default,
        /// <summary>
        /// Teammate of LocalPlayer.
        /// </summary>
        Teammate,
        /// <summary>
        /// Hostile/Enemy Player.
        /// </summary>
        Player,
        /// <summary>
        /// Normal Bot.
        /// </summary>
        Bot,
        /// <summary>
        /// Human Controlled Hostile PMC/Scav that has a Twitch account name as their IGN.
        /// </summary>
        Streamer
    }
}
