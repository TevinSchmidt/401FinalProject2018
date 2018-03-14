using ClientApplicationMVC.Models;

using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;
using System.Web;
using System.Web.Mvc;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/Authentication/*
    /// </summary>
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// The default method for this controller
        /// </summary>
        /// <returns>The login page</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Please enter your username and password.";
            return View("Index");
        }

        public ActionResult LogIn(string username, string password)
        {

            LogInRequest request = new LogInRequest(username, password);

            ServiceBusResponse response;
            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                response = ConnectionManager.sendLogIn(request);
            }
            else
            {
                response = connection.sendLogIn(request);
            }
            if (response.result)
            {
                ViewBag.LoginResponse = "Login Successful";
            }else
            {
                ViewBag.LoginResponse = "Invalid Username or Password";
            }
            
            return View("Index");

        }

        public ActionResult CreateAccount()
        {
            

            return View("CreateAccount");
        }

        public ActionResult CreationConfirmationPage(string username, string email, string password,
            string address, string phonenumber, AccountType users)
        {
            CreateAccount account = new CreateAccount();
            account.username = username;
            account.password = password;
            account.address = address;
            account.phonenumber = phonenumber;
            account.email = email;
            account.type = users;
            CreateAccountRequest request = new CreateAccountRequest(account);
            ServiceBusResponse response;
            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if(connection == null)
            {
                response = ConnectionManager.sendNewAccountInfo(request);
            } else
            {
                response = connection.sendNewAccountInfo(request);
            }
            if (response.result)
            {
                return View("Index");
            } else
            {
                ViewBag.CreationMessage = response.response;
                return View("CreateAccount");
            }
            

            
        }
		
		//This class is incomplete and should be completed by the students in milestone 2
		//Hint: You will need to make use of the ServiceBusConnection class. See EchoController.cs for an example.
    }
}