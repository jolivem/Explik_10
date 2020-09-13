﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Roadkill.Core.Converters;
using System.Text.RegularExpressions;
using Directory = System.IO.Directory;
using LuceneVersion = Lucene.Net.Util.Version;
using Lucene.Net.Store;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Plugins;
using Roadkill.Core.MyLucene;
using Roadkill.Core.Logging;

namespace Roadkill.Core.Services
{
	/// <summary>
	/// Provides searching tasks using a Lucene.net search index.
	/// </summary>
	public class SearchService : ServiceBase
	{
       
        //private static Regex _removeTagsRegex = new Regex("<(.|\n)*?>");
		private MarkupConverter _markupConverter;
		protected virtual string IndexPath { get; set; }
		//private IPluginFactory _pluginFactory;
		private static readonly LuceneVersion LUCENEVERSION = LuceneVersion.LUCENE_29;
	    //private IRepository _repository;
	    //private ApplicationSettings _applicationSettings;

		public SearchService(ApplicationSettings applicationSettings, IRepository repository, IPluginFactory pluginFactory)//, PageService _pageService)
            : base(applicationSettings, repository)
		{
            _markupConverter = new MarkupConverter(applicationSettings, repository, pluginFactory);
			IndexPath = applicationSettings.SearchIndexPath;
		    //_repository = repository;
		    //_applicationSettings = applicationSettings;
		}

		/// <summary>
		/// Searches the lucene index with the search text.
		/// </summary>
		/// <param name="searchText">The text to search with.</param>
		/// <remarks>Syntax reference: http://lucene.apache.org/java/2_3_2/queryparsersyntax.html#Wildcard</remarks>
		/// <exception cref="SearchException">An error occured searching the lucene.net index.</exception>
		public virtual IEnumerable<PageViewModel> Search(string searchText)
		{
			// This check is for the benefit of the CI builds
			if (!Directory.Exists(IndexPath))
				CreateIndex();

			List<PageViewModel> list = new List<PageViewModel>();

			if (string.IsNullOrWhiteSpace(searchText))
				return list;

			Log.Information("searchText = {0}", searchText);

			MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
			MultiFieldQueryParser parser = new MultiFieldQueryParser(LuceneVersion.LUCENE_29, new string[] { "content", "title", "tags", "createdby" }, analyzer);
            parser.DefaultOperator = QueryParser.Operator.AND;

			Query query = null;
			try
			{
				query = parser.Parse(searchText);
			}
			catch (Lucene.Net.QueryParsers.ParseException)
			{
				// Catch syntax errors in the search and remove them.
				searchText = QueryParser.Escape(searchText);
				query = parser.Parse(searchText);
			}

			if (query != null)
			{
				try
				{
					using (IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(new DirectoryInfo(IndexPath)), true))
					{
						TopDocs topDocs = searcher.Search(query, 1000);

						foreach (ScoreDoc scoreDoc in topDocs.ScoreDocs)
						{
							Document document = searcher.Doc(scoreDoc.Doc);

						    int id = int.Parse(document.GetField("id").StringValue);
							// TODO optimyze: use cachebrowser or LUCENE DB
							PageContent content = Repository.GetLatestPageContent(id);
							if (content != null)
							{
								PageViewModel pageModel = new PageViewModel(content, _markupConverter, Repository.FindCoursesByPageId(id));
								pageModel.Ranking = Repository.GetPageRanking(id);
								list.Add(pageModel);
							}

							//Page page = _repository.GetPageById(id);
							//   if (page != null)
							//   {
							//                         //page.FilePath = _applicationSettings.AttachmentsUrlPath + "/" + page.FilePath + "/";
							//                         int ranking = _repository.GetPageRanking(id);
							//	var courses = Repository.FindCoursesByPageId(page.Id);
							//	List<CourseViewModel> allCourses = (from course in courses
							//							select new CourseViewModel(course)).ToList();


							//	var model = new SearchResultViewModel(document, scoreDoc, page, ranking, allCourses);
							//       list.Add(pageModel);
							//   }
						}
					}
				}
				catch (FileNotFoundException)
				{
					// For 1.7's change to the Lucene search path.
					CreateIndex();
				}
				catch (Exception ex)
				{
					throw new SearchException(ex, "An error occured while searching the index, try rebuilding the search index via the admin tools to fix this.");
				}
			}

			return list;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageId"></param>
		public virtual void Add(int pageId)
		{
			PageViewModel pageModel = new PageViewModel(Repository.GetLatestPageContent(pageId),
									 _markupConverter, Repository.FindCoursesByPageId(pageId));
			Add(pageModel);
		}

		/// <summary>
		/// Adds the specified page to the search index.
		/// </summary>
		/// <param name="model">The page to add.</param>
		/// <exception cref="SearchException">An error occured with the lucene.net IndexWriter while adding the page to the index.</exception>
		public virtual void Add(PageViewModel model)
		{
			try
			{
                // never index admin pages
                if (!model.IsLocked)
                {
                    EnsureDirectoryExists();

                    MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
                    using (IndexWriter writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(IndexPath)), analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        Document document = new Document();
                        document.Add(new Field("id", model.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                        document.Add(new Field("content", model.Content, Field.Store.YES, Field.Index.ANALYZED));
                        //document.Add(new Field("contentsummary", GetContentSummary(model), Field.Store.YES, Field.Index.NO));
                        document.Add(new Field("title", model.Title, Field.Store.YES, Field.Index.ANALYZED));

						// add tags, createdby and courses to "tags"
						string courses = string.Join(" ", (from course in model.AllCourses select course.Title));
						string misc = model.SpaceDelimitedTags() + " " + courses + " " + model.CreatedBy;
						document.Add(new Field("tags", misc, Field.Store.YES, Field.Index.ANALYZED));

						//document.Add(new Field("tags", model.SpaceDelimitedTags(), Field.Store.YES, Field.Index.ANALYZED));
                        //document.Add(new Field("createdby", model.CreatedBy, Field.Store.YES, Field.Index.ANALYZED));
                        //document.Add(new Field("createdon", model.CreatedOn.ToShortDateString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //document.Add(new Field("contentlength", model.Content.Length.ToString(), Field.Store.YES, Field.Index.NO));

						//string titles = string.Join(" ", (from course in model.AllCourses select course.Title));
						//document.Add(new Field("coursetitle", titles, Field.Store.YES, Field.Index.ANALYZED));

						writer.AddDocument(document);
                        writer.Optimize();
                    }
                }
			}
			catch (Exception ex)
			{
				if (!ApplicationSettings.IgnoreSearchIndexErrors)
					throw new SearchException(ex, "An error occured while adding page '{0}' to the search index", model.Title);
			}
		}

		/// <summary>
		/// Deletes the specified page from the search indexs.
		/// </summary>
		/// <param name="model">The page to remove.</param>
		/// <exception cref="SearchException">An error occured with the lucene.net IndexReader while deleting the page from the index.</exception>
		public virtual int Delete(PageViewModel model)
		{
			try
			{
				MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
				int count = 0;
				using (IndexReader reader = IndexReader.Open(FSDirectory.Open(new DirectoryInfo(IndexPath)), false))
				{
					count += reader.DeleteDocuments(new Term("id", model.Id.ToString()));
				}

				return count;
			}
			catch (Exception ex)
			{
				if (!ApplicationSettings.IgnoreSearchIndexErrors)
					throw new SearchException(ex, "An error occured while deleting page '{0}' from the search index", model.Title);
				else
					return 0;
			}
		}

		/// <summary>
		/// Deletes the pages given in the list.
		/// </summary>
		/// <param name="model">The list of pages to remove.</param>
		/// <exception cref="SearchException">An error occured with the lucene.net IndexReader while deleting the page from the index.</exception>
		public virtual int Delete(List<int> pageIds)
		{
			try
			{
				MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
				int count = 0;
				using (IndexReader reader = IndexReader.Open(FSDirectory.Open(new DirectoryInfo(IndexPath)), false))
				{
					foreach (int id in pageIds)
					{
						count += reader.DeleteDocuments(new Term("id", id.ToString()));
					}
				}

				return count;
			}
			catch (Exception ex)
			{
				if (!ApplicationSettings.IgnoreSearchIndexErrors)
					throw new SearchException(ex, "An error occured while deleting pages from the search index");
				else
					return 0;
			}
		}

		/// <summary>
		/// Updates the <see cref="Page"/> in the search index, by removing it and re-adding it.
		/// </summary>
		/// <param name="model">The page to update</param>
		/// <exception cref="SearchException">An error occured with lucene.net while deleting the page or inserting it back into the index.</exception>
		public virtual void Update(PageViewModel model)
		{
			EnsureDirectoryExists();
			Delete(model);
			Add(model);
		}

		/// <summary>
		/// Updates the <see cref="Page"/> in the search index, by removing it and re-adding it.
		/// </summary>
		/// <param name="model">The page to update</param>
		/// <exception cref="SearchException">An error occured with lucene.net while deleting the page or inserting it back into the index.</exception>
		public virtual void Update(List<int> pages)
		{
			EnsureDirectoryExists();

			Delete(pages);
			Add(pages);
		}

		public virtual void Add(List<int> pageIds)
		{
			try
			{
				MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
				using (IndexWriter writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(IndexPath)), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
				{
					int competitionId = Repository.GetOnGoingCompetitionId();
					foreach (int id in pageIds)
					{
						PageViewModel pageModel = new PageViewModel(
							Repository.GetLatestPageContent(id), _markupConverter,
							Repository.FindCoursesByPageId(id));

						// do not index admin, controlled and competitioning pages
						if (!pageModel.IsLocked && pageModel.IsControlled &&
							(competitionId == -1 || pageModel.CompetitionId != competitionId))
						{
							Document document = new Document();
							document.Add(new Field("id", pageModel.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
							document.Add(new Field("content", pageModel.Content, Field.Store.YES, Field.Index.ANALYZED));
							//document.Add(new Field("contentsummary", GetContentSummary(pageModel), Field.Store.YES, Field.Index.NO));
							document.Add(new Field("title", pageModel.Title, Field.Store.YES, Field.Index.ANALYZED));

							// add tags, createdby and courses to "tags"
							string courses = string.Join(" ", (from course in pageModel.AllCourses select course.Title));
							string misc = pageModel.SpaceDelimitedTags() + " " + courses + " " + pageModel.CreatedBy;
							document.Add(new Field("tags", misc, Field.Store.YES, Field.Index.ANALYZED));
							
							//document.Add(new Field("createdby", pageModel.CreatedBy, Field.Store.YES, Field.Index.ANALYZED));
							//document.Add(new Field("createdon", pageModel.CreatedOn.ToShortDateString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
							//document.Add(new Field("contentlength", pageModel.Content.Length.ToString(), Field.Store.YES, Field.Index.NO));

							//string titles = string.Join(" ", (from course in pageModel.AllCourses select course.Title));
							//document.Add(new Field("coursetitle", titles, Field.Store.YES, Field.Index.ANALYZED));

							writer.AddDocument(document);
						}
					}

					writer.Optimize();
				}
			}
			catch (Exception ex)
			{
				throw new SearchException(ex, "An error occured while creating the search index");
			}
		}


		/// <summary>
		/// Creates the initial search index based on all pages in the system.
		/// </summary>
		/// <exception cref="SearchException">An error occured with the lucene.net IndexWriter while adding the page to the index.</exception>
		public virtual void CreateIndex()
		{
			EnsureDirectoryExists();

			try
			{
				MyFrenchAnalyzer analyzer = new MyFrenchAnalyzer(LUCENEVERSION);
				using (IndexWriter writer = new IndexWriter(FSDirectory.Open(new DirectoryInfo(IndexPath)), analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
				{
                    int competitionId = Repository.GetOnGoingCompetitionId();
 
                    foreach (Page page in Repository.AllPages().ToList())
					{
                        // do not index admin, controlled and competitioning pages
                        if (!page.IsLocked && page.IsControlled &&
                            (competitionId == -1 || page.CompetitionId != competitionId)) 
                        {
                            PageViewModel pageModel = new PageViewModel(
								Repository.GetLatestPageContent(page.Id), _markupConverter, 
								Repository.FindCoursesByPageId(page.Id));

                            Document document = new Document();
                            document.Add(new Field("id", pageModel.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                            document.Add(new Field("content", pageModel.Content, Field.Store.YES, Field.Index.ANALYZED));
                            //document.Add(new Field("contentsummary", GetContentSummary(pageModel), Field.Store.YES, Field.Index.NO));
                            document.Add(new Field("title", pageModel.Title, Field.Store.YES, Field.Index.ANALYZED));

							// add tags, createdby and courses to "tags"
							string courses = string.Join(" ", (from course in pageModel.AllCourses select course.Title));
							string misc = pageModel.SpaceDelimitedTags() + " " + courses + " " + pageModel.CreatedBy;
							document.Add(new Field("tags", misc, Field.Store.YES, Field.Index.ANALYZED));
                            
							//document.Add(new Field("createdby", pageModel.CreatedBy, Field.Store.YES, Field.Index.ANALYZED));
                            //document.Add(new Field("createdon", pageModel.CreatedOn.ToShortDateString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            //document.Add(new Field("contentlength", pageModel.Content.Length.ToString(), Field.Store.YES, Field.Index.NO));

							writer.AddDocument(document);
                        }
					}

					writer.Optimize();
				}
			}
			catch (Exception ex)
			{
				throw new SearchException(ex, "An error occured while creating the search index");
			}
        }

        private void EnsureDirectoryExists()
		{
			try
			{
				if (!Directory.Exists(IndexPath))
					Directory.CreateDirectory(IndexPath);
			}
			catch (IOException ex)
			{
				throw new SearchException(ex, "An error occured while creating the search directory '{0}'", IndexPath);
			}
		}

		/// <summary>
		/// Converts the page summary to a lucene Document with the relevant searchable fields.
		/// </summary>
		internal string GetContentSummary(PageViewModel model)
		{
            return model.GetContentSummary(_markupConverter);
		}
	}
}
