using Roadkill.Core.Database;
using Roadkill.Core.Localization;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Roadkill.Core.Mvc.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class CompetitionViewModel
    {

        public enum Statuses
        {
            Init,
            PublicationOngoing,
            PauseBeforeRating,
            RatingOngoing,
            PauseBeforeAchieved, // do not display anything
            Achieved,
        }

        public int Id { get; set; }
        public DateTime PublicationStart { get; set; }
        public DateTime PublicationStop { get; set; }
        public DateTime RatingStart { get; set; }
        public DateTime RatingStop { get; set; }
        public Statuses Status { get; set; }
        public string PageTag { get; set; }
        public int PageId { get; set; } // -1 if  not significative
        public string PageTitle { get; set; }
        public string PageText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CompetitionViewModel()
        {
            PublicationStart = DateTime.Now;
            PublicationStop = DateTime.Now;
            RatingStart = DateTime.Now;
            RatingStop = DateTime.Now;
            Status = Statuses.Init;
            string date = String.Format("{0:yyMMdd.hhmmss}", DateTime.Now);
            PageTag = $"__Competition_{date}";
            PageId = -1;
            PageTitle = "";
            PageText = "";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="competition"></param>
        public CompetitionViewModel(Competition competition)
        {
            Id = competition.Id;
            PublicationStart = competition.PublicationStart;
            PublicationStop = competition.PublicationStop;
            RatingStart = competition.RatingStart;
            RatingStop = competition.RatingStop;
            Status = (Statuses)competition.Status;
            StatusString = StatusToString(Status);
            PageTag = competition.PageTag;
            PageId = competition.PageId;
            PageTitle = "";
            PageText = "";
            //Page Title and text shall be set later
        }

        public string StatusString { get; set; }
  
        public string DateString
        {
            get
            {
                if (Status == Statuses.Achieved)
                {
                    return RatingStop.ToString("MM/yyyy");
                }
                else
                {
                    return RatingStop.ToString("dd/MM/yyyy");
                }
            }
        }

        public string PublicationStartString
        {
            get
            {
                 return PublicationStart.ToString("dd/MM/yyyy");
            }
            set
            {
                PublicationStart = DateTime.ParseExact(value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }

        public string PublicationStopString
        {
            get
            {
                return PublicationStop.ToString("dd/MM/yyyy");
            }
            set
            {
                PublicationStop = DateTime.ParseExact(value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }
        public string RatingStartString
        {
            get
            {
                return RatingStart.ToString("dd/MM/yyyy");
            }
            set
            {
                RatingStart = DateTime.ParseExact(value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }
        public string RatingStopString
        {
            get
            {
                return RatingStop.ToString("dd/MM/yyyy");
            }
            set
            {
                RatingStop = DateTime.ParseExact(value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }

 
        public string EncodedTitle
        {
            get
            {
                return PageViewModel.EncodePageTitle(PageTitle);
            }
        }

        public IEnumerable<string> StatusesList
        {
            get
            {
                return new string[]
                {
                    SiteStrings.Competition_StatusInit,
                    SiteStrings.Competition_StatusPublication,
                    SiteStrings.Competition_StatusPauseBeforeRating,
                    SiteStrings.Competition_StatusRating,
                    SiteStrings.Competition_StatusPauseBeforeAchieved,
                    SiteStrings.Competition_StatusAchieved,
                };
            }
        }

        public static  Statuses StatusStringToEnum( string statusString)
        {
            if (statusString == SiteStrings.Competition_StatusPublication)
                return Statuses.PublicationOngoing;
            if (statusString == SiteStrings.Competition_StatusRating)
                return Statuses.RatingOngoing;
            if (statusString == SiteStrings.Competition_StatusAchieved)
                return Statuses.Achieved;
            if (statusString == SiteStrings.Competition_StatusPauseBeforeRating)
                return Statuses.PauseBeforeRating;
            if (statusString == SiteStrings.Competition_StatusPauseBeforeAchieved)
                return Statuses.PauseBeforeAchieved;

            //else

            return Statuses.Init;
        }

        public static string StatusToString(Statuses status)
        {
            switch (status)
            {
                case Statuses.Achieved:
                    return SiteStrings.Competition_StatusAchieved;
                case Statuses.Init:
                    return SiteStrings.Competition_StatusInit;
                case Statuses.PublicationOngoing:
                    return SiteStrings.Competition_StatusPublication;
                case Statuses.RatingOngoing:
                    return SiteStrings.Competition_StatusRating;
                case Statuses.PauseBeforeRating:
                    return SiteStrings.Competition_StatusPauseBeforeRating;
                case Statuses.PauseBeforeAchieved:
                    return SiteStrings.Competition_StatusPauseBeforeAchieved;
            }

            return SiteStrings.Competition_StatusInit;
        }
    }
}
