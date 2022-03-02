using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter SSH-Address: ");
            string? AddressString = Console.ReadLine();
            Console.Write("Enter Username: ");
            string? UserNameString = Console.ReadLine();
            Console.Write("Enter Password: ");

            string PasswordString = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if(key == ConsoleKey.Backspace && PasswordString.Length > 0)
                {
                    Console.Write("\b \b");
                    PasswordString = PasswordString.Remove(PasswordString.Length - 1);
                }
                else if(!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write('*');
                    PasswordString+=keyInfo.KeyChar;
                }
            }while(key != ConsoleKey.Enter);


            IPAddress? Address;
            
            if (!IPAddress.TryParse(AddressString, out Address))
                throw new Exception("Invalid IPAddr");
            if (Address == null)
                throw new Exception("Invalid IPAddr");
            if (UserNameString == null)
                throw new Exception("Invalid Username");

            using(var client = new SshClient(Address.ToString(), UserNameString, PasswordString))
            {
                try
                {
                    client.Connect();
                }
                catch(Exception ex)
                {
                    throw new Exception("Connection Failed!\n" + ex.Message);
                }
                if (!client.IsConnected)
                    throw new Exception("Couldn't connect to Target");

                client.RunCommand("");
                client.Disconnect();
            }
        }
    }
}