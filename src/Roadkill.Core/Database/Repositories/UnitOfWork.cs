using Roadkill.Core.Database.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private Entities.Entities context = new Entities.Entities();
        private BaseRepository<explik_alerts> alertsRepository;
        private BaseRepository<explik_comments> commentsRepository;
        private BaseRepository<explik_competition> competitionRepository;
        private BaseRepository<explik_competitionpage> competitionPagesRepository;
        private BaseRepository<explik_course> courseRepository;
        private BaseRepository<explik_coursepage> coursePageRepository;
        private BaseRepository<explik_pagecontent> pageContentRepository;
        private BaseRepository<explik_pages> pagesRepository;
        private BaseRepository<explik_siteconfiguration> siteConfigurationRepository;
        private BaseRepository<explik_users> usersRepository;
        //public void Add<TEntity>( TEntity entity) where TEntity : class
        //{

        //}

        public BaseRepository<explik_alerts> AlertsRepository
        {
            get
            {

                if (this.alertsRepository == null)
                {
                    this.alertsRepository = new BaseRepository<explik_alerts>(context);
                }
                return alertsRepository; 
            }
        }

        public BaseRepository<explik_comments> CommentsRepository
        {
            get
            {

                if (this.commentsRepository == null)
                {
                    this.commentsRepository = new BaseRepository<explik_comments>(context);
                }
                return commentsRepository;
            }
        }

        public BaseRepository<explik_competition> CompetitionRepository
        {
            get
            {

                if (this.competitionRepository == null)
                {
                    this.competitionRepository = new BaseRepository<explik_competition>(context);
                }
                return competitionRepository;
            }
        }

        public BaseRepository<explik_competitionpage> CompetitionPagesRepository
        {
            get
            {

                if (this.competitionPagesRepository == null)
                {
                    this.competitionPagesRepository = new BaseRepository<explik_competitionpage>(context);
                }
                return competitionPagesRepository;
            }
        }

        public BaseRepository<explik_course> CourseRepository
        {
            get
            {

                if (this.courseRepository == null)
                {
                    this.courseRepository = new BaseRepository<explik_course>(context);
                }
                return courseRepository;
            }
        }

        public BaseRepository<explik_coursepage> CoursePageRepository
        {
            get
            {

                if (this.coursePageRepository == null)
                {
                    this.coursePageRepository = new BaseRepository<explik_coursepage>(context);
                }
                return coursePageRepository;
            }
        }

        public BaseRepository<explik_pagecontent> PageContentRepository
        {
            get
            {

                if (this.pageContentRepository == null)
                {
                    this.pageContentRepository = new BaseRepository<explik_pagecontent>(context);
                }
                return pageContentRepository;
            }
        }

        public BaseRepository<explik_pages> PagesRepository
        {
            get
            {

                if (this.pagesRepository == null)
                {
                    this.pagesRepository = new BaseRepository<explik_pages>(context);
                }
                return pagesRepository;
            }
        }

        public BaseRepository<explik_siteconfiguration> SiteConfigurationRepository
        {
            get
            {

                if (this.siteConfigurationRepository == null)
                {
                    this.siteConfigurationRepository = new BaseRepository<explik_siteconfiguration>(context);
                }
                return siteConfigurationRepository;
            }
        }

        public BaseRepository<explik_users> UsersRepository
        {
            get
            {

                if (this.usersRepository == null)
                {
                    this.usersRepository = new BaseRepository<explik_users>(context);
                }
                return usersRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
