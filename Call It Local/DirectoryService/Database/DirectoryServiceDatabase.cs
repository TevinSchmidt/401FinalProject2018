using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryService.Database
{
    public partial class DirectoryServiceDatabase : AbstractDatabase
    {

        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private DirectoryServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static DirectoryServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new DirectoryServiceDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Saves the foreward echo to the database
        /// </summary>
        /// <param name="company">Information about the company</param>
        public ServiceBusResponse saveCompany(CompanyInstance company)
        {
            bool result = false;
            string message = "";

            if (openConnection() == true)
            {
                string query = @"INSERT INTO companies(companyname, phonenumber, email, location)" +
                    @"VALUES('" + company.companyName + @"', '" + company.phoneNumber + @"', '" +
                    company.email + @"', '" + company.locations[0] + @"');";
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete insert new company into database." +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to complete insert new user into database." +
                        " Error:" + e.Message);
                    message = e.Message;
                }
                finally
                {
                    closeConnection();
                }                
            }
            else
            {
                message = "Unable to connect to database";
            }

            return new ServiceBusResponse(result, message);
        }

        public CompanySearchResponse searchCompany(CompanySearchRequest company)
        {
            bool result = false;
            string message = "";
            CompanyList list = new CompanyList();

            string query = @"SELECT * FROM " + databaseName + @".companies WHERE companyname='" + 
                company.searchDeliminator + @"';";
            
            if (openConnection() == true)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = command.ExecuteReader();

                    List<string> temp = new List<string>();
                    while(dataReader.Read()) {
                        temp.Add(dataReader.GetString("companyname"));
                    }
                    dataReader.Close();
                    list.companyNames = temp.ToArray();
                    result = true;
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete select from company into database." +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to Unable to complete insert new user into database." +
                        " Error:" + e.Message);
                    message = e.Message;
                }
                finally
                {
                    closeConnection();
                }
            }
            else
            {
                message = "Unable to connect to database";
            }
            return new CompanySearchResponse(result, message, list);
        }

        public GetCompanyInfoResponse getCompany(GetCompanyInfoRequest request)
        {
            bool result = false;
            string message = "";
            CompanyInstance company = null;

            string query = @"SELECT * FROM " + databaseName + @".companies WHERE companyname='" +
                request.companyInfo.companyName + @"';";

            if (openConnection() == true)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = command.ExecuteReader();


                    if (dataReader.Read())
                    {

                        string companyname = dataReader.GetString("companyname");
                        string phonenumber = dataReader.GetString("phonenumber");
                        string email = dataReader.GetString("email");
                        string location = dataReader.GetString("location");

                        company = new CompanyInstance(companyname, phonenumber, email, new string[] { location });
                    }
                    else
                    {
                        throw new Exception("Reader cannot read. DirectoryServiceDatabase Error.");
                    }

                    dataReader.Close();
                    result = true;
                }
                catch (MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete select from company into database." +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to Unable to complete insert new user into database." +
                        " Error:" + e.Message);
                    message = e.Message;
                }
                finally
                {
                    closeConnection();
                }
            }
            else
            {
                message = "Unable to connect to database";
            }
            return new GetCompanyInfoResponse(result, message, company);
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class DirectoryServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "directory";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static DirectoryServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "companies",
                new Column[]
                {
                    new Column("id", "INT(64)", new string[] {"NOT NULL", "UNIQUE", "AUTO_INCREMENT"}, true),
                    new Column("companyname", "VARCHAR(100)", new string[] {"NOT NULL"}, false),
                    new Column("phonenumber", "VARCHAR(15)", new string[] {}, false),
                    new Column("email", "VARCHAR(50)", new string[]{"NOT NULL" }, false),
                    new Column("location", "VARCHAR(300)", new string[] {"NOT NULL"}, false)
                }
            )
        };
    }

}

