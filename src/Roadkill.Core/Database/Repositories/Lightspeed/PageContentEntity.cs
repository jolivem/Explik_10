using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("explik_pagecontent")]
	public class PageContentEntity : Entity<Guid>
	{
		[Column("text")]
		private string _text;

		[Column("controlledby")]
		private string _controlledBy;

		[Column("editedon")]
		private DateTime _editedOn;

		[Column("versionnumber")]
		private int _versionNumber;

		[ReverseAssociation("PageId")]
		private readonly EntityHolder<PageEntity> _page = new EntityHolder<PageEntity>();
		private int _pageId;

		public PageEntity Page
		{
			get
			{
				return Get(_page);
			}
			set
			{
				Set(_page, value);
			}
		}

		public int PageId
		{
			get { return Get(ref _pageId, "PageId"); }
			set { Set(ref _pageId, value, "PageId"); }
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				Set<string>(ref _text, value);
			}
		}

		public string ControlledBy
		{
			get
			{
				return _controlledBy;
			}
			set
			{
				Set<string>(ref _controlledBy, value);
			}
		}

		public DateTime EditedOn
		{
			get
			{
				return _editedOn;
			}
			set
			{
				Set<DateTime>(ref _editedOn, value);
			}
		}

		public int VersionNumber
		{
			get
			{
				return _versionNumber;
			}
			set
			{
				Set<int>(ref _versionNumber, value);
			}
		}
	}
}
