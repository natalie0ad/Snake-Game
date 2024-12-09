/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>
using System.Text.Json.Serialization;
namespace GUI.Client.Models
{
    /// <summary>
    /// Handles all Walls
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// Gets/Sets wallID
        /// </summary>
        public int wall { get; set; }

        /// <summary>
        /// Gets/Sets p1 of a wall
        /// </summary>
        public Point2D p1 { get; set; }

        /// <summary>
        /// gets/sets position 2 of a wall
        /// </summary>
        public Point2D p2 { get; set; }

        /// <summary>
        /// empty intailizes wall
        /// </summary>
        public Wall()   
        {
            p1 = new Point2D();
            p2 = new Point2D();
        }

        /// <summary>
        /// Checks if the Wall is aligned along the X-axis.
        /// </summary>
        /// <returns>True if the Wall is aligned along the X-axis, false otherwise.</returns>
        public bool IsXAligned()
        {
            return p1.Y == p2.Y;
        }

        /// <summary>
        /// Checks if the Wall is aligned along the Y-axis.
        /// </summary>
        /// <returns>True if the Wall is aligned along the Y-axis, false otherwise.</returns>
        public bool IsYAligned()
        {
            return p1.X == p2.X;
        }
    }
}
