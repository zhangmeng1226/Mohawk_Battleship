namespace MBC.Shared
{
    /// <summary>
    /// The GameMode enumeration identifies the type of game logic that will be used in a battleship round.
    /// </summary>
    public enum GameMode
    {
        /// <summary>
        /// A classic game of battleship.
        /// </summary>
        Classic = 1,

        /// <summary>
        /// A game of battleship where controllers may make as many shots as they have remaining ships in
        /// their turn.
        /// </summary>
        Salvo = 2,

        /// <summary>
        /// A game of battleship that contains items on the field that can be shot at to gain certain powered
        /// items. The controller is given a hint as to the whereabouts of the nearest item on the field.
        /// The controllers may make one shot per turn as in classic mode.
        /// </summary>
        Powered = 4,

        /// <summary>
        /// A game of battleship where controllers are free to fire at any time during the game. The
        /// motivation for this game mode is to make decisions on where to fire in the least amount of time
        /// to maximize shots made in a certain time.
        /// </summary>
        FreeFire = 8,

        /// <summary>
        /// When paired with one of the game modes given above, indicates a game mode of more than two
        /// controllers.
        /// </summary>
        Multi = 16,

        /// <summary>
        /// When paired with one of the game modes given above, indicates a game mode that includes teams.
        /// </summary>
        Teams = 32,
    }
}