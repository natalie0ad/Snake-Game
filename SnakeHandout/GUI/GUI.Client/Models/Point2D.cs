/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>

namespace GUI.Client.Models
{
    /// <summary>
    /// Point2D used to assign the location of a wall or powerup
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// Get/set value of X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Get/set value of Y
        /// </summary>
        public int Y { get; set; }
        
        /// <summary>
        /// Intial constructor
        /// </summary>
        public Point2D()
        {
        }
    }
}
