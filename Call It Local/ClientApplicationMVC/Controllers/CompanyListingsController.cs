﻿using ClientApplicationMVC.Models;

using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;


using System;
using System.Web.Mvc;
using System.Web.Routing;
using Messages.ServiceBusRequest.CompanyReviews;
using Messages.ServiceBusRequest.CompanyReviews.Requests;
using Messages.DataTypes.Database.CompanyReview;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/CompanyListings/*
    /// </summary>
    public class CompanyListingsController : Controller
    {
        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Index()
        {
            if (Globals.isLoggedIn())
            {
                ViewBag.Companylist = null;
                return View("Index");
            }
            return RedirectToAction("Index", "Authentication");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/Search
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Search(string textCompanyName)
        {

            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if(connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            CompanySearchRequest request = new CompanySearchRequest(textCompanyName);

            CompanySearchResponse response = connection.searchCompanyByName(request);
            if (response.result == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.Companylist = response.list;

            return View("Index");
        }

        public ActionResult Review(string companyReview, string companyStars)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            DateTime now = DateTime.Now;
            AddCompanyReviewRequest request = new AddCompanyReviewRequest(new Review(ViewBag.CompanyName, companyReview, companyStars, now.ToString(), Globals.getUser()));
            ServiceBusResponse response = connection.addCompanyReview(request);
            
            //Can check if result is true here or just redirect to displaycompany and reload page

            return RedirectToAction("DisplayCompany");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/DisplayCompany/*info*
        /// </summary>
        /// <param name="id">The name of the company whos info is to be displayed</param>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult DisplayCompany(string id)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(id))
            {
                return View("Index");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.CompanyName = id;

            GetCompanyInfoRequest infoRequest = new GetCompanyInfoRequest(new CompanyInstance(id));
            GetCompanyInfoResponse infoResponse = connection.getCompanyInfo(infoRequest);
            ViewBag.CompanyInfo = infoResponse.companyInfo;

            CompanyReviewSearchRequest reviewRequest = new CompanyReviewSearchRequest(id);
            CompanyReviewResponse reviewResponse = connection.searchCompanyReview(reviewRequest);

            ViewBag.Reviewlist = reviewResponse.reviews;
            return View("DisplayCompany");
        }
    }
}  