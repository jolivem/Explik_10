using Roadkill.Core.Database.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database.Repositories
{
    public class UnitOfWork : IDisposable
    {

        private readonly Entities.Entities context;
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

        #region repositories

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

        #endregion

        internal IQueryable<explik_pages> Pages
        {
            get { return PagesRepository.GetAll(); }
        }
        internal IQueryable<explik_pagecontent> PageContents
        {
            get { return PageContentRepository.GetAll(); }
        }

        internal IQueryable<explik_users> Users
        {
            get { return UsersRepository.GetAll(); }
        }

        internal IQueryable<explik_comments> Comments
        {
            get { return CommentsRepository.GetAll(); }
        }

        internal IQueryable<explik_alerts> Alerts
        {
            get { return AlertsRepository.GetAll(); }
        }

        internal IQueryable<explik_competition> Competitions
        {
            get { return CompetitionRepository.GetAll(); }
        }

        internal IQueryable<explik_competitionpage> CompetitionPages
        {
            get { return CompetitionPagesRepository.GetAll(); }
        }

        internal IQueryable<explik_course> Courses
        {
            get { return CourseRepository.GetAll(); }
        }

        internal IQueryable<explik_coursepage> CoursePages
        {
            get { return CoursePageRepository.GetAll(); }
        }


        public UnitOfWork(string connectionString)
        {
            var entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = @"MySql.Data.MySqlClient";
            // use your ADO.NET connection string
            //entityBuilder.ProviderConnectionString = @"server=localhost;user id=Admin;password=Admin;persistsecurityinfo=True;database=explik";
            entityBuilder.ProviderConnectionString = "persistsecurityinfo=True;"+connectionString;


            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/Database.Repositories.Entities.EntityModel.csdl|res://*/Database.Repositories.Entities.EntityModel.ssdl|res://*/Database.Repositories.Entities.EntityModel.msl";
            //var dbContext = new DbContext(entityBuilder);
            context = new Entities.Entities(entityBuilder.ConnectionString);
        }

        public bool ConnectionSuccessful()
        {
            return context.Database.Exists();
        }

        public void Save()
        {
            try
            {

                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
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
