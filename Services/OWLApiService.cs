using System.Collections.Generic;
using System.Threading.Tasks;
using OWLMiddleware.Models;
using RestEase;

namespace OWLMiddleware.Services
{
    public interface IOWLApiService
    {
        [Get("teams/{id}")]
        Task<Team> GetTeamAsync([Path("id")]int id);
    }
}