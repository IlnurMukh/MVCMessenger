using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json.Linq;

namespace MVCMessenger.Models
{
    [Serializable]
    public class Message
    {
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string MessageText { get; set; }

        

    }
}
