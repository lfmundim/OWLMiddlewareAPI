using System.Collections.Generic;
using System.Threading.Tasks;
using OWLMiddleware.Models.Responses;
using RestEase;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Services
{
    /// <summary>
    /// OWL Official API
    /// </summary>
    public interface IOWLApiService
    {
        /// <summary>
        /// Gets all team info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Get("teams/{id}")]
        Task<TeamResponse> GetTeamAsync([Path("id")]int id);

        /// <summary>
        /// Gets all teams info
        /// </summary>
        /// <returns></returns>
        [Get("teams")]
        Task<TeamsResponse> GetTeamsAsync();

        /// <summary>
        /// Gets ranking from top to bottom
        /// </summary>
        /// <returns></returns>
        [Get("rankings")]
        Task<RankingResponse> GetRankingAsync();

        /// <summary>
        /// Gets latest news
        /// </summary>
        /// <param name="newsAmmount"></param>
        /// <returns></returns>
        [Get("news?pageSize={count}&page=1")]
        Task<NewsResponse> GetNewsAsync([Path("count")]int newsAmmount = 5);
    }
}