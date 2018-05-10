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
using OWLMiddleware.Extensions;
using Lime.Protocol;

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

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="owlApiService"></param>
        /// <param name="carouselService"></param>
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
        /// <returns>Serialized JSON (BLiP format as default or regular JSON) with the filtered OWL-API Response.</returns>
        /// <response code="200">Successful call</response>
        /// <response code="202">Successful conversion to carrousel</response>
        /// <response code="400">Invalid Post Body</response>
        /// <response code="500">Internal error</response>
        [HttpPost, Route("lastmatchup"), ProducesResponseType(typeof(ScheduleResponse), 200)]
        public async Task<IActionResult> GetLastMatchup([FromBody] MatchupRequest request)
        {
            try
            {
                if (!(request.firstTeamId.IsTeamId()) || !(request.secondTeamId.IsTeamId())) return BadRequest();

                var team = await _owlApiService.GetTeamAsync(request.firstTeamId);
                var concludedMatches = GetConcludedMatches(team);
                var lastMatchup = GetLastMatchup(concludedMatches, request.secondTeamId);
                if (request.blipFormat)
                    return Accepted(_carouselService.CreateMatchCarousel(lastMatchup));
                else
                    return Ok(lastMatchup);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
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
        /// <returns>Serialized JSON (BLiP format as default or regular JSON) with the filtered OWL-API Response.</returns>
        /// <response code="200">Successful call</response>
        /// <response code="202">Successful conversion to carrousel</response>
        /// <response code="400">Invalid Post Body</response>
        /// <response code="500">Internal error</response>
        [HttpPost, Route("nextmatch"), ProducesResponseType(typeof(ScheduleResponse), 200)]
        public async Task<IActionResult> GetNextMatch([FromBody] MatchRequest request)
        {
            try
            {
                if (!request.teamId.IsTeamId()) return BadRequest();

                var team = await _owlApiService.GetTeamAsync(request.teamId);
                var futureMatches = GetFutureMatches(team);
                var nextMatch = futureMatches.FirstOrDefault();
                if (request.blipFormat)
                    return Accepted(_carouselService.CreateMatchCarousel(nextMatch));
                else
                    return Ok(nextMatch);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
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
        /// <param name="isBlipFormat">True for blip format; Default = true</param>
        /// <returns>Serialized JSON (BLiP format or regular JSON) with the filtered OWL-API Response.</returns>
        /// <response code="200">Successful call</response>
        /// <response code="202">Successful conversion to carrousel</response>
        /// <response code="400">Invalid Post Body</response>
        /// <response code="500">Internal error</response>
        [HttpGet, Route("divisionteams/{divisionId}/{isBlipFormat}"), ProducesResponseType(typeof(CompetitorElement), 200)]
        public async Task<IActionResult> GetTeamsByDivision(DivisionIds divisionId, bool isBlipFormat = true){
            try
            {
                var allTeams = await _owlApiService.GetTeamsAsync();
                var competitors = allTeams.Competitors;
                var divisionteams = competitors.Where(t => t.Competitor.OwlDivision == (int)divisionId);
                if (isBlipFormat)
                    return Accepted(_carouselService.CreateTeamsCarousel(divisionteams.ToList()));
                else
                    return Ok(divisionteams.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Gets future team matches
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private static ScheduleResponse[] GetFutureMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("PENDING")).OrderBy(s => s.StartDate).ToArray();

        /// <summary>
        /// Gets past team matches
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        private static ScheduleResponse[] GetConcludedMatches(TeamResponse team) => team.Schedule.Where(t => t.State.Equals("CONCLUDED")).OrderBy(s => s.StartDate).ToArray();

        /// <summary>
        /// Gets last matchup against an opponent
        /// </summary>
        /// <param name="concludedMatches"></param>
        /// <param name="opponentId"></param>
        /// <returns></returns>
        private static ScheduleResponse GetLastMatchup(ScheduleResponse[] concludedMatches, int opponentId) => concludedMatches.Where(m => m.Competitors[1].Id == opponentId || m.Competitors[0].Id == opponentId).ToArray().Last();
    }
}