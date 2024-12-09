/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [November 24, 2024] </date>
using System.Text.Json.Serialization;
namespace GUI.Client.Models
{
    /// <summary>
    /// Handles moving the snake in a direction
    /// </summary>
    public class ControlCommands
    {
        /// <summary>
        /// Keeps track if its currently moving
        /// </summary>
        [JsonInclude]
        private string moving { get; set; }

        /// <summary>
        /// Sets moving to none.
        /// </summary>
        public ControlCommands()
        {
            moving = "none";
        }
        /// <summary>
        /// Sets the snake is moving in the received direction
        /// </summary>
        /// <param name="direction"> The string representing the direction the snake is moving in. 
        /// Expected to be in JSON representation. </param>
        public ControlCommands(string direction)
        {
            moving = direction;
        }
    }
}
