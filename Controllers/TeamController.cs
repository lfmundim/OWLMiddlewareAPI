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
using OWLMiddleware.Services;

namespace OWLMiddleware.Controllers
{
    /// <summary>
    /// Controller responsible for team-related calls.
    /// </summary>
    [Route("team")]
    [Produces("application/json")]
    public class TeamController : Controller
    {
        private readonly IOWLApiService _owlApiService;
        private readonly ICarouselService _carouselService;

        public TeamController(IOWLApiService owlApiService,
                              ICarouselService carouselService)
        {
            _owlApiService = owlApiService;
            _carouselService = carouselService;
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
        ///        "secondTeamId":4408,
        ///        "blipFormat":true
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Serialized JSON with the filtered OWL-API Response.</returns>
        [HttpPost, Route("lastmatchup")]
        public async Task<IActionResult> GetLastMatchup([FromBody] MatchupRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.firstTeamId);
            var concludedMatches = GetConcludedMatches(team);
            var lastMatchup = GetLastMatchup(concludedMatches, request.secondTeamId);
            if(request.blipFormat)
                return Ok(_carouselService.CreateMatchCarousel(lastMatchup));
            else
                return Ok(lastMatchup);
        }

        /// <summary>
        /// Gets next team match, by its ID
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /team/nextmatch
        ///     {
        ///        "teamId":4523,
        ///        "blipFormat":true
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Serialized JSON (BLiP format or regular JSON) with the filtered OWL-API Response.</returns>
        [HttpPost, Route("nextmatch")]
        public async Task<IActionResult> GetNextMatch([FromBody] MatchRequest request)
        {
            var team = await _owlApiService.GetTeamAsync(request.teamId);
            var futureMatches = GetFutureMatches(team);
            var nextMatch = futureMatches.FirstOrDefault();
            if(request.blipFormat)
                return Ok(_carouselService.CreateMatchCarousel(nextMatch));
            else
                return Ok(nextMatch);
        }

        /// <summary>
        /// Gets all teams from a division, from its ID
        /// </summary>
        /// /// <remarks>
        /// Atlantic Division using blip:
        ///
        ///     GET /team/divisionteams/79/true
        ///
        /// </remarks>
        /// <param name="divisionId">Can be either 80 (Pacific) or 79 (Atlantic)</param>
        /// <param name="isBlipFormat">True for blip format</param>
        /// <returns>Serialized JSON (BLiP format or regular JSON) with the filtered OWL-API Response.</returns>
        [HttpGet, Route("divisionteams/{divisionId}/{isBlipFormat}")]
        public async Task<IActionResult> GetTeamsByDivision(DivisionIds divisionId, bool blipFormat){
            var allTeams = await _owlApiService.GetTeamsAsync();
            var competitors = allTeams.Competitors;
            var divisionteams = competitors.Where(t => t.Competitor.OwlDivision == (int)divisionId);
            if(blipFormat)
                return Ok(_carouselService.CreateTeamsCarousel(divisionteams.ToList()));
            else
                return Ok(divisionteams.ToList());
        }

        private static ScheduleResponse[] GetFutureMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("PENDING")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse[] GetConcludedMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.StartDate).ToArray();

        private static ScheduleResponse GetLastMatchup(ScheduleResponse[] concludedMatches, int opponentId) => concludedMatches.Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}