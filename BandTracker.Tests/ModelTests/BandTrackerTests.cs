using Microsoft.VisualStudio.TestTools.UnitTesting;
using BandTracker.Models;
using System;

namespace BandTracker.Tests
{
    [TestClass]
    public class BandTests : IDisposable
    {
        public void Dispose()
        {
            Band.DeleteAll();
        }
        public BandTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=;port=3306;database=band_tracker;";
        }

        [TestMethod]
        public void BandList_isEmpty()
        {
            int result = Band.GetAll().Count;

            Assert.AreEqual(0, result);
        }
    }

    [TestClass]
    public class VenueTests
    {
        public VenueTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=;port=3306;database=band_tracker;";
        }

        [TestMethod]
        public void Venue_returns_BlueCafe()
        {
            Venue result = Venue.Find(1);
            string finalResult = result.GetVenueName();

            Assert.AreEqual("Blues Cafe", finalResult);
        }
    }
}
