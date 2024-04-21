using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace MVCMessenger.Models
{
    [Serializable]
    public class Chat
    {
        [Key]
        public int Id { get; set; }

        [Required] public string ChatDetailsJSON { get; set; } = "{\r\n  \"Users\": [],\r\n  \"Messages\": {}\r\n}";

        //public static JObject StringToJObject(string messageText) => JObject.Parse(messageText);
        public JObject StringToJObject() => JObject.Parse(ChatDetailsJSON);

        public static string JObjectToString(JObject data) => data.ToString();
    }


    // ChatDetailsJSON structure:
    // {
    //   "Users": [
    //     (str)"User 0 id",
    //     (str)"User 1 id"
    //   ],
    //   "Messages": {
    //     (str)"Message id 0": {
    //     (str)"Sender id": 0,
    //     (str)"Receiver id":1,
    //     (str)"DateTime": "01-01-1970 00:00:00",
    //     (str)"MessageText": "Message from 0 to 1"
    //     },
    //     (str)"Message id 1": {
    //     (str)"Sender id": 1,
    //     (str)"Receiver id":0,
    //     (str)"DateTime": "01-01-1970 00:00:10",
    //     (str)"MessageText": "Message from 1 to 0"
    //     }
    //   }
    // }
    //
    // DateTime conversion pattern: "dd-MM-yyyy HH:mm:ss"
}
