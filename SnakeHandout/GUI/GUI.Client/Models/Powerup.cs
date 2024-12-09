/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>
namespace GUI.Client.Models
{
    /// <summary>
    /// Model for powerups
    /// </summary>
    public class Powerup
    {
        /// <summary>
        /// get and sets the power of powerup
        /// </summary>
        public int power { get; set; }

        /// <summary>
        /// Get/sets location of powerup
        /// </summary>
        public Point2D loc { get; set; }

        /// <summary>
        /// gets and sets the value of died for powerup
        /// </summary>
        public bool died { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public Powerup()
        {
            loc = new Point2D();
        }
	}
}
