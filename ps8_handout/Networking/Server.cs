// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
// <date> [November 7, 2024] </date>

using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace CS3500.Networking;

/// <summary>
///   Represents a server task that waits for connections on a given
///   port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{
	/// <summary>
	///   Wait on a TcpListener for new connections. Alert the main program
	///   via a callback (delegate) mechanism.
	/// </summary>
	/// <param name="handleConnect">
	///   Handler for what the user wants to do when a connection is made.
	///   This should be run asynchronously via a new thread.
	/// </param>
	/// <param name="port"> The port (e.g., 11000) to listen on. </param>
	public static void StartServer(Action<NetworkConnection> handleConnect, int port)
	{
		TcpListener listener = new(IPAddress.Any, port);
		listener.Start();
		Console.WriteLine("Server started");

		//Intialize Connections
		while (true)
		{
			TcpClient client;
			client = listener.AcceptTcpClient();
			NetworkConnection newConnection = new NetworkConnection(client);
			Console.WriteLine("accepted a connection");
			new Thread(() => handleConnect(newConnection)).Start();
		}

	}
}
