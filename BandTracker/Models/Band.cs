using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Band
    {
        private string _band;
        private int _id;

        public Band(string Band, int Id = 0)
        {
            _band = Band;
            _id = Id;
        }

        public int GetId()
        {
          return _id;
        }

        public string GetBand()
        {
          return _band;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO bands (band_name) VALUES (@band);";

            MySqlParameter band = new MySqlParameter();
            band.ParameterName = "@band";
            band.Value = this._band;
            cmd.Parameters.Add(band);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Band> GetAll()
        {
            List<Band> allBands = new List<Band> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int bandId = rdr.GetInt32(0);
              string bandName = rdr.GetString(1);
              Band newBand = new Band(bandName, bandId);
              allBands.Add(newBand);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBands;
        }

        public List<Venue> GetVenues()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT venues.* FROM bands
                JOIN events ON (bands.id = events.band_id)
                JOIN venues ON (events.venue_id = venues.id)
                WHERE bands.id = @BandId;";

            MySqlParameter bandIdParameter = new MySqlParameter();
            bandIdParameter.ParameterName = "@BandId";
            bandIdParameter.Value = _id;
            cmd.Parameters.Add(bandIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Venue> venues = new List<Venue>{};

            while(rdr.Read())
            {
              int VenueId = rdr.GetInt32(0);
              string VenueName = rdr.GetString(1);
              Venue newVenue = new Venue(VenueName, VenueId);
              venues.Add(newVenue);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return venues;
        }

        public void SetVenues(Venue newVenue)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO events (venue_id, band_id) VALUES (@VenueId, @BandId);";

            MySqlParameter venue_id = new MySqlParameter();
            venue_id.ParameterName = "@VenueId";
            venue_id.Value = newVenue.GetId();
            cmd.Parameters.Add(venue_id);

            MySqlParameter band_id = new MySqlParameter();
            band_id.ParameterName = "@BandId";
            band_id.Value = _id;
            cmd.Parameters.Add(band_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Band Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int bandId = 0;
            string band = "";

            while(rdr.Read())
            {
              bandId = rdr.GetInt32(0);
              band = rdr.GetString(1);
            }
            Band newBand = new Band(band, bandId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBand;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM bands;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

    }
}
