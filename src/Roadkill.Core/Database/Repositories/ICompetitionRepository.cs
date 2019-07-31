using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database
{
    public interface ICompetitionRepository
    {
        /// <summary>
        /// Create new competition
        /// </summary>
        /// <param name="competition"></param>
        void AddCompetition( Competition competition);

        /// <summary>
        /// Update the dates of the competition, or th state
        /// </summary>
        /// <param name="competition"></param>
        void UpdateCompetition(Competition competition);

        /// <summary>
        /// Update the pageId of the page tag of the competition
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="pageId"></param>
        void UpdateCompetitionPageId(int competitionId, int pageId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="pageId"></param>
        /// <param name="rarnking"></param>
        void UpdateCompetitionPageRanking(int competitionId, int pageId, int ranking);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        int GetPageRanking(int pageId);

        /// <summary>
        /// Get all compeitions for display
        /// </summary>
        /// <returns></returns>
        IEnumerable<Competition> GetCompetitions(bool forAdmin = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Competition GetCompetitionById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Competition GetCompetitionByPageTag(string tag);

        Competition GetCompetitionByStatus(int status);

        /// <summary>
        /// Get all pages registered for a given competition
        /// </summary>
        /// <param name="competitionId"></param>
        /// <returns></returns>
        IEnumerable<CompetitionPage> GetCompetitionPages(int competitionId);

        /// <summary>
        /// Delete a given competition
        /// </summary>
        /// <param name="id"></param>
        void DeleteCompetition(int id);

        /// <summary>
        /// Add a page in a given competition
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="page"></param>
        void ArchiveCompetitionPage(int competitionId, Page page);

 
        int[] GetUserHits(string username);

    }
}
