using System.Collections.Generic;
using MySql.Data.MySqlClient;
using BandTracker;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BandTracker.Models
{
    public class Event
    {
        private int _venueId;
        private int _bandId;
        private int _id;

        public Event(int venueId, int bandId, int id=0)
        {
            _venueId = venueId;
            _bandId = bandId;
            _id = id;
        }

        public int GetEventId()
        {
            return _id;
        }
        public int GetBandId()
        {
            return _bandId;
        }
        public int GetVenueId()
        {
            return _venueId;
        }

        public override bool Equals(System.Object otherEvent)
        {
          if (!(otherEvent is Event))
          {
            return false;
          }
          else
          {
             Event newEvent = (Event) otherEvent;
             bool idEquality = this.GetEventId() == newEvent.GetEventId();
             bool idBandEquality = this.GetBandId() == newEvent.GetBandId();
             bool idVenueEquality = this.GetVenueId() == newEvent.GetVenueId();
             return (idEquality && idBandEquality && idVenueEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetEventId().GetHashCode();
        }


        public void DeleteEvent()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM events WHERE event_id = @EventID;";

            MySqlParameter eventIdParameter = new MySqlParameter();
            eventIdParameter.ParameterName = "@EventId";
            eventIdParameter.Value = _id;
            cmd.Parameters.Add(eventIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
              conn.Close();
            }

            conn.Dispose();
        }

        public static Event Find(int eventId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM events WHERE event_id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = eventId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            int VenueId = 0;
            int BandId = 0;

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              VenueId = rdr.GetInt32(1);
              BandId = rdr.GetInt32(2);
            }
            Event newEvent= new Event(VenueId, BandId, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newEvent;
        }
    }
}
