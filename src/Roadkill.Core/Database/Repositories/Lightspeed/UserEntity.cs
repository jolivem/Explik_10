﻿using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("explik_users")]
	[Cached(ExpiryMinutes = 1)]
	public class UserEntity : Entity<Guid>
	{
		[Column("username")]
		private string _username;

		[Column("email")]
		private string _email;

		[Column("firstname")]
		private string _firstname;

		[Column("lastname")]
		private string _lastname;

		[Column("password")]
		private string _password;

		[Column("salt")]
		private string _salt;

        [Column("iseditor")]
        private bool _isEditor;

        [Column("iscontroller")]
        private bool _isController;

        [Column("attachmentspath")]
        private string _attachmentsPath;

        [Column("isadmin")]
		private bool _isAdmin;

		[Column("isactivated")]
		private bool _isActivated;

        [Column("contributionlevel")]
        private long _contributionLevel;

        [Column("displayflags")]
        private long _displayFlags;

        [Column("activationkey")]
		private string _activationKey;

		[Column("passwordresetkey")]
		private string _passwordResetKey;

		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				Set<string>(ref _username, value);
			}
		}

		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				Set<string>(ref _email, value);
			}
		}

		public string Firstname
		{
			get
			{
				return _firstname;
			}
			set
			{
				Set<string>(ref _firstname, value);
			}
		}

		public string Lastname
		{
			get
			{
				return _lastname;
			}
			set
			{
				Set<string>(ref _lastname, value);
			}
		}

		internal string Password
		{
			get
			{
				return _password;
			}
			set
			{
				Set<string>(ref _password, value);
			}
		}

		internal string Salt
		{
			get
			{
				return _salt;
			}
			set
			{
				Set<string>(ref _salt, value);
			}
		}

        public bool IsEditor
        {
            get
            {
                return _isEditor;
            }
            set
            {
                Set<bool>(ref _isEditor, value);
            }
        }

        public bool IsController
        {
            get
            {
                return _isController;
            }
            set
            {
                Set<bool>(ref _isController, value);
            }
        }

        public string AttachmentsPath
        {
            get
            {
                return _attachmentsPath;
            }
            set
            {
                Set<string>(ref _attachmentsPath, value);
            }
        }

        public bool IsAdmin
		{
			get
			{
				return _isAdmin;
			}
			set
			{
				Set<bool>(ref _isAdmin, value);
			}
		}

		public bool IsActivated
		{
			get
			{
				return _isActivated;
			}
			set
			{
				Set<bool>(ref _isActivated, value);
			}
		}

        public long ContributionLevel
        {
            get
            {
                return _contributionLevel;
            }
            set
            {
                Set<long>(ref _contributionLevel, value);
            }
        }

        public long DisplayFlags
        {
            get
            {
                return _displayFlags;
            }
            set
            {
                Set<long>(ref _displayFlags, value);
            }
        }

        public string ActivationKey
		{
			get
			{
				return _activationKey;
			}
			set
			{
				Set<string>(ref _activationKey, value);
			}
		}

		public string PasswordResetKey
		{
			get
			{
				return _passwordResetKey;
			}
			set
			{
				Set<string>(ref _passwordResetKey, value);
			}
		}

	}
}
