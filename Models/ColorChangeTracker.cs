using System;
namespace ColorGridSSE.Models
{
    public class ColorChangeTracker
    {        
        public long ChgTimestamp { get; set; }
        private Dictionary<string, string> gridColors;

        public ColorChangeTracker() {         
            gridColors = new Dictionary<string, string>();
        }

        public void SetColor(string nodeId, string color) {
            gridColors[nodeId] = color;

            // track time of change
            ChgTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public Dictionary<string, string> GetColors() {
            return gridColors;
        }
    }
}

