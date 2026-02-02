/*
 * Lone Arena DMA Radar - Copyright (c) 2026 Lone DMA
 * Licensed under GNU AGPLv3. See https://www.gnu.org/licenses/agpl-3.0.html
 */

namespace LoneArenaDmaRadar.Misc.Workers
{
    /// <summary>
    /// Contains arguments for Worker Thread API.
    /// </summary>
    public sealed class WorkerThreadArgs : EventArgs
    {
        public WorkerThreadArgs(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }
        /// <summary>
        /// Cancellation Token for this Thread. When the object is disposed this token will be cancelled to signal the thread to stop.
        /// </summary>
        public CancellationToken CancellationToken { get; }
    }
}
