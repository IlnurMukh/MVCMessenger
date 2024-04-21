using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MVCMessenger.Interfaces;
using MVCMessenger.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVCMessenger.Repositories
{ 
    public class MessengerRepository : IMessengerRepository
    {
        private readonly AppDbContext _context;

        public MessengerRepository(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetChattingUsersAsync(string userId)
        {
            List<User> users = new List<User>();
            foreach (var chat in GetChatsAsync(userId).Result)
            {
                users.Add(GetUserFromChat(chat, userId));
            }

            return users;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User?>> GetUsernamesLike(string? usernameBeginning)
        {
            return await _context.Users.Where(u => u.Username.StartsWith(usernameBeginning)).ToListAsync();
        }

        public bool AddUserAsync(User user)
        {
            if(!UserExists(user))
                _context.Add(user);
            return Save();
        }

        public bool UserExists(User user)
        {
            return _context.Users.Where(u => u.Email == user.Email || u.Username == user.Username).ToList().Count > 0;
        }

        public User? FindUser(User user)
        {
            var users = _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).ToList();
            return users.Count > 0 ? users[0] : null;
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<IEnumerable<Chat>> GetChatsAsync(string userId)
        {
            List<Chat> allChats = await _context.Chats.ToListAsync();
            List<Chat> chats = new List<Chat>();
            foreach (var chat in allChats)
            {
                JObject jObject = chat.StringToJObject();
                JArray jArray = (JArray)jObject["Users"];
                var id0 = jArray[0];
                var id1 = jArray[1];
                if (userId == id0.Value<string>() || userId == id1.Value<string>())
                    chats.Add(chat);
            }
            return chats;
        }

        public async Task<Chat?> GetChat(string userId0, string userId1)
        {
            List<Chat> allChats = await _context.Chats.ToListAsync();
            foreach (var chat in allChats)
            {
                JObject jObject = chat.StringToJObject();
                JArray jArray = (JArray)jObject["Users"];
                var id0 = jArray[0].Value<string>();
                var id1 = jArray[1].Value<string>();
                if ((userId0 == id0 && userId1 == id1) || (userId0 == id1 && userId1 == id0))
                    return chat;
            }
            return null;
        }

        public bool CreateChat(string userId0, string userId1)
        {
            Chat chat = new Chat();
            JObject jObject = chat.StringToJObject();
            JArray usersJArray = (JArray)jObject["Users"];
            usersJArray.Add(userId0);
            usersJArray.Add(userId1);
            jObject["Users"] = usersJArray;
            chat.ChatDetailsJSON = Chat.JObjectToString(jObject);
            _context.Chats.Add(chat);
            return Save();
        }

        //public async Task<Chat> EditChatById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<Message>> GetMessagesAsync(int chatId)
        {
            Chat chat = await _context.Chats.FirstOrDefaultAsync(u =>  u.Id == chatId);
            JObject jObject = chat.StringToJObject();
            JObject messagesJObject = (JObject)jObject["Messages"];
            List<Message> messages = new List<Message>();
            var jMessages = jObject["Messages"].Children();
            int i = 0;
            int cnt = jMessages.Count();
            while (i < cnt)
            {
                JToken token = messagesJObject[i.ToString()];
                
                Message message = new Message()
                {
                    Sender = int.Parse(token["Sender"].Value<string>()),
                    Receiver = int.Parse(token["Receiver"].Value<string>()),
                    DateTime = DateTime.Parse(token["DateTime"].Value<string>()),
                    MessageText = token["MessageText"].Value<string>()
                };
                messages.Add(message);
                i++;
            }
            return messages;
        }

        public bool SendMessageAsync(string messageText, string sender, string receiver)
        {
            Chat chat = GetChat(sender, receiver).Result;
            JObject messageJObject = new JObject
            {
                { "Sender", sender },
                { "Receiver", receiver},
                { "DateTime", DateTime.Now.ToString()},
                { "MessageText", messageText}
            };
            JObject jObject = chat.StringToJObject();
            JObject messagesJObject = (JObject)jObject["Messages"];
            messagesJObject.Add(GetMessageId(chat), messageJObject);
            chat.ChatDetailsJSON = Chat.JObjectToString(jObject);
            _context.Chats.Update(chat);
            return Save();
        }

        public string GetMessageId(Chat chat) => chat.StringToJObject()["Messages"].Children().Count().ToString();

        public User GetUserFromChat(Chat chat, string userId)
        {
            JObject jObject = chat.StringToJObject();
            JArray jArray = (JArray)jObject["Users"];
            var id0 = jArray[0].Value<string>();
            var id1 = jArray[1].Value<string>();
            if (id0 == userId)
                return _context.Users.FirstOrDefault(u => u.Id == int.Parse(id1));
            return _context.Users.FirstOrDefault(u => u.Id == int.Parse(id0));

        }
    }
}
