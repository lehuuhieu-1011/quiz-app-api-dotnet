using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;

namespace quiz_app_dotnet_api.SignalR
{
  public class ChatHub : Hub
  {
    private readonly IDictionary<string, UserSignalR> _user;
    private readonly string _bot;
    public ChatHub(IDictionary<string, UserSignalR> user)
    {
      _user = user;
      _bot = "My Bot";
    }
    public async Task JoinRoom(UserSignalR user)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, user.RoomId);
      _user[Context.ConnectionId] = user;
      await Clients.Group(user.RoomId).SendAsync("ReceiveMessageAdmin", $"{user.UserName} has join!");
      await ListUserInRoom(user.RoomId);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
      if (_user.TryGetValue(Context.ConnectionId, out UserSignalR user))
      {
        _user.Remove(Context.ConnectionId);
        Clients.Group(user.RoomId).SendAsync("ReceiveMessageAdmin", $"{_bot} {user.UserName} has left");
        ListUserInRoom(user.RoomId);
      }
      return base.OnDisconnectedAsync(exception);
    }

    public override Task OnConnectedAsync()
    {
      Clients.Caller.SendAsync("ReceiveMessage", $"{_bot} Hi {Context.ConnectionId}");
      return base.OnConnectedAsync();
    }

    public async Task AdminRequest(string roomId)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
      await Clients.Caller.SendAsync("ReceiveMessageAdmin", $"{_bot} Hi Admin");
    }

    public async Task ListUserInRoom(string roomId)
    {
      var users = _user.Values.Where(u => u.RoomId == roomId).Select(u => u.UserName);

      await Clients.Group(roomId).SendAsync("UserInRoom", users);
    }

    public async Task ReceiveQuestion(string roomId, string courseId, QuestionQuiz question)
    {
      await Clients.Group(roomId).SendAsync("Question", courseId, question);
    }

    public async Task EndGame(string roomId)
    {
      await Clients.Group(roomId).SendAsync("EndGame", "Done");
    }

    public async Task QuestionTimeout(string roomId)
    {
      await Clients.Group(roomId).SendAsync("QuestionTimeout", "Timeout");
    }
  }
}