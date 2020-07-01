using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class to hold static strings for all Mob tags/names in the game
/// </summary>
public class TagStatics
{
    //**NAMES**//

    /// <summary>
    /// The name assigned to the Player
    /// </summary>
    private static string _playerName = "TankActor";

    //**TAGS**//

    /// <summary>
    /// The tag assigned to Pickup prefabs
    /// </summary>
    private static string _pickupTag = "Pickup";

    /// <summary>
    /// The tag assigned to Mob prefabs
    /// </summary>
    private static string _mobTag = "Mob";

    /// <summary>
    /// Returns the name currently assigned to the Player
    /// </summary>
    /// <returns></returns>
    public static string GetPlayerName()
    {
        return _playerName;
    }

    /// <summary>
    /// Returns the tag currently assigned to Pickup prefab instances
    /// </summary>
    /// <returns></returns>
    public static string GetPickupTag()
    {
        return _pickupTag;
    }

    /// <summary>
    /// Returns the tag currently assigned to Mob prefab instances
    /// </summary>
    /// <returns></returns>
    public static string GetMobTag()
    {
        return _mobTag;
    }
        
}
