using AutoMapper;
using GenZStyleApp.DAL.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProjectParticipantManagement.DAL.Infrastructures;
using System.Reflection;
using GenZStyleAPP.BAL.DTOs.ChatHistorys;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleApp.DAL.ViewModels;

namespace GenZStyleApp_API.SignalRHubs
{


    public class ChatHub : Hub
    {

        #region Properties
        /// <summary>
        /// List of online users
        /// </summary>
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();

        /// <summary>
        /// List of all users
        /// </summary>
        public readonly static List<UserViewModel> _Users = new List<UserViewModel>();

        /// <summary>
        /// List of available chat rooms
        /// </summary>
        private readonly static List<RoomViewModel> _Rooms = new List<RoomViewModel>();

        /// <summary>
        /// Mapping SignalR connections to application users.
        /// (We don't want to share connectionId)
        /// </summary>
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        #endregion
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ChatHub(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _mapper = mapper;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userid"].ToString();
            if (userId is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }
        public int Send(int roomId, string sender, string receiver, string message)
        {
            if (roomId != 0 && roomId != null)
            {
                return SendToRoom(roomId, message);
            }
            else
            {
                return  SendMessage(sender, receiver, message);
            }
        }
        public int SendMessage(string sender, string receiver, string message)
        {
            try
            {
                var context = new GenZStyleDbContext();
                var time = DateTime.UtcNow.AddHours(7);
                Message message1 = new Message
                {
                    SenderId = Int32.Parse(sender),
                    ReceiverId = Int32.Parse(receiver),
                    Content = message,
                    CreateAt = time
                };
                 context.Messages.Add(message1);
                 context.SaveChangesAsync();
                 int idMess = message1.AccountId;
                 Clients.Groups(sender, receiver).SendAsync("ReceiveMessage", sender, receiver, message, time);
                return idMess;
            }
             
            catch (Exception)
            {

                throw;
            }
                        
        }
        public int SendToRoom(int roomId, string message)
        {
            try
            {
                using (var db = new GenZStyleDbContext())
                {
                    var account = db.Accounts.Where(u => u.Username == IdentityName).FirstOrDefault();
                    var room = db.Inboxes.Where(r => r.InboxId == roomId).FirstOrDefault();

                    // Create and save message in database
                    Message msg = new Message()
                    {
                        Content = Regex.Replace(message, @"(?i)<(?!img|a|/a|/img).*?>", String.Empty),
                        CreateAt = DateTime.Now,
                        AccountSender = account,
                        Inbox = room
                    };
                    db.Messages.Add(msg);
                    db.SaveChanges();
                    int idMess = msg.AccountId;
                    // Broadcast the message
                    var messageViewModel = _mapper.Map<MessageViewModel>(msg);                   
                    Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", messageViewModel);
                    return idMess;
                }
            }
            catch (Exception)
            {
                Clients.Caller.SendAsync("OnError", "Message not send!");
            }
            return 0;
        }
        public void Join(int roomId)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == IdentityName).FirstOrDefault();
                if (user.CurrentRoomId != roomId)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoomId.ToString()))
                        Clients.OthersInGroup(user.CurrentRoomId.ToString()).SendAsync("removeUser", user);

                    // Join to new chat room
                    Leave(  user.CurrentRoomId);
                    Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
                    user.CurrentRoomId = roomId;

                    // Tell others to update their list of users
                    Clients.OthersInGroup(roomId.ToString()).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("You failed to join the chat room!" + ex.Message);
            }
            
        }
        private void Leave(int roomId)
        {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        public int CreateRoom(string roomName, string userSelected)
        {
            try
            {
                using (var db = new GenZStyleDbContext())
                {                   
                    // Create and save chat room in database
                    var account = db.Accounts
                    .Where(a => a.Username == IdentityName)
                    .FirstOrDefault();
                    var inbox = new Inbox()
                    {
                        Name = roomName,
                        Account = account
                    };
                    var result = db.Inboxes.Add(inbox);
                    db.SaveChanges();
                    inbox.InboxId = inbox.InboxId;
                    if (inbox != null)
                    {
                        // Update room list
                        var roomViewModel = _mapper.Map<Inbox,RoomViewModel>(inbox);
                        _Rooms.Add(roomViewModel);


                        char[] spearatorUser = { '|' };
                        char[] spearatorElement = { ';' };
                        String[] arrayUserSelected = userSelected.Split(spearatorUser);

                        for (var i = 0; i < arrayUserSelected.Length; i++)
                        {   
                            String[] User = arrayUserSelected[i].Split(spearatorElement);
                            
                            
                                if (User[0] == account.Username)
                                {
                                    this.AddUserToRoom(User[0], inbox.InboxId, 1);
                                }
                                else
                                {
                                    this.AddUserToRoom(User[0], inbox.InboxId);
                                }


                                string userId;
                                if (_ConnectionsMap.TryGetValue(User[1], out userId))
                                {
                                    Clients.Client(userId).SendAsync("addChatRoom", roomViewModel);
                                }
                            
                        }
                    }
                    return inbox.InboxId;
                } //using
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("Couldn't create chat room: " + ex.Message);
            }
            return 0;
        }
        public void AddUserToRoom(string username, int roomId, int role = 0)
        {
            try
            {
                using (var db = new GenZStyleDbContext())
                {   
                    var account = db.Accounts.Where( a => a.Username == username ).FirstOrDefault();
                    // Create and save chat room in database
                    // var user = db.Users.Where(u => u.UserName == IdentityName).FirstOrDefault();
                    var inboxPaticipant = new InboxPaticipant()
                    {
                        AccountId = account.AccountId,
                        InboxId = roomId,
                        
                    };
                    db.InboxPaticipants.Add(inboxPaticipant);
                    db.SaveChanges();
                }//using
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("Couldn't create chat room: " + ex.Message);
            }
        }
        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }
    }
}
