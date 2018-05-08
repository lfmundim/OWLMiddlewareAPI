using System;
using System.Collections.Generic;
using System.Text;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Models
{
    public static class MyConstants
    {
        public const string FacebookCarouselAspectRatio = "1:1";
        public static TeamIds[] AllTeams = { TeamIds.BostonUprising, TeamIds.DallasFuel, TeamIds.FloridaMayhem, TeamIds.HoustonOutlaws, TeamIds.LondonSpitfire, TeamIds.LosAngelesGladiators,
            TeamIds.LosAngelesValiant, TeamIds.NewYorkExcelsior, TeamIds.PhiladelphiaFusion, TeamIds.SanFranciscoShock, TeamIds.SeoulDynasty, TeamIds.ShanghaiDragons };
        public static int DefaultBroadcastOverhead = -30;
        //public static string BroadcastDomain = "@broadcast.msging.net";
        //public static string BroadcastMessageIdPrefix = "Match-Id-";
    }
}
