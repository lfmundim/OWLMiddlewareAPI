using System.Collections.Generic;
using System.Threading.Tasks;
using OWLMiddleware.Models.Responses;
using RestEase;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Services
{
    //actual OWL-API
    public interface IOWLApiService
    {
        [Get("teams/{id}")]
        Task<TeamResponse> GetTeamAsync([Path("id")]int id);

        [Get("teams")]
        Task<TeamsResponse> GetTeamsAsync();

        [Get("rankings")]
        Task<RankingResponse> GetRankingAsync();
    }
}