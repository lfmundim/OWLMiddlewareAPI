using OWLMiddleware.Models;
using Microsoft.AspNetCore.Mvc;
using OWLMiddleware.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OWLMiddleware.Models.Responses;
using OWLMiddleware.Models.Requests;

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
        public async Task<ScheduleResponse> GetLastMatchup([FromBody] MatchupRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.firstTeamId);
            var concludedMatches = GetConcludedMatches(team);
            var lastMatchup = GetLastMatchup(concludedMatches, request.secondTeamId);
            
            return lastMatchup;
        }

        [HttpPost, Route("nextmatch")]
        public async Task<ScheduleResponse> GetNextMatch([FromBody] MatchRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.teamId);
            var futureMatches = GetFutureMatches(team);
            var nextMatch = futureMatches.FirstOrDefault();
            return nextMatch;
        }

        [HttpPost, Route("divisionteams")]
        public async Task<List<CompetitorElement>> GetTeamsByDivision([FromBody] TeamRequest request){
            var allTeams = await _owlApiService.GetTeamsAsync();
            var competitors = allTeams.Competitors;
            var divisionteams = competitors.Where(t => t.Competitor.OwlDivision == (int)request.divisionId);
            return divisionteams.ToList();
        }

        private static ScheduleResponse[] GetFutureMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("PENDING")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse[] GetConcludedMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse GetLastMatchup(ScheduleResponse[] concludedMatches, int opponentId) => concludedMatches.Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}