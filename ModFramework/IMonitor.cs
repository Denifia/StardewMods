using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewModdingApi
{
    /// <summary>Encapsulates monitoring and logging for a given module.</summary>
    public interface IMonitor
    {
        /*********
        ** Methods
        *********/
        /// <summary>Log a message for the player or developer.</summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">The log severity level.</param>
        void Log(string message, LogLevel level = LogLevel.Debug);

        /// <summary>Immediately exit the game without saving. This should only be invoked when an irrecoverable fatal error happens that risks save corruption or game-breaking bugs.</summary>
        /// <param name="reason">The reason for the shutdown.</param>
        void ExitGameImmediately(string reason);
    }
}
