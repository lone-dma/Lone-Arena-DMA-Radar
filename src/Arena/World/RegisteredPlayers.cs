/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

using Collections.Pooled;
using LoneArenaDmaRadar.Arena.Mono.Collections;
using LoneArenaDmaRadar.Arena.World.Player;
using VmmSharpEx.Extensions;

namespace LoneArenaDmaRadar.Arena.World
{
    public sealed class RegisteredPlayers : IReadOnlyCollection<AbstractPlayer>
    {
        #region Fields/Properties/Constructor

        public static implicit operator ulong(RegisteredPlayers x) => x.Base;
        private ulong Base { get; }
        private readonly GameWorld _game;
        private readonly ConcurrentDictionary<ulong, AbstractPlayer> _players = new();

        /// <summary>
        /// LocalPlayer Instance.
        /// </summary>
        public LocalPlayer LocalPlayer { get; private set; }

        /// <summary>
        /// RegisteredPlayers List Constructor.
        /// </summary>
        public RegisteredPlayers(ulong baseAddr, GameWorld game)
        {
            Base = baseAddr;
            _game = game;
            var mainPlayer = Memory.ReadPtr(_game + Offsets.ClientLocalGameWorld.MainPlayer, false);
            var localPlayer = new LocalPlayer(mainPlayer);
            _players[localPlayer] = LocalPlayer = localPlayer;
        }

        #endregion

        /// <summary>
        /// Updates the ConcurrentDictionary of 'Players'
        /// </summary>
        public void Refresh()
        {
            try
            {
                var mainPlayer = Memory.ReadPtr(_game + Offsets.ClientLocalGameWorld.MainPlayer);
                if (mainPlayer != LocalPlayer)
                {
                    var localPlayer = new LocalPlayer(mainPlayer);
                    _players.Clear();
                    _players[localPlayer] = LocalPlayer = localPlayer;
                }
                ArgumentNullException.ThrowIfNull(LocalPlayer, nameof(LocalPlayer));
                using var playersList = MonoList<ulong>.Create(this, false); // Realtime Read
                using var registered = playersList.Where(x => x.IsValidUserVA()).ToPooledSet();
                /// Allocate New Players
                foreach (var playerBase in registered)
                {
                    if (playerBase == LocalPlayer) // Skip LocalPlayer, already allocated
                        continue;
                    // Add New Player
                    AbstractPlayer.Allocate(_players, playerBase);
                }
                /// Update Existing Players
                UpdateExistingPlayers(registered);
            }
            catch (Exception ex)
            {
                Logging.WriteLine($"CRITICAL ERROR - RegisteredPlayers Loop FAILED: {ex}");
            }
        }

        /// <summary>
        /// Returns the Player Count currently in the Registered Players List.
        /// </summary>
        /// <returns>Count of players.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetPlayerCount()
        {
            var count = Memory.ReadValue<int>(this + MonoList<byte>.CountOffset, false);
            if (count < 0 || count > 256)
                throw new ArgumentOutOfRangeException(nameof(count));
            return count;
        }

        /// <summary>
        /// Scans the existing player list and updates Players as needed.
        /// </summary>
        private void UpdateExistingPlayers(ISet<ulong> registered)
        {
            var allPlayers = _players.Values;
            if (allPlayers.Count == 0)
                return;
            using var scatter = Memory.CreateScatter(VmmSharpEx.Options.VmmFlags.NOCACHE);
            foreach (var player in allPlayers)
            {
                player.OnRegRefresh(scatter, registered);
            }
            scatter.Execute();
        }

        #region IReadOnlyCollection
        public int Count => _players.Values.Count;
        public IEnumerator<AbstractPlayer> GetEnumerator() =>
            _players.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
