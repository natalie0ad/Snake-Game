/// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
/// <date> [December 5, 2024] </date>
using CS3500.Networking;
using GUI.Client.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebServer
{
	/// <summary>
	/// Reponsible for handling the http and the data base
	/// </summary>
	internal class WebServer
	{
		private const string httpOKRequest =
		 "HTTP/1.1 200 OK\r\n" +
		 "Connection: close\r\n" +
		"Content-Type: text/html; charset=UTF-8\r\n";

		private const string httpBadRequest =
		 "HTTP/1.1 400 Bad Request\r\n" +
		 "Connection: close\r\n" +
		"Content-Type: text/html; charset=UTF-8\r\n" +
		"\r\n";

		private static int GameID = 0;

		/// <summary>
		/// Starts page
		/// </summary>
		/// <param name="args"></param>
		public static void Main(String[] args)
		{
			Server.StartServer(HandleHttpConnection, 80);

			Console.Read();
		}

		/// <summary>
		/// Handles the HTTP connection
		/// </summary>
		/// <param name="client"></param>
		public static void HandleHttpConnection(NetworkConnection client)
		{
			try
			{
				string request = client.ReadLine();
				Console.WriteLine($"Request: {request}");

				if (string.IsNullOrEmpty(request))
				{
					SendHttpResponse(client, httpBadRequest, "Invalid Request");
					return;
				}

				//for home page
				if (request.StartsWith("GET / "))
				{
					SendHttpResponse(client, httpOKRequest, "<html>\r\n<h3>Welcome to the Snake Games Database!</h3>\r\n<a href=\"/games\">View Games</a>\r\n</html>");
				}
				//Page with all games
				else if (request.StartsWith("GET /games "))
				{
					 
					SendHttpResponse(client, httpOKRequest, CreatePage(request));
				}

				//find specific game
				else if (request.StartsWith("GET /games?gid" ))
				{
			
					SendHttpResponse(client, httpOKRequest, CreatePage(request));
				}

				else
				{
					SendHttpResponse(client, httpBadRequest, "Page not found.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex}");
				SendHttpResponse(client, httpBadRequest, "Internal server error.");
			}

		}

		/// <summary>
		/// Sends response to client
		/// </summary>
		/// <param name="client">client connected</param>
		/// <param name="header">http header</param>
		/// <param name="content">content/page being sent </param>
		private static void SendHttpResponse(NetworkConnection client, string header, string content)
		{
			string response = $"{header + "Content-Length: " + Encoding.UTF8.GetByteCount(content) +"\r\n" + "\r\n" }{content}";
			client.Send(response);
		}

		/// <summary>
		/// Creates Pages for all games, and specific games
		/// </summary>
		/// <returns></returns>
		private static string CreatePage(string request)
		{
			string Gamesheader = "<html><table border=\"1\"><thead><tr><td>ID</td><td>Start</td><td>End</td></tr></thead><tbody>";
			string Playersheader = "<html><table border=\"1\"><thead><tr><td>PlayerID</td><td>Name</td><td>Max Score</td><td>JoinTime</td><td>EndTime</td></tr></thead><tbody>";
			string footer = "</tbody></table></html>";
;			string table = string.Empty;
			string PlayersLabel = string.Empty;
			string GameLabel = string.Empty;
			StringBuilder body = new StringBuilder();

			// get game data from the database
			using (MySqlConnection conn = new MySqlConnection("server=atr.eng.utah.edu;" +
															  "database=u1462465;" +
	   															"uid=u1462465;" +
															 "password=changeThisPassword;"))
			{
				conn.Open();
				string GamesQuery = "SELECT `Game ID`, `Start Time`, `End Time` FROM Games";
				string PlayersQuery = "SELECT `Player ID`, `Name`, `Max Score`, `Enter Time`, `Leave Time`, `Game ID` FROM Players";

				//creates page with all games listed
				if (request.StartsWith("GET /games ")) {
					using (var command = new MySqlCommand(GamesQuery, conn))
					{
						using (MySqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								int id = reader.GetInt32(0);
								string startTime = reader.GetDateTime(1).ToString("yyyy-MM-dd H:mm:ss");

								string endTime = string.Empty;
								try
								{
									endTime = reader.GetDateTime(2).ToString("yyyy-MM-dd H:mm:ss");
								}
								catch (Exception e)
								{
									endTime = string.Empty;
								}
								GameLabel = $"\r\n<h3>Games Played</ h3 >\r\n";
								body.Append($"<tr><td><a href='/games?gid={id}'>{id}</a></td><td>{startTime}</td><td>{endTime}</td></tr>");
							}
						}
					}
					table = GameLabel + Gamesheader + body.ToString() + footer;
				}
				//creates specific game pages with player table
				else if (request.StartsWith("GET /games?gid"))
				{
					string regex = @"gid=(\d+)";
					Match match = Regex.Match(request, regex);
					int GameID = int.Parse(match.Groups[1].Value);

					using (var command = new MySqlCommand(PlayersQuery, conn))
					{
						using (MySqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								int id = reader.GetInt32(0);
								string name = reader.GetString(1);
								int maxScore = reader.GetInt32(2);
								string enterTime = reader.GetDateTime(3).ToString("yyyy-MM-dd H:mm:ss");

								string leaveTime = string.Empty;
								try
								{
									leaveTime = reader.GetDateTime(4).ToString("yyyy-MM-dd H:mm:ss");
								}
								catch (Exception e)
								{
									leaveTime = string.Empty;
								}

								int _GameID = reader.GetInt32(5);

								if (GameID != _GameID)
									continue;

								PlayersLabel = $"\r\n<h3> Stats for Game {_GameID}</ h3 >\r\n";
								body.Append($"<tr><td>{id}</td><td>{name}</td><td>{maxScore}</td><td>{enterTime}</td><td>{leaveTime}</td></tr>");
							}
						}
					}
					table = PlayersLabel + Playersheader + body.ToString() + footer;
				}
			}
			return table;
			
		}
	}
}