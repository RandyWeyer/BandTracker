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


    }
}
