
/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [December 5, 2024] </date>
using CS3500.Networking;
using GUI.Client.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace GUI.Client.Controllers
{
	/// <summary>
	/// Controlls the information being received from server
	/// </summary>
	public class NetworkController
	{
		/// <summary>
		/// Establish new connection
		/// </summary>
		private NetworkConnection connection = new();

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, int> SnakesHighScores = new();

		/// <summary>
		/// The connection string.
		/// uID login name serves as both your database name and your uid
		/// </summary>
		private const string connectionString = "server=atr.eng.utah.edu;" +
		  "database=u1462465;" +
	   	"uid=u1462465;" +
		 "password=changeThisPassword;";

		/// <summary>
		/// Checks Client is connected to server
		/// </summary>
		public bool IsConnected
		{ 
			get { return connection.IsConnected; }
			private set { }
		}

		/// <summary>
		/// Handles the "handshake" between the client and the server.
		/// It sets the username of the player, the world size and adds
		/// the walls recieved from the server.
		/// </summary>
		/// <param name="host"> string representing host address</param>
		/// <param name="port"> int representing the port </param>
		/// <param name="username"> string representing username of player </param>
		/// <param name="PlayerID"> int representing id of player</param>
		/// <param name="size"> int representing size of world</param>
		/// <param name="walls"> the list of walls in the world</param>
		internal void Handshake(string host, int port, string username, ref int PlayerID, ref int size, List<Wall> walls)
		{
			SendSQLCommand("INSERT INTO Games (`Start Time`) VALUES ('" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "');");
			connection.Connect(host, port);

			SendMessageToServer(username);
            RecieveMessageFromServer(out string ID);
            PlayerID = int.Parse(ID);

            RecieveMessageFromServer(out string WorldSize);
            size = int.Parse(WorldSize);

            AddWalls(walls);
        }

		/// <summary>
		/// Disconnects the client from the server, updates database
		/// </summary>
		internal void Disconnect()
		{
			SnakesHighScores.Clear();
			connection.Disconnect();
			IsConnected = false;
			SendSQLCommand("UPDATE Games SET `End Time` = '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' WHERE `End Time` is null;");
			SendSQLCommand("UPDATE Players SET `Leave Time` = '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' WHERE `Leave Time` is null;");

		}
		/// <summary>
		/// Sends a message to the connected server
		/// </summary>
		/// <param name="message">message to be sent</param>
		public void SendMessageToServer(string message)
		{
			connection.Send(message);
		}
        /// <summary>
		/// Reads message from the connected server
		/// </summary>
		/// <returns></returns>
        public bool RecieveMessageFromServer(out string message)
        {
			try
			{
				message = connection.ReadLine();
				return true; 
			}
			catch (Exception)
			{
				message = string.Empty;
				return false;
			}

        }

		/// <summary>
		/// Deserializes JSON to find what kind of object it is
		/// </summary>
		/// <param name="world"> The world object to add the deserialized objects into </param>
        internal void NetworkLoop(World world)
        {
            while (true && connection.IsConnected)
            {
                RecieveMessageFromServer(out string JsonString);

                lock (world)
                {
					// checks if snake
					if (JsonString.Contains("snake"))
					{
						Snake? snake = JsonSerializer.Deserialize<Snake>(JsonString);
						if (snake!.dc)
						{
							world.snakes.Remove(snake.snake);
							//sets the leave time correspondiong to correct snake
							SendSQLCommand("UPDATE Players SET `Leave Time` = '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' WHERE`Player ID` = " + snake.snake + ";");
						}

						else
						{
							if(!world.snakes.ContainsKey(snake.snake))
							{
								SendSQLCommand("INSERT INTO Players (`Player ID`, `Name`, `Max Score`, `Enter Time`, `Game ID`) VALUES (" + snake.snake + ",'" + snake.name + "'," + snake.score + ",'" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'," + GetLastGameID() + ");");
							}

							if(!SnakesHighScores.ContainsKey(snake.snake))
							{
								SnakesHighScores[snake.snake] = snake.score;
							}
							
							world.snakes[snake.snake] = snake;

							if (snake.score > SnakesHighScores[snake.snake])
							{
								SnakesHighScores[snake.snake] = snake.score;
								SendSQLCommand("UPDATE Players SET `Max Score` = " + SnakesHighScores[snake.snake] + " WHERE `Player ID` = " + snake.snake + ";");
							}
						}
                    }

					//checks if powerup
                    else if (JsonString.Contains("power"))
                    {
                        Powerup? powerup = JsonSerializer.Deserialize<Powerup>(JsonString);
                        if (powerup!.died)
                            world.powerups.Remove(powerup.power);
                        else
                            world.powerups[powerup.power] = powerup;
                    }
                }
            }
        }

		/// <summary>
		/// If json is wall, adds wall to list of walls
		/// </summary>
		/// <param name="walls"> The list of walls to add the deserialized walls into </param>
		private void AddWalls(List<Wall> walls)
		{
			while (true)
			{
				RecieveMessageFromServer(out string JsonWall);
				
				if (JsonWall.Contains("wall"))
				{
                    Wall? wall = JsonSerializer.Deserialize<Wall>(JsonWall);
                    walls.Add(wall!);
				}
				else
					break;
			}
        }
        /// <summary>
        /// If user presses 'w' creates and sends command to move snake up 
        /// </summary>
        public void Up()
		{
			ControlCommands up = new ControlCommands("up");
			connection.Send(JsonSerializer.Serialize(up));
        }

        /// <summary>
        /// If user presses 'a' creates and sends command to move snake left
		/// </summary>
        public void Left()
		{
            ControlCommands left = new ControlCommands("left");
            connection.Send(JsonSerializer.Serialize(left));
        }

        /// <summary>
        /// If user presses 's' creates and sends command to move snake down
        /// </summary>
        public void Down()
		{
            ControlCommands down = new ControlCommands("down");
            connection.Send(JsonSerializer.Serialize(down));
        }

        /// <summary>
        /// If user presses 'd' creates and sends command to move snake right
        /// </summary>
        public void Right()
		{
            ControlCommands right = new ControlCommands("right");
            connection.Send(JsonSerializer.Serialize(right));
        }

		/// <summary>
		/// Sends an SQL query to the connect database.
		/// </summary>
		/// <param name="SqlCommand">String representing the SQL query.</param>
		private void SendSQLCommand(string SqlCommand)
		{
			// Connect to the DB
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					// Open a connection
					conn.Open();

					// Create a command
					MySqlCommand command = conn.CreateCommand();
					command.CommandText = SqlCommand;
					command.ExecuteNonQuery();

				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}

		/// <summary>
		/// Gets the Game ID of the last inserted game in the MySQL database.
		/// </summary>
		/// <returns>An integer representing the last played game.</returns>
		private int GetLastGameID()
		{
			// Connect to the DB
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				try
				{
					// Open a connection
					conn.Open();

					// Create a command
					MySqlCommand command = conn.CreateCommand();
					command.CommandText = "select last_insert_id();";
					using(MySqlDataReader reader = command.ExecuteReader())
					{
						if(reader.Read())
							return reader.GetInt32(0);

                        else
							return -1;
                    }
				}
				catch (Exception e)
				{
					return -1;
				}
			}
		}
	}
}
