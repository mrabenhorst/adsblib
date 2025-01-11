using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADSBLib {

    /*****************************************************************
    * Collector
    * 
    * Class that collects the dump1090 lines into useful ADSBData 
    * 
    * **************************************************************/
    public class Collector {

        private int staleTime = 60 * 5;

        public List<ADSBData> archive { get; private set; } // Old data that has been superceeded (old completed data)
        public Dictionary<string, ADSBData> current; // latest data (complete set)
        private Dictionary<string, ADSBData> holding; // data that is still waiting to be completed (partial set)

        private Action<ADSBData> holdingPostback;
        private Action<ADSBData> currentPostback;

        /*****************************************************************
        * Collector
        * 
        * Constructor with postback functions
        * 
        * **************************************************************/
        public Collector(Action<ADSBData> holdingPostback = null, Action<ADSBData> currentPostback = null) {
            archive = new List<ADSBData>();
            current = new Dictionary<string, ADSBData>();
            holding = new Dictionary<string, ADSBData>();

            this.holdingPostback = holdingPostback;
            this.currentPostback = currentPostback;
        }

        /*****************************************************************
        * AddData
        * 
        * Pushes ADSBData into the collection
        * 
        * **************************************************************/
        public void AddData(ADSBData data) {
            if(holding.ContainsKey(data.uid)) {
                ADSBData holdingData = holding[data.uid];
                holdingData.Merge(data);

                holdingPostback?.Invoke(holdingData);

                if(holdingData.IsComplete()) {
                    UpdateHolding(holdingData);
                }
            } else {
                holding.Add(data.uid, data);
            }

            RemoveStale();
        }

        /*****************************************************************
        * UpdateHolding
        * 
        * Pushes packets through holding->current->archive
        * 
        * **************************************************************/
        private void UpdateHolding(ADSBData data) {
            if(current.ContainsKey(data.uid)) {
                ADSBData currentData = current[data.uid];
                archive.Add(currentData);

                current.Remove(data.uid);
                current.Add(data.uid, data);
                currentPostback?.Invoke(data);
            } else {
                current.Add(data.uid, data);
                currentPostback?.Invoke(data);
            }
        }

        /*****************************************************************
        * RemoveStale
        * 
        * Removes stale (old) ADSBData from queues
        * 
        * **************************************************************/
        private void RemoveStale() {
            DateTime currDateTime = DateTime.Now;

            // Remove stale holding keys
            string[] holdingKeys = holding.Keys.ToArray();
            foreach(string holdingKey in holdingKeys) {
                ADSBData data = holding[holdingKey];
                if(data.msgDateTime.AddSeconds(staleTime) < currDateTime) {
                    holding.Remove(data.uid);
                }
            }

            // Remove stale current keys
            string[] currentKeys = current.Keys.ToArray();
            foreach(string currentKey in currentKeys) {
                ADSBData data = current[currentKey];
                if(data.msgDateTime.AddSeconds(staleTime) < currDateTime) {
                    current.Remove(data.uid);
                }
            }
        }

    }
}
