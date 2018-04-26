using OWLMiddleware.Models;
using Microsoft.AspNetCore.Mvc;
using OWLMiddleware.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace OWLMiddleware.Controllers
{
    [Route("team")]
    public class TeamController : Controller
    {
        private readonly IOWLApiService _owlApiService;

        public TeamController(IOWLApiService owlApiService)
        {
            _owlApiService = owlApiService;
        }
        
        [HttpPost, Route("lastmatchup")]
        public async Task<string> GetLastMatchup([FromBody] MatchupRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(Convert.ToInt32(request.firstTeamId));
            var concludedMatches = GetConcludedMatches(team);
            var lastMatchup = GetLastMatchup(concludedMatches, request.secondTeamId);
            return JsonConvert.SerializeObject(lastMatchup);
        }

        private static Schedule[] GetConcludedMatches(Team team) => team.Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.EndDate).ToArray();

        private static Schedule GetLastMatchup(Schedule[] concludedMatches, int opponentId) => concludedMatches.Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}