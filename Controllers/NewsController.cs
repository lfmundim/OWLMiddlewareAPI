using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWLMiddleware.Models.Responses;
using OWLMiddleware.Services;
using RestEase;

namespace OWLMiddleware.Controllers
{
    /// <summary>
    /// Controller responsible for News related API calls
    /// </summary>
    [Route("news")]
    [Produces("application/json")]
    public class NewsController : Controller
    {
        private readonly IOWLApiService _owlApiService;
        private readonly ICarouselService _carouselService;

        public NewsController(IOWLApiService owlApiService,
                              ICarouselService carouselService)
        {
            _owlApiService = owlApiService;
            _carouselService = carouselService;
        }

        /// <summary>
        /// Gets latest N news
        /// </summary>
        /// /// <remarks>
        /// Get latest 5 news:
        ///
        ///     GET /news/latest/5/true
        /// </remarks>
        /// <param name="ammount">The quantity of news you want to get (Messenger has a max. limit of 10 Carousel Cards); Default = 5</param>
        /// <param name="isBlipFormat">True for blip format; Default = true</param>
        /// <returns>Serialized JSON (BLiP format as default or regular JSON) with the filtered OWL-API Response.</returns>
        /// <response code="200">Successful call</response>
        /// <response code="202">Successful conversion to carrousel</response>
        /// <response code="500">Internal error</response>
        [HttpGet, Route("latest/{ammount}/{isBlipFormat}"), ProducesResponseType(typeof(NewsResponse), 200)]
        public async Task<IActionResult> GetLatestNews(int ammount, bool isBlipFormat = true)
        {
            try
            {
                var news = await _owlApiService.GetNewsAsync(ammount);
                if (!isBlipFormat)
                    return Ok(news);
                else
                    return Accepted(_carouselService.CreateNewsCarousel(news));
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }
    }
}