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
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Controllers
{
    /// <summary>
    /// Controller responsible for AI calls.
    /// </summary>
    [Route("team")]
    [Produces("application/json")]
    public class TeamController : Controller
    {
        private readonly IOWLApiService _owlApiService;

        public TeamController(IOWLApiService owlApiService)
        {
            _owlApiService = owlApiService;
        }

        /// <summary>
        /// Gets last matchup between two teams, by their ID
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /team/lastmatchup
        ///     {
        ///        "firstTeamId":4523,
        ///        "secondTeamId":4408
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Serialized JSON with the filtered OWL-API Response.</returns>
        [HttpPost, Route("lastmatchup")]
        public async Task<ScheduleResponse> GetLastMatchup([FromBody] MatchupRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.firstTeamId);
            var concludedMatches = GetConcludedMatches(team);
            var lastMatchup = GetLastMatchup(concludedMatches, request.secondTeamId);
            
            return lastMatchup;
        }

        /// <summary>
        /// Gets next team match, by its ID
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /team/nextmatch
        ///     {
        ///        "teamId":4523
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Serialized JSON with the filtered OWL-API Response.</returns>
        [HttpPost, Route("nextmatch")]
        public async Task<ScheduleResponse> GetNextMatch([FromBody] MatchRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.teamId);
            var futureMatches = GetFutureMatches(team);
            var nextMatch = futureMatches.FirstOrDefault();
            return nextMatch;
        }

        /// <summary>
        /// Gets all teams from a division, from its ID
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /team/divisionteams
        ///     {
        ///        "divisionId":79
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Serialized JSON with the filtered OWL-API Response.</returns>
        [HttpGet, Route("divisionteams/{divisionId}")]
        public async Task<List<CompetitorElement>> GetTeamsByDivision(DivisionIds divisionId){
            var allTeams = await _owlApiService.GetTeamsAsync();
            var competitors = allTeams.Competitors;
            var divisionteams = competitors.Where(t => t.Competitor.OwlDivision == (int)divisionId);
            return divisionteams.ToList();
        }

        private static ScheduleResponse[] GetFutureMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("PENDING")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse[] GetConcludedMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse GetLastMatchup(ScheduleResponse[] concludedMatches, int opponentId) => concludedMatches.Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}