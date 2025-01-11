using System;
using System.Collections.Generic;
using System.Text;

namespace ADSBLib {

    /*****************************************************************
    * ADSBData
    * 
    * Container for ADS-B packets
    * 
    * **************************************************************/
    public class ADSBData {

        public string uid;
        public DateTime msgDateTime;
        public string callsign;
        public float latitude;
        public float longitude;
        public float speed;
        public float altitude;

        enum ADSBStringParts {
            UID = 4,
            MSG_DATE = 6,
            MSG_TIME = 7,
            CURR_DATE = 8,
            CURR_TIME = 9,
            CALLSIGN = 10,
            ALTITUDE = 11,
            SPEED = 12,
            HEADING = 13,
            LATITUDE = 14,
            LONGITUDE = 15,
            VRATE = 16,
            SQUAWK = 17,
            SQUAWK_ALERT = 18,
            EMERGENCY = 19,
            IDENT = 20,
            GROUNDED = 21
        }

        /*****************************************************************
        * ADSBData
        * 
        * Constructor for a dump1090 TCP 30003 string
        * 
        * **************************************************************/
        public ADSBData(string line) {
            string[] parts = line.Split(',');

            this.uid = parts[(int)ADSBStringParts.UID];
            this.callsign = parts[(int)ADSBStringParts.CALLSIGN];

            string date = parts[(int)ADSBStringParts.MSG_DATE];
            string time = parts[(int)ADSBStringParts.MSG_TIME];
            this.msgDateTime = DateTime.Parse(date + " " + time);

            if(!string.IsNullOrWhiteSpace(parts[(int)ADSBStringParts.ALTITUDE])) {
                this.altitude = float.Parse(parts[(int)ADSBStringParts.ALTITUDE]);
            } else {
                this.altitude = float.NaN;
            }

            if(!string.IsNullOrWhiteSpace(parts[(int)ADSBStringParts.LATITUDE])) {
                this.latitude = float.Parse(parts[(int)ADSBStringParts.LATITUDE]);
            } else {
                this.latitude = float.NaN;
            }

            if(!string.IsNullOrWhiteSpace(parts[(int)ADSBStringParts.LONGITUDE])) {
                this.longitude = float.Parse(parts[(int)ADSBStringParts.LONGITUDE]);
            } else {
                this.longitude = float.NaN;
            }

            if(!string.IsNullOrWhiteSpace(parts[(int)ADSBStringParts.SPEED])) {
                this.speed = float.Parse(parts[(int)ADSBStringParts.SPEED]);
            } else {
                this.speed = float.NaN;
            }
        }

        /*****************************************************************
        * Merge
        * 
        * Updates self data with other data
        * 
        * **************************************************************/
        public void Merge(ADSBData other) {
            bool didMerge = false;

            if(!float.IsNaN(other.altitude)) {
                this.altitude = other.altitude;
                didMerge = true;
            }
            if(!float.IsNaN(other.latitude)) {
                this.latitude = other.latitude;
                didMerge = true;
            }
            if(!float.IsNaN(other.longitude)) {
                this.longitude = other.longitude;
                didMerge = true;
            }
            if(!float.IsNaN(other.speed)) {
                this.speed = other.speed;
                didMerge = true;
            }
            if(!string.IsNullOrWhiteSpace(other.callsign)) {
                this.callsign = other.callsign;
                didMerge = true;
            }
            if(didMerge) {
                this.msgDateTime = other.msgDateTime;
            }
        }

        /*****************************************************************
        * IsComplete
        * 
        * Has 3D location and datetime
        * 
        * **************************************************************/
        public bool IsComplete() {
            return
                    !float.IsNaN(latitude) &&
                    !float.IsNaN(longitude) &&
                    !float.IsNaN(altitude) &&
                    msgDateTime != null;
        }

        /*****************************************************************
        * Readout
        * 
        * Converts ADSBData to a CSV-format string
        * 
        * **************************************************************/
        public string Readout() {
            StringBuilder sb = new StringBuilder();
            if(!string.IsNullOrWhiteSpace(uid)) { sb.Append(uid); }
            sb.Append(',');
            if(!string.IsNullOrWhiteSpace(callsign)) { sb.Append(callsign); } 
            sb.Append(',');
            if(!float.IsNaN(latitude)) { sb.Append(latitude.ToString()); }
            sb.Append(',');
            if(!float.IsNaN(longitude)) { sb.Append(longitude.ToString()); }
            sb.Append(',');
            if(!float.IsNaN(altitude)) { sb.Append(altitude.ToString()); }
            sb.Append(',');
            if(!float.IsNaN(speed)) { sb.Append(speed.ToString()); }
            sb.Append(',');
            sb.Append(msgDateTime.ToString());
            return sb.ToString();
        }
    }
}
