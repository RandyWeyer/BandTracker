using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class BandsController : Controller
    {
        [HttpGet("/bands")]
        public ActionResult Index()
        {
            List<Band> allBands = Band.GetAll();
            return View(allBands);
        }
        [HttpGet("/bands/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-band")]
        public ActionResult Create()
        {
            Band newBand = new Band(Request.Form["band-name"]);
            newBand.Save();
            return View("Success", "Home");
        }
        [HttpGet("/bands/{id}")]
        public ActionResult ViewBands(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Band selectedBand = Band.Find(id);
            List<Venue> events = selectedBand.GetVenues();
            List<Venue> allVenues = Venue.GetAll();
            model.Add("selectedBand", selectedBand);
            model.Add("events", events);
            model.Add("allVenues", allVenues);
            return View(model);
        }
        [HttpPost("/bands/{venueId}/venues/new")]
        public ActionResult AddVenueToBand(int venueId)
        {
            Band band = Band.Find(venueId);
            Venue venue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
            band.SetVenues(venue); //Want to run the join table method
            return RedirectToAction("ViewBands",  new { id = venueId });
        }
    }
}
