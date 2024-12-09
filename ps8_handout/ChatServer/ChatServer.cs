// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Natalie DeSimone and Rishabh Saini] </authors>
// <date> [November 7, 2024] </date>

using CS3500.Networking;
using System.Text;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    private static readonly List<NetworkConnection> clients = new();
    private static readonly object clientLock = new();

    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    /// <returns> A Task. Not really used. </returns>
    private static void Main(string[] args)
    {
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // don't stop the program.
    }



    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect(NetworkConnection connection)
    {
        string? userName = null;

        //add new client to list of clients
        lock (clientLock)
        {
            clients.Add(connection);
        }

        // handle all messages until disconnect.
        try
        {
            while (true)
            {
                var message = connection.ReadLine();

                if (userName == null)
                {
                    // first message as the username
                    userName = message;
                    Console.WriteLine(userName + " has joined the chat.");
                }
                else
                {
                    // Broadcast the messages to other clients
                    Broadcast(userName + " : " + message);
                    Console.WriteLine(userName + " : " + message);
                }

            }
        }
        catch (Exception)
        {
            //broadcast that a user disconnected
            Broadcast(userName + " has disconnected");
            Console.WriteLine(userName + " has disconnected");
        }
        finally
        {
            //Get rid of connection
            lock (clientLock)
            {
                clients.Remove(connection);
            }
            connection.Disconnect();
            connection.Dispose();
        }
    }

    /// <summary>
    ///   <pre>
    ///     when message is sent, broadcasts to all clients, including themselves
    ///   </pre>
    /// </summary>
    ///
    private static void Broadcast(string message)
    {
        lock (clientLock)
        {

            List<NetworkConnection> ClientsToDelete = new();
            foreach (var client in clients)
            {
                try
                {
                    client.Send(message);
                }
                catch (Exception)
                {
                    ClientsToDelete.Add(client);
                    continue;
                }
            }

            foreach (var client in ClientsToDelete)
            {
                if (clients.Contains(client))
                    clients.Remove(client);
            }
        }
    }
}