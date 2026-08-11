namespace FPLBE.Models
{
    /// <summary>
    /// Represents the playing positions available to a player
    /// in Fantasy Premier League (FPL).
    /// </summary>
    public enum FplPlayerPosition
    {
        /// <summary>
        /// Indicates that the player's position has not been set
        /// or could not be determined.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// Represents a goalkeeper.
        /// </summary>
        Goalkeeper = 1,

        /// <summary>
        /// Represents a defender.
        /// </summary>
        Defender = 2,

        /// <summary>
        /// Represents a midfielder.
        /// </summary>
        Midfielder = 3,

        /// <summary>
        /// Represents a forward.
        /// </summary>
        Forward = 4,
    }
}