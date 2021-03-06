﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Nexus.Client.Games;
using Nexus.Client.Settings;
using Nexus.Client.UI;
using Nexus.Client.UI.Controls;
using Nexus.Client.Util.Collections;

namespace Nexus.Client
{
	/// <summary>
	/// Selects the game for which mods will be managed.
	/// </summary>
	public partial class GameModeSelectionForm : ManagedFontForm
	{
		private const string RESCAN_INSTALLED_GAMES = "__rescaninstalledgames";

		/// <summary>
		/// A dummy game mode descriptor that is used as a placeholder
		/// for the rescan item in the game list.
		/// </summary>
		private class RescanGameModeDescriptor : IGameModeDescriptor
		{
			#region Properties

			/// <summary>
			/// Gets the display name of the game mode.
			/// </summary>
			/// <value>The display name of the game mode.</value>
			public string Name
			{
				get
				{
					return "Rescan Installed Games";
				}
			}

			/// <summary>
			/// Gets the unique id of the game mode.
			/// </summary>
			/// <value>The unique id of the game mode.</value>
			public string ModeId
			{
				get
				{
					return RESCAN_INSTALLED_GAMES;
				}
			}

			/// <summary>
			/// Gets the list of possible executable files for the game.
			/// </summary>
			/// <value>The list of possible executable files for the game.</value>
			public string[] GameExecutables
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the path to which mod files should be installed.
			/// </summary>
			/// <value>The path to which mod files should be installed.</value>
			public string InstallationPath
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the secondary path to which mod files should be installed.
			/// </summary>
			/// <value>The secondary path to which mod files should be installed.</value>
			public string SecondaryInstallationPath
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the extensions that are used by the game mode for plugin files.
			/// </summary>
			/// <value>The extensions that are used by the game mode for plugin files.</value>
			public virtual IEnumerable<string> PluginExtensions
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets a list of possible folders that should be looked for in mod archives to determine
			/// file structure.
			/// </summary>
			/// <value>A list of possible folders that should be looked for in mod archives to determine
			/// file structure.</value>
			public virtual IEnumerable<string> StopFolders
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the path to the game executable.
			/// </summary>
			/// <returns>The path to the game executable, or
			/// <c>null</c> if the path could not be determined.</returns>
			public string ExecutablePath
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the list of critical plugin names, ordered by load order.
			/// </summary>
			/// <value>The list of critical plugin names, ordered by load order.</value>
			public string[] OrderedCriticalPluginNames
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the name of the required tool (if any) for the current game mode.
			/// </summary>
			/// <value>The name of the required tool (if any) for the current game mode.</value>
			public string RequiredToolName
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the list of required tools file names, ordered by load order.
			/// </summary>
			/// <value>The list of required tools file names, ordered by load order.</value>
			public string[] OrderedRequiredToolFileNames
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the error message specific to a missing required tool.
			/// </summary>
			/// <value>The error message specific to a missing required tool.</value>
			public string RequiredToolErrorMessage
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			/// <summary>
			/// Gets the theme to use for this game mode.
			/// </summary>
			/// <value>The theme to use for this game mode.</value>
			public Theme ModeTheme
			{
				get
				{
					return new Theme(null, Color.Black, null);
				}
			}

			/// <summary>
			/// Gets the custom message for missing critical files.
			/// </summary>
			/// <value>The custom message for missing critical files.</value>
			public virtual string CriticalFilesErrorMessage
			{
				get
				{
					return null;
				}
			}

			/// <summary>
			/// Gets the directory where the game plugins are installed.
			/// </summary>
			/// <value>The directory where the game plugins are installed.</value>
			public virtual string PluginDirectory
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			#endregion
		}

		#region Properties

		/// <summary>
		/// Gets the id of the selected game mode.
		/// </summary>
		/// <value>The id of the selected game mode.</value>
		public string SelectedGameModeId
		{
			get
			{
				return glvGameMode.SelectedGameMode.ModeId;
			}
		}

		/// <summary>
		/// Gets whether a rescan of install games was requested.
		/// </summary>
		/// <value>Whether a rescan of install games was requested.</value>
		public bool RescanRequested
		{
			get
			{
				return glvGameMode.SelectedGameMode.ModeId.Equals(RESCAN_INSTALLED_GAMES);
			}
		}

		/// <summary>
		/// Gets or sets the application's user settings.
		/// </summary>
		/// <value>The application's user settings.</value>
		protected ISettings Settings { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="p_lstGameModes">The list of avaliable game modes.</param>
		/// <param name="p_setSettings">The application settings.</param>
		public GameModeSelectionForm(IList<IGameModeDescriptor> p_lstGameModes, ISettings p_setSettings)
		{
			Settings = p_setSettings;
			InitializeComponent();
			glvGameMode.MinimumSize = lblPrompt.Size;
			Icon = Properties.Resources.DefaultIcon;
			List<IGameModeDescriptor> lstSortedModes = new List<IGameModeDescriptor>(p_lstGameModes);
			lstSortedModes.Sort((x, y) => x.Name.CompareTo(y.Name));
			foreach (IGameModeDescriptor gmdInfo in lstSortedModes)
			{
				GameModeListViewItem gliGameModeItem = new GameModeListViewItem(gmdInfo);
				glvGameMode.Controls.Add(gliGameModeItem);
			}
			GameModeListViewItem gliRescan = new GameModeListViewItem(new RescanGameModeDescriptor());
			glvGameMode.Controls.Add(gliRescan);

			IGameModeDescriptor gmdDefault = p_lstGameModes.Find(x => x.ModeId.Equals(p_setSettings.RememberedGameMode));
			if (gmdDefault != null)
				glvGameMode.SelectedGameMode = gmdDefault;
			else
				glvGameMode.SelectedItem = glvGameMode.Items[0];
			cbxRemember.Checked = Settings.RememberGameMode;
		}

		#endregion

		/// <summary>
		/// Hanldes the <see cref="Control.Click"/> event of the OK button.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">An <see cref="EventArgs"/> describing the event arguments.</param>
		private void butOK_Click(object sender, EventArgs e)
		{
			StartupGameMode();
		}

		/// <summary>
		/// Hanldes the <see cref="Control.MouseDoubleClick"/> event of the controls.
		/// </summary>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">An <see cref="MouseEventArgs"/> describing the event arguments.</param>
		private void glvGameMode_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			StartupGameMode();
		}

		/// <summary>
		/// Handles the <see cref="GameModeListView.SelectedItemChanged"/> event of the game mode
		/// selection box.
		/// </summary>
		/// <remarks>
		/// This changes the icon to refect the currently selected game mode.
		/// </remarks>
		/// <param name="sender">The object that raised the event.</param>
		/// <param name="e">An <see cref="SelectedItemEventArgs"/> describing the event arguments.</param>
		private void glvGameMode_SelectedItemChanged(object sender, SelectedItemEventArgs e)
		{
			if (glvGameMode.SelectedGameMode.ModeTheme.Icon != null)
				Icon = glvGameMode.SelectedGameMode.ModeTheme.Icon;
		}

		/// <summary>
		/// Starts the mod manager for the selected game mode.
		/// </summary>
		/// <remarks>
		/// This makes the mod manager remember the selected game, if requested.
		/// </remarks>
		private void StartupGameMode()
		{
			if (!RescanRequested)
			{
				Settings.RememberGameMode = cbxRemember.Checked;
				Settings.RememberedGameMode = SelectedGameModeId;
				Settings.Save();
			}
			DialogResult = DialogResult.OK;
		}
	}
}
