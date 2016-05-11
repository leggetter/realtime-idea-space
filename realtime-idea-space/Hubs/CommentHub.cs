using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using realtime_idea_space.Models;

namespace realtime_idea_space.Hubs
{
    public class CommentHub : Hub
    {
        public static void NewCommmentAdded(CommentModel comment)
        {
            var hub =GlobalHost.ConnectionManager.GetHubContext<CommentHub>();
            hub.Clients.All.NewCommentAdded(comment);
        }
    }
}