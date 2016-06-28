using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommunicateWithServer();
        }

        private static void CommunicateWithServer()
        {
            Console.WriteLine("Client CLI started!\n");

            int port = 5400;
            String serverIP = "127.0.0.1";
            try
            {
                IFormatter binaryFormatter = new BinaryFormatter();

                string command;
                string[] commandSplitted;
                do
                {
                    Console.Write(">>");
                    command = Console.ReadLine();
                    commandSplitted = command.Split(',');
                    List<string> list;

                    using (TcpClient client = new TcpClient(serverIP, port))
                    {
                        using (NetworkStream clientStream = client.GetStream())
                        {
                            binaryFormatter.Serialize(clientStream, commandSplitted); //Client -> Server (Serialize object to a given stream)
                            list = (List<string>)binaryFormatter.Deserialize(clientStream);//Server -> Client (Deserialize object from a given stream)
                        }
                    }

                    //Print retrieved list
                    foreach (string item in list)
                    {
                        Console.WriteLine(item);
                    }
                } while (!command.Equals("exit"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error communicating with server at: {0}. Exception: {1}", serverIP, e.Message);
            }
        }
    }
}