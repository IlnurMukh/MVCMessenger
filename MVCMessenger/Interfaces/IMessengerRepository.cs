using MVCMessenger.Models;

namespace MVCMessenger.Interfaces;

public interface IMessengerRepository
{
    
    Task<IEnumerable<User>> GetUsersAsync();
    Task<IEnumerable<User>> GetChattingUsersAsync(string userId);
    Task<User> GetByUsernameAsync(string username);
    Task<IEnumerable<User?>> GetUsernamesLike(string? usernameBeginning);
    bool AddUserAsync(User user);
    bool UserExists(User user);
    User? FindUser(User user);
    bool Save();

    Task<IEnumerable<Chat>> GetChatsAsync(string userId); // Or any other way
    Task<Chat?> GetChat(string userId0, string userId1);

    bool CreateChat(string userId0, string userId1);
    //Task<Chat> EditChatById(int id);
    Task<IEnumerable<Message>> GetMessagesAsync(int chatId);
    bool SendMessageAsync(string messageText, string sender, string receiver);
    // TODO Add: GetMessagesAsync(by chat id), SendMessage(by chat id, Message), 
    
}