/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>
using System.Text.Json.Serialization;
namespace GUI.Client.Models

{
    /// <summary>
    /// Creates world
    /// </summary>
    public class World
    {
        /// <summary>
        /// Gets/sets size of the world
        /// </summary>
        [JsonInclude]
        public int size { get; set; }

        /// <summary>
        /// Dictionary to keep track of snakes and their IDs
        /// </summary>
        [JsonInclude]
        public Dictionary<int, Snake> snakes { get; set; }

        /// <summary>
        /// Dictionary tp keep track of walls and their IDs
        /// </summary>
        [JsonInclude]
        public Dictionary<int, Wall> walls { get; set; }

        /// <summary>
        /// Dictionary to keep track if powerups and their ids
        /// </summary>
        [JsonInclude]
        public Dictionary<int, Powerup> powerups { get; set; }

        /// <summary>
        /// Intializes thr needed dicitonaries for a world
        /// </summary>
        public World()
        {
            snakes = new Dictionary<int, Snake>();
            walls = new Dictionary<int, Wall>();
            powerups = new Dictionary<int, Powerup>();
        }

        /// <summary>
        /// Intializes a world
        /// </summary>
        /// <param name="other"></param>
        public World(World other)
        {
            snakes = other.snakes;
            walls = other.walls;
            powerups = other.powerups;
        }
     }
}
