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
            cmd.CommandText = @"INSERT INTO bands (band) VALUES (@band);";

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
              string band = rdr.GetString(1);
              Band newBand = new Band(band, bandId);
              allBands.Add(newBand);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBands;
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
