using MVCMessenger.Models;

namespace MVCMessenger.ViewModels
{
    public class MainViewModel
    {
        
        public List<User>? AllUsers { get; set; }
        public List<User>? Users { get; set; }
        public List<User>? FoundUsers { get; set; }
        public string? UserSearch { get; set; }
        public Status Status { get; set; } = Status.Main;
        public string? MessageText { get; set; }
        public string? UserId { get; set; }
        public string? InterlocutorId { get; set; }
        public List<Message> MessagesWithUser { get; set; }

        public MainViewModel() { }
        public MainViewModel(MainViewModel mvm)
        {
            AllUsers = mvm.AllUsers?? new List<User>();
            Users = mvm.Users?? new List<User>();
            FoundUsers = mvm.FoundUsers?? new List<User>();
            UserSearch = mvm.UserSearch?? null;
            Status = mvm.Status;
            MessageText = mvm.MessageText ?? null;
            UserId = mvm.UserId ?? null;
            InterlocutorId = mvm.InterlocutorId ?? null;
        }
    }
    

    public enum Status
    {
        Main,
        Searching,
        OnChat
    }
}
