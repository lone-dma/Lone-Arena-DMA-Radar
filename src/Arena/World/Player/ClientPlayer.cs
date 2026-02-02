/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using LoneArenaDmaRadar.Arena.Mono.Collections;
using LoneArenaDmaRadar.Arena.Unity.Structures;
using static SDK.Offsets;

namespace LoneArenaDmaRadar.Arena.World.Player
{
    public class ClientPlayer : AbstractPlayer
    {
        /// <summary>
        /// EFT.Profile Address
        /// </summary>
        public ulong Profile { get; }
        /// <summary>
        /// PlayerInfo Address (GClass1044)
        /// </summary>
        public ulong Info { get; }

        internal ClientPlayer(ulong playerBase) : base(playerBase)
        {
            Profile = Memory.ReadPtr(this + Offsets.Player.Profile);
            Info = Memory.ReadPtr(Profile + Offsets.Profile.Info);
            CorpseAddr = this + Offsets.Player.Corpse;

            AccountID = GetAccountID();
            TeamID = GetTeamID();
            if (GameWorld.MatchHasTeams)
                ArgumentOutOfRangeException.ThrowIfEqual(TeamID, -1, nameof(TeamID));
            MovementContext = GetMovementContext();
            RotationAddress = ValidateRotationAddr(MovementContext + Offsets.MovementContext._rotation);
            /// Setup Transform
            Span<uint> tiOffsets = stackalloc uint[6];
            GetTransformInternalChain(Unity.Structures.Bones.HumanBase, tiOffsets);
            var tiRoot = Memory.ReadPtrChain(this, true, tiOffsets);
            SkeletonRoot = new UnityTransform(tiRoot);
            _ = SkeletonRoot.UpdatePosition();

            if (this is LocalPlayer) // Handled in derived class
                return;

            IsHuman = true;
            Name = GetName();
            Type = PlayerType.Player;
        }

        /// <summary>
        /// Get Player Name.
        /// </summary>
        /// <returns>Player Name String.</returns>
        private string GetName()
        {
            var namePtr = Memory.ReadPtr(Info + Offsets.PlayerInfo.Nickname);
            var name = Memory.ReadUnityString(namePtr)?.Trim();
            if (string.IsNullOrEmpty(name))
                name = "default";
            return name;
        }

        /// <summary>
        /// Gets player's Team ID.
        /// </summary>
        private int GetTeamID()
        {
            try
            {
                var inventoryController = Memory.ReadPtr(this + Offsets.Player._inventoryController);
                return GetTeamID(inventoryController);
            }
            catch { return -1; }
        }

        /// <summary>
        /// Get Player's Account ID.
        /// </summary>
        /// <returns>Account ID Numeric String.</returns>
        private string GetAccountID()
        {
            var idPTR = Memory.ReadPtr(Profile + Offsets.Profile.AccountId);
            return Memory.ReadUnityString(idPTR);
        }

        /// <summary>
        /// Get Movement Context Instance.
        /// </summary>
        private ulong GetMovementContext()
        {
            var movementContext = Memory.ReadPtr(this + Offsets.Player.MovementContext);
            var player = Memory.ReadPtr(movementContext + Offsets.MovementContext.Player, false);
            if (player != this)
                throw new ArgumentOutOfRangeException(nameof(movementContext));
            return movementContext;
        }

        /// <summary>
        /// Get the Transform Internal Chain for this Player.
        /// </summary>
        /// <param name="bone">Bone to lookup.</param>
        /// <param name="offsets">Buffer to receive offsets.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GetTransformInternalChain(Bones bone, Span<uint> offsets)
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(offsets.Length, 6, nameof(offsets));
            offsets[0] = Offsets.Player._playerBody;
            offsets[1] = PlayerBody.SkeletonRootJoint;
            offsets[2] = DizSkinningSkeleton._values;
            offsets[3] = MonoList<byte>.ArrOffset;
            offsets[4] = MonoList<byte>.ArrStartOffset + (uint)bone * 0x8;
            offsets[5] = 0x10;
        }
    }
}
