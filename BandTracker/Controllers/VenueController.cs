using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BandTracker.Models;
using System;

namespace BandTracker.Controllers
{
    public class VenuesController : Controller
    {
        [HttpGet("/venues")]
        public ActionResult Index()
        {
            List<Venue> allVenues = Venue.GetAll();
            return View(allVenues);
        }
        [HttpGet("/venues/new")]
        public ActionResult CreateForm()
        {
            return View();
        }
        [HttpPost("/create-venue")]
        public ActionResult Create()
        {
            Venue newVenue = new Venue(Request.Form["venue-name"]);
            newVenue.Save();
            return View("Success", "Home");
        }

        [HttpGet("/venues/{id}")]
        public ActionResult ViewVenues(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Venue selectedVenue = Venue.Find(id);
            List<Band> events = selectedVenue.GetBands();
            List<Band> allBands = Band.GetAll();
            model.Add("selectedVenue", selectedVenue);
            model.Add("events", events);
            model.Add("allBands", allBands);
            return View(model);
        }

        [HttpPost("/venues/{bandId}/bands/new")]
        public ActionResult AddBandToVenue(int bandId)
        {
            Venue venue = Venue.Find(bandId);
            Band band = Band.Find(Int32.Parse(Request.Form["band-id"]));
            venue.SetBands(band);
            return RedirectToAction("ViewVenues",  new { id = bandId });
        }

        [HttpGet("/venues/{id}/delete")]
        public ActionResult Delete(int id)
        {
            Event selectedEvent = Event.Find(id);
            int venueCityId = selectedEvent.GetVenueId();
            selectedEvent.DeleteEvent();
            return RedirectToAction("ViewVenues",  new { id = venueCityId });


        }
    }
}
