
﻿using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.NServiceBus.Commands;
using MySql.Data.MySqlClient;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.ServiceBusRequest.Chat.Responses;

namespace ChatService.Database
{

    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>

    public partial class ChatServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private ChatServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static ChatServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new ChatServiceDatabase();
            }
            return instance;
        }

        /// <summary>
        ///Sends a new message to the database
        /// </summary>
        /// <param name="message">Information about the message</param>
        public void saveMessage(SendMessageRequest message)
        {
            if(openConnection() == true)
            {
                string query = @"INSERT INTO " + dbname + @".chat(sender, receiver, message, timestamp)VALUES('" +
                    message.message.sender + @"', '" + message.message.receiver + @"', '" + message.message.messageContents +
                    @"', '" + message.message.unix_timestamp + @"');";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
        }

        /// <summary>
        /// Gets the contacts for a user from DB.
        /// </summary>
        /// <param name="request"></param>
        public GetChatContactsResponse getChatContacts(GetChatContactsRequest request)
        {
            bool result = false;
            string message = "";
            GetChatContacts chatContacts = new GetChatContacts();
            if (openConnection() == true)
            {
                string query = @"SELECT RECEIVER FROM " + dbname + @".CHAT WHERE SENDER = '" + request.getCommand.usersname +
                    @"';";
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        chatContacts.contactNames.Add(reader.GetString("RECEIVER"));

                    }
                    reader.Close();
                    result = true;


                } catch (MySqlException e)
                {
                    message = e.Message;
                } catch (Exception e){
                    Messages.Debug.consoleMsg("Unable to complete select from chat contacts database." +
                        " Error :" +  e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    chatContacts.contactNames.Add(e.Message);
                    message = e.Message;
                } finally
                {
                    closeConnection();
                }
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
                message = "Unable to connect to database";
            }
            return new GetChatContactsResponse(result, message, chatContacts);
        }


        /// <summary>
        /// Obtains the chat history for a particular conversation pair.
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public GetChatHistory getChatHistory(GetChatHistoryRequest request)
        {
            if (openConnection() == true)
            {
                GetChatHistory ChatHistory = new GetChatHistory();
                string query = "";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                closeConnection();
                return ChatHistory;
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
            return null;
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class ChatServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>

        private const String dbname = "chatserviceDB";

        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static ChatServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "chat",
                new Column[]
                {
                    new Column("sender", "VARCHAR(100)", new string[] {"NOT NULL"}, true),
                    new Column("receiver", "VARCHAR(100)", new string[] {"NOT NULL"}, true),
                    new Column("message", "VARCHAR(1000)", new string[]{"NOT NULL"}, false),
                    new Column("timestamp", "INT(64)", new string[] {"NOT NULL"}, false)

                }
            )
        };
    }
}
