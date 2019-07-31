using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms.VisualStyles;

using Lucene.Net.Documents;
using Lucene.Net.Search;

using Roadkill.Core.Database;

namespace Roadkill.Core.Mvc.ViewModels
{
	/// <summary>
	/// Contains data for a single search result from a search query.
	/// </summary>
	public class SearchResultViewModel
	{
        private global::Lucene.Net.Documents.Document document;
        private global::Lucene.Net.Search.ScoreDoc scoreDoc;

        /// <summary>
        /// The page id 
        /// </summary>
        public int Id { get; internal set; }

		/// <summary>
		/// The page title.
		/// </summary>
		public string Title { get; internal set; }

		/// <summary>
		/// The page title, encoded so it is a safe search-engine friendly url.
		/// </summary>
		public string EncodedTitle
		{
			get
			{
				return PageViewModel.EncodePageTitle(Title);
			}
		}

		/// <summary>
		/// The summary of the content (the first 150 characters of text with all HTML removed).
		/// </summary>
		public string ContentSummary { get; internal set; }

		/// <summary>
		/// The length of the content in bytes.
		/// </summary>
		public int ContentLength { get; internal set; }

		// TODO: tests
		/// <summary>
		/// Formats the page length in bytes using KB or bytes if it is less than 1024 bytes.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <param name="size">The size in bytes.</param>
		/// <returns>If the size parameter is 900: 900 bytes. If size is 4000: 4KB.</returns>
		public string ContentLengthInKB
		{
			get
			{
				if (ContentLength > 1024)
					return ContentLength / 1024 + "KB";
				else
					return ContentLength + " bytes";
			}
		}

		/// <summary>
		/// The person who created the page.
		/// </summary>
		public string CreatedBy { get; internal set; }

		/// <summary>
		/// The date the page was created on.
		/// </summary>
		public DateTime CreatedOn { get; internal set; }

		/// <summary>
		/// The tags for the page, in space delimited format.
		/// </summary>
		public string Tags { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public long NbView { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public double Rating { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public long TotalRating { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        //public string Canvas { get; internal set; }


        /// <summary>
        /// The lucene.net score for the search result.
        /// </summary>
        public float Score { get; internal set; }

		public SearchResultViewModel()
		{
		}

		public SearchResultViewModel(Document document, ScoreDoc scoreDoc, Page page)
		{
			if (document == null)
				throw new ArgumentNullException("document");

			if (scoreDoc == null)
				throw new ArgumentNullException("scoreDoc");

			EnsureFieldsExist(document);

			Id = int.Parse(document.GetField("id").StringValue);
			Title = document.GetField("title").StringValue;
			ContentSummary = document.GetField("contentsummary").StringValue;
			Tags = document.GetField("tags").StringValue;
			CreatedBy = document.GetField("createdby").StringValue;		
			ContentLength = int.Parse(document.GetField("contentlength").StringValue);
            Score = scoreDoc.Score;

			DateTime createdOn = DateTime.UtcNow;
			if (!DateTime.TryParse(document.GetField("createdon").StringValue, out createdOn))
				createdOn = DateTime.UtcNow;

			CreatedOn = createdOn;

		    NbView = page.NbView;
		    if (page.NbRating > 0)
		    {
		        Rating = (double)page.TotalRating / (double)page.NbRating;
		    }
		    else
		    {
		        Rating = 0.0;
		    }
		    //Canvas = page.FilePath + "page_" + page.Id + ".png";

		}

        public SearchResultViewModel(global::Lucene.Net.Documents.Document document, global::Lucene.Net.Search.ScoreDoc scoreDoc)
        {
            this.document = document;
            this.scoreDoc = scoreDoc;
        }
        /*                                 @foreach (string tag in result.TagsAsList())
                                {
                                    if (!string.IsNullOrWhiteSpace(tag))
                                    {
                                        <span class="searchresult-tags">@tag &nbsp;</span>
                                    }
                                }
                                @if (result.Tags != null)
                                {
                                    <br/>
                                }
                                */
        public string EncodeTags()
        {
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='searchresult-tags'>{0}&nbsp;</span>";
            bool atLeastOne = false;
            foreach (string tag in TagsAsList())
            {
                if (!string.IsNullOrWhiteSpace(tag))
                {
                    builder.AppendFormat(formatStr, tag);
                }
                atLeastOne = true;
            }

            // add carriage return if necessary
            if (atLeastOne)
            {
                builder.Append("<br />");
            }
             return builder.ToString();
        }

        public string EncodePageRating(double rating)
        {
 
            StringBuilder builder = new StringBuilder();
            string formatStr = "<span class='rating16 stars16 passive star16-{0}' value='{1}'></span>";

            //rating = Math.Round(rating, 1);

            for (double i = .5; i <= 5.0; i = i + .5)
            {
                if (i <= rating)
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_on" : "right_on", i);
                }
                else
                {
                    builder.AppendFormat(formatStr, (i * 2) % 2 == 1 ? "left_off" : "right_off", i);
                }
            }
            //builder.AppendFormat("rating={0}", rating);
            return builder.ToString();
        }

        private void EnsureFieldsExist(Document document)
		{
			IList<IFieldable> fields = document.GetFields();
			EnsureFieldExists(fields, "id");
			EnsureFieldExists(fields, "title");
			EnsureFieldExists(fields, "contentsummary");
			EnsureFieldExists(fields, "tags");
			EnsureFieldExists(fields, "createdby");
			EnsureFieldExists(fields, "contentlength");
            EnsureFieldExists(fields, "createdon");
        }

        private void EnsureFieldExists(IList<IFieldable> fields, string fieldname)
		{
			if (fields.Any(x => x.Name == fieldname) == false)
				throw new SearchException(null, "The LuceneDocument did not contain the expected field '{0}'", fieldname);
		}

		/// <summary>
		/// Used by the search results view, for the list of tags.
		/// </summary>
		public IEnumerable<string> TagsAsList()
		{
			List<string> tags = new List<string>();

			if (!string.IsNullOrEmpty(Tags))
			{
				if (Tags.IndexOf(" ") != -1)
				{
					string[] parts = Tags.Split(' ');
					foreach (string item in parts)
					{
						if (item != " ")
							tags.Add(item);
					}
				}
				else
				{
					tags.Add(Tags.TrimEnd());
				}
			}

			return tags;
		}
	}
}
