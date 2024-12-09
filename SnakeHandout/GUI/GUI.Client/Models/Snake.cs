/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>
using System.Text.Json.Serialization;
namespace GUI.Client.Models
{
    /// <summary>
    /// Responible for creating snake 
    /// </summary>
    public class Snake
    {

        /// <summary>
        /// Gets and sets the integer snake id.
        /// </summary>
        [JsonInclude]
        public int snake {  get; set; }

        /// <summary>
        /// Gets and sets name of snake
        /// </summary>
        [JsonInclude]
        public string name { get; set; }

        /// <summary>
        /// Gets and sets the Point2D list of the snake's body
        /// </summary>
        public List<Point2D> body { get; set; }

        /// <summary>
        /// Gets and sets the direction of the snake
        /// </summary>
        [JsonInclude]
        public Point2D dir { get; set; }

        /// <summary>
        /// Gets and sets the score of the snake
        /// </summary>
        [JsonInclude]
        public int score { get; set; }

        /// <summary>
        /// Gets and sets of the snake has died
        /// </summary>
        [JsonInclude]
        public bool died { get; set; }

        /// <summary>
        /// Gets and sets if the Snake is alive
        /// </summary>
        [JsonInclude]
        public bool alive { get; set; }

        /// <summary>
        /// Gets and sets the direction the snake is moving in
        /// </summary>
        [JsonInclude]
        public bool dc { get; set; }

        /// <summary>
        /// keeps track of if snake has joined
        /// </summary>
        [JsonInclude]
        public bool joined { get; set; }

		/// <summary>
		/// Empty constructor for the Snake object.
		/// </summary>
		public Snake()  {
            name = string.Empty;
            body = new List<Point2D>();
            dir = new Point2D();

		}
    }
}
