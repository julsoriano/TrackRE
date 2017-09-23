using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TrackRE.Hubs
{
    [HubName("reprop")]
    public class TrackREHub : Hub
    {}

    [HubName("routeplanner")]
    public class RoutePlannerHub : Hub
    {
        public void refreshRoute(dynamic directions)
        {
            Clients.Others.refreshRoutes(directions);
        }
    }
}