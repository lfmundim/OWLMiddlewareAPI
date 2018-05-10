using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLMiddleware.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OWLMiddleware.Services
{
    public interface ICarouselService
    {
        DocumentCollection CreateTeamsCarousel(List<CompetitorElement> teams);
        DocumentCollection CreateMatchCarousel(ScheduleResponse match);
        DocumentCollection CreateNewsCarousel(NewsResponse news);
    }
}