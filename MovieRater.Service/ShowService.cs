﻿using MovieRater.Data;
using MovieRater.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRater.Service
{
    public class ShowService
    {
        private readonly Guid _userId;

        public ShowService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateShow(ShowCreate model)
        {
            var entity =
                new Show()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Description = model.Description,
                    Actors = model.Actors
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Shows.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ShowListItem> GetShows()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Shows
                        .Where(s => s.OwnerId == _userId)
                        .Select(
                            s =>
                                new ShowListItem
                                {
                                    ShowId = s.ShowId,
                                    Title = s.Title,
                                    Description = s.Description,
                                    AddedShow = s.AddedShow,
                                    //Erics Changes
                                    Reviews = s.Reviews.Select(r => new ShowReviewDisplayItem()
                                    {
                                        ReviewId = r.ReviewId,
                                        Score = r.Score,
                                        ReviewText = r.ReviewText
                                    }).ToList()
                                }                                
                        );
                return query.ToArray();
            }
        }

        public IEnumerable<ShowListItem> GetShowByTitle(string title)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Shows
                    .Where(s => s.Title == title)
                    .Select(
                        s =>
                        new ShowListItem()
                        {
                            ShowId = s.ShowId,
                            Title = s.Title,
                            Description = s.Description,
                            Genre = s.Genre,
                            AddedShow = s.AddedShow,
                        }
                        );
                return query.ToArray();
            }

        }

        /*public ShowDetail GetShowByTitle(string title)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Shows
                        .SingleOrDefault(s => s.Title == title && s.OwnerId == _userId);
                return
                    new ShowDetail
                    {
                        ShowId = entity.ShowId,
                        Title = entity.Title,
                        Description = entity.Description,
                        Genre = entity.Genre,
                        AddedShow = entity.AddedShow
                    };
                
            }*/
        
    }
}
