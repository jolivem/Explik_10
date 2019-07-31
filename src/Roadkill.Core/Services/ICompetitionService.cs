using Roadkill.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Services
{
    public interface ICompetitionService
    {

        CompetitionViewModel GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PageViewModel> GetCompetitionPages(int id);

        /// <summary>
        /// Add a new competition
        /// </summary>
        /// <param name="model"></param>
        void AddCompetition(CompetitionViewModel model, bool debug=false);

        /// <summary>
        /// Update the dates of the competition, or th state
        /// </summary>
        /// <param name="competition"></param>
        void UpdateCompetition(CompetitionViewModel competition);

        /// <summary>
        /// Get all compeitions for display
        /// </summary>
        /// <returns></returns>
        List<CompetitionViewModel> GetCompetitions(bool forAdmin = false);

        /// <summary>
        /// Get 
        /// </summary>
        /// <returns>status</returns>
        Competition GetCompetitionByStatus( CompetitionViewModel.Statuses status);


        /// <summary>
        /// Close everything related to a competition
        /// </summary>
        /// <param name="id"></param>
        void Achieve(int competitionId);
    }
}
