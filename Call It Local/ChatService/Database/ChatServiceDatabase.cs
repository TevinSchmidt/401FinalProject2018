using Messages;
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
                string query = "";

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
        public GetChatContacts getChatContacts(GetChatContactsRequest request)
        {
            if (openConnection() == true)
            {
                GetChatContacts chatContacts = new GetChatContacts();
                string query = "";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                
                closeConnection();
                return chatContacts;
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
            return null;
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
                "echoforward",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(64)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "username", "VARCHAR(50)",
                        new string[] {},
                        false
                    ),
                    new Column
                    (
                        "datain", "VARCHAR(" + SharedData.MAX_MESSAGE_LENGTH.ToString() + ")",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                }
            ),
            new Table
            (
                dbname,
                "echoreverse",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(64)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "username", "VARCHAR(50)",
                        new string[] 
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "datain", "VARCHAR(" + SharedData.MAX_MESSAGE_LENGTH.ToString() + ")",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                }
            )
        };
    }
}
