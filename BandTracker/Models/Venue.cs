using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Venue
    {
        private string _venue_Name;
        private int _id;

        public Venue(string venueName, int id=0)
        {
            _venue_Name = venueName;
            _id = id;
        }

        public string GetVenueName()
        {
            return _venue_Name;
        }
        public void SetVenueName(string VenueName)
        {
            _venue_Name = VenueName;
        }

        public int GetId()
        {
            return _id;
        }


        public override bool Equals(System.Object otherVenue)
        {
          if (!(otherVenue is Venue))
          {
            return false;
          }
          else
          {
             Venue newVenue = (Venue) otherVenue;
             bool idEquality = this.GetId() == newVenue.GetId();
             bool nameEquality = this.GetVenueName() == newVenue.GetVenueName();
             return (idEquality && nameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetVenueName().GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO venues (venue_name) VALUES (@venue);";

            MySqlParameter venue = new MySqlParameter();
            venue.ParameterName = "@venue";
            venue.Value = this._venue_Name;
            cmd.Parameters.Add(venue);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM venues;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int id = rdr.GetInt32(0);
              string venue = rdr.GetString(1);
              Venue newVenue = new Venue(venue, id);
              allVenues.Add(newVenue);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allVenues;
        }

        public static Venue Find(int venueId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = venueId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string venueName = "";

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              venueName = rdr.GetString(1);
            }
            Venue newVenue = new Venue(venueName, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newVenue;
        }

        public List<Event> GetEvents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            cmd.CommandText = @"SELECT * FROM events WHERE venue_id = @ThisEventId;";


            MySqlParameter venueIdParameter = new MySqlParameter();
            venueIdParameter.ParameterName = "@ThisEventId";
            venueIdParameter.Value = _id;
            cmd.Parameters.Add(venueIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Event> Events = new List<Event>{};

            while(rdr.Read())
            {
              int eventId = rdr.GetInt32(0);
              int venueId = rdr.GetInt32(1);
              int bandId = rdr.GetInt32(2);
              Event newEvent = new Event(eventId, venueId, bandId);
              Events.Add(newEvent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return Events;
        }

        public List<Band> GetBands()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT bands.* FROM venues
                JOIN events ON (venues.id = events.venue_id)
                JOIN bands ON (events.band_id = bands.id)
                WHERE venues.id = @BandId;";

            MySqlParameter venueIdParameter = new MySqlParameter();
            venueIdParameter.ParameterName = "@BandId";
            venueIdParameter.Value = _id;
            cmd.Parameters.Add(venueIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Band> bands = new List<Band>{};

            while(rdr.Read())
            {
              int BandId = rdr.GetInt32(0);
              string BandName = rdr.GetString(1);
              Band newBand = new Band(BandName, BandId);
              bands.Add(newBand);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return bands;
        }


        public void SetBands(Band newBand)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO events (venue_id, band_id) VALUES (@VenueId, @BandId);";

            MySqlParameter band_id = new MySqlParameter();
            band_id.ParameterName = "@BandId";
            band_id.Value = newBand.GetId();
            cmd.Parameters.Add(band_id);

            MySqlParameter venue_id = new MySqlParameter();
            venue_id.ParameterName = "@VenueId";
            venue_id.Value = _id;
            cmd.Parameters.Add(venue_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM venues;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
