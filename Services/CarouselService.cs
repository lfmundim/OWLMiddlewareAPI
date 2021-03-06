﻿using System;
using System.Collections.Generic;
using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLMiddleware.Models.Responses;

namespace OWLMiddleware.Services
{
    public class CarouselService : ICarouselService
    {
        public DocumentCollection CreateNewsCarousel(NewsResponse news)
        {
            List<DocumentSelect> documentSelect = new List<DocumentSelect>();
            foreach (Blog item in news.Blogs)
            {
                documentSelect.Add(new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = CreateMediaLink(item)
                    },
                    Options = new DocumentSelectOption[]
                {
                    new DocumentSelectOption
                    {
                        Label = new DocumentContainer
                        {
                            Value = new WebLink()
                            {
                                Uri = new Uri(item.DefaultUrl),
                                Target = WebLinkTarget.Self,
                                Title = "📰 Read"
                            }
                        }
                    }
                }
                });
            }

            return CreateDocumentCollection(documentSelect);
        }
        public DocumentCollection CreateTeamsCarousel(List<CompetitorElement> teams)
        {
            List<DocumentSelect> documentSelect = new List<DocumentSelect>();
            foreach (CompetitorElement item in teams)
            {
                documentSelect.Add(new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = CreateMediaLink(item)
                    },
                    Options = new DocumentSelectOption[]
                {
                    new DocumentSelectOption
                    {
                        Label = new DocumentContainer
                        {
                            Value = new PlainText
                            {
                                Text = $"This one!"
                            }
                        },
                        Value = new DocumentContainer
                        {
                            Value = new PlainText
                            {
                                Text = $"MainTeam_{item.Competitor.AbbreviatedName}"
                            }
                        }
                    }
                }
                });
            }

            return CreateDocumentCollection(documentSelect);
        }

        public DocumentCollection CreateMatchCarousel(ScheduleResponse match)
        {
            var list = new List<DocumentSelect>()
            {
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = CreateMediaLink(match)
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption()
                        {
                            Label = new DocumentContainer
                            {
                                Value = new PlainText
                                {
                                    Text = $"???"
                                }
                            },
                            Value = new DocumentContainer
                            {
                                Value = new PlainText
                                {
                                    Text = $"???"
                                }
                            }
                        }
                    }
                }
            };
            
            return CreateDocumentCollection(list);
        }

        private DocumentCollection CreateDocumentCollection(List<DocumentSelect> documentSelect)
        {
            return new DocumentCollection
            {
                ItemType = DocumentSelect.MediaType,
                Items = documentSelect.ToArray()
            };
        }

        private MediaLink CreateMediaLink(CompetitorElement team)
        {
            return new MediaLink
            {
                Uri = new Uri(team.Competitor.Logo),
                Title = $"{team.Competitor.Name}",
                AspectRatio = "1:1"
            };
        }

        private MediaLink CreateMediaLink(ScheduleResponse match)
        {
            var text = "";
            if(match.State.Equals("CONCLUDED"))
            {
                text = $"Winner: {match.Winner.Name} - Score {match.Scores[0].Value}x{match.Scores[1].Value}";
            }
            else
            {
                foreach(GameResponse g in match.Games)
                {
                    // TODO convert string names into "real names"
                    text += "->" + g.Attributes.Map;
                }
            }
            return new MediaLink
            {
                Uri = new Uri("https://www.tubefilter.com/wp-content/uploads/2018/01/overwatch-league.jpg"),
                Title = $"{match.Competitors[0].Name} vs {match.Competitors[1].Name}",
                Text = text,
                AspectRatio = "1:1"
            };
        }

        private MediaLink CreateMediaLink(Blog news)
        {
            var newsUri = "https://" + news.Thumbnail.Url.Substring(2);
            return new MediaLink
            {
                Uri = new Uri(newsUri),
                Title = news.Title,
                Text = $"{news.Summary}",
                AspectRatio = "1:1"
            };
        }
    }
}