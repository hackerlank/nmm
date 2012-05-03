﻿using System;
using System.Collections.Generic;
using System.IO;
using ChinhDo.Transactions;
using Nexus.Client.Games.Tools;
using Nexus.Client.ModManagement;
using Nexus.Client.ModManagement.InstallationLog;
using Nexus.Client.Mods;
using Nexus.Client.PluginManagement;
using Nexus.Client.PluginManagement.InstallationLog;
using Nexus.Client.PluginManagement.OrderLog;
using Nexus.Client.Plugins;
using Nexus.Client.Settings.UI;
using Nexus.Client.Updating;
using Nexus.Client.Util;
using Nexus.Client.Util.Collections;

namespace Nexus.Client.Games
{
	/// <summary>
	/// The base class for game modes.
	/// </summary>
	/// <remarks>
	/// A Game Mode is a state in which the programme manages plugins for a specific game.
	/// </remarks>
	public abstract class GameModeBase : IGameMode, IDisposable
	{
		/// <summary>
		/// The class that encasulates game mode specific environment info.
		/// </summary>
		private class GameModeInfo : IGameModeEnvironmentInfo
		{
			#region Properties

			/// <summary>
			/// Gets or sets the application's environement info.
			/// </summary>
			/// <value>The application's environement info.</value>
			protected IEnvironmentInfo EnvironmentInfo { get; set; }

			/// <summary>
			/// Gets or sets the game mode to which this info belongs.
			/// </summary>
			/// <value>The game mode to which this info belongs.</value>
			protected IGameMode GameMode { get; set; }

			/// <summary>
			/// Gets the path to which mod files should be installed.
			/// </summary>
			/// <value>The path to which mod files should be installed.</value>
			public string InstallationPath
			{
				get
				{
					return GameMode.InstallationPath;
				}
			}

			/// <summary>
			/// Gets the directory where installation information is stored for this game mode.
			/// </summary>
			/// <remarks>
			/// This is where install logs, overwrites, and the like are stored.
			/// </remarks>
			/// <value>The directory where installation information is stored for this game mode.</value>
			public string InstallInfoDirectory
			{
				get
				{
					if (EnvironmentInfo.Settings.InstallInfoFolder.ContainsKey(GameMode.ModeId))
						return (string)EnvironmentInfo.Settings.InstallInfoFolder[GameMode.ModeId];
					return null;
				}
			}

			/// <summary>
			/// Gets the directory where overwrites are stored for this game mode.
			/// </summary>
			/// <value>The directory where overwrites are stored for this game mode.</value>
			public string OverwriteDirectory
			{
				get
				{
					string strDirectory = InstallInfoDirectory;
					if (String.IsNullOrEmpty(strDirectory))
						return null;
					strDirectory = Path.Combine(strDirectory, "overwrites");
					return strDirectory;
				}
			}

			/// <summary>
			/// Gets the path of the directory where this Game Mode's mods are stored.
			/// </summary>
			/// <value>The path of the directory where this Game Mode's mods are stored.</value>
			public string ModDirectory
			{
				get
				{
					if (!EnvironmentInfo.Settings.ModFolder.ContainsKey(GameMode.ModeId))
						return null;
					string strDirectory = (string)EnvironmentInfo.Settings.ModFolder[GameMode.ModeId];
					return strDirectory;
				}
			}

			/// <summary>
			/// Gets the path of the directory where this Game Mode's mods' cache files are stored.
			/// </summary>
			/// <value>The path of the directory where this Game Mode's mods' cache files are stored.</value>
			public string ModCacheDirectory
			{
				get
				{
					string strDirectory = ModDirectory;
					if (String.IsNullOrEmpty(strDirectory))
						return null;
					strDirectory = Path.Combine(strDirectory, "cache");
					return strDirectory;
				}
			}

			/// <summary>
			/// Gets the path of the directory where this Game Mode's mods' partial download files are stored.
			/// </summary>
			/// <value>The path of the directory where this Game Mode's mods' partial download files are stored.</value>
			public string ModDownloadCacheDirectory
			{
				get
				{
					string strDirectory = ModDirectory;
					if (String.IsNullOrEmpty(strDirectory))
						return null;
					strDirectory = Path.Combine(strDirectory, "downloads");
					return strDirectory;
				}
			}

			#endregion

			/// <summary>
			/// A simple constructor that initializes the object with the required dependencies.
			/// </summary>
			/// <param name="p_gmdGameMode">The game mode to which this info belongs.</param>
			/// <param name="p_eifEnvironmentInfo">The application's environement info.</param>
			public GameModeInfo(IGameMode p_gmdGameMode, IEnvironmentInfo p_eifEnvironmentInfo)
			{
				GameMode = p_gmdGameMode;
				EnvironmentInfo = p_eifEnvironmentInfo;
			}
		}

		private IEnvironmentInfo m_eifEnvironmentInfo = null;
		private IGameModeDescriptor m_gmdGameModeInfo = null;

		#region Properties

		/// <summary>
		/// Gets or sets the application's environement info.
		/// </summary>
		/// <value>The application's environement info.</value>
		protected IEnvironmentInfo EnvironmentInfo
		{
			get
			{
				if (m_eifEnvironmentInfo == null)
					throw new InvalidOperationException("The Game Mode's EnvironmentInfo has not been set.");
				return m_eifEnvironmentInfo;
			}
			set
			{
				m_eifEnvironmentInfo = value;
			}
		}

		/// <summary>
		/// Gets the display name of the game mode.
		/// </summary>
		/// <value>The display name of the game mode.</value>
		public string Name
		{
			get
			{
				return m_gmdGameModeInfo.Name;
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
				return m_gmdGameModeInfo.ModeId;
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
				return m_gmdGameModeInfo.InstallationPath;
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
				return m_gmdGameModeInfo.GameExecutables;
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
				return m_gmdGameModeInfo.OrderedCriticalPluginNames;
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
				return m_gmdGameModeInfo.ModeTheme;
			}
		}

		/// <summary>
		/// Gets the information about the game mode's environement.
		/// </summary>
		/// <value>The information about the game mode's environement.</value>
		public IGameModeEnvironmentInfo GameModeEnvironmentInfo { get; private set; }

		/// <summary>
		/// Gets the version of the installed game.
		/// </summary>
		/// <value>The version of the installed game.</value>
		public abstract Version GameVersion { get; }

		/// <summary>
		/// Gets a list of paths to which the game mode writes.
		/// </summary>
		/// <value>A list of paths to which the game mode writes.</value>
		public abstract IEnumerable<string> WritablePaths { get; }

		/// <summary>
		/// Gets the exported settings groups specific to the game mode.
		/// </summary>
		/// <returns>The exported settings groups specific to the game mode.</returns>
		public IEnumerable<ISettingsGroupView> SettingsGroupViews { get; protected set; }

		/// <summary>
		/// Gets the game launcher for the game mode.
		/// </summary>
		/// <value>The game launcher for the game mode.</value>
		public abstract IGameLauncher GameLauncher { get; }

		/// <summary>
		/// Gets the tool launcher for the game mode.
		/// </summary>
		/// <value>The tool launcher for the game mode.</value>
		public abstract IToolLauncher GameToolLauncher { get; }

        /// <summary>
        /// Sets if the game mode uses plugins or normal mods.
        /// </summary>
        /// <value>True for game using Gamebryo-like plugins</value>
        public virtual bool UsesPlugins
        {
            get 
            {
                return true;
            }
        }

		#endregion

		#region Constructors

		/// <summary>
		/// A simple constructor that initializes the object with the given values.
		/// </summary>
		/// <param name="p_eifEnvironmentInfo">The application's environment info.</param>
		public GameModeBase(IEnvironmentInfo p_eifEnvironmentInfo)
		{
			EnvironmentInfo = p_eifEnvironmentInfo;
			m_gmdGameModeInfo = CreateGameModeDescriptor();
			GameModeEnvironmentInfo = new GameModeInfo(this, p_eifEnvironmentInfo);
		}

		#endregion

		#region Plugin Management

		/// <summary>
		/// Gets the factory that builds plugins for this game mode.
		/// </summary>
		/// <returns>The factory that builds plugins for this game mode.</returns>
		public abstract IPluginFactory GetPluginFactory();

		/// <summary>
		/// Gets the serailizer that serializes and deserializes the list of active plugins
		/// for this game mode.
		/// </summary>
		/// <param name="p_polPluginOrderLog">The <see cref="IPluginOrderLog"/> tracking plugin order for the current game mode.</param>
		/// <returns>The serailizer that serializes and deserializes the list of active plugins
		/// for this game mode.</returns>
		public abstract IActivePluginLogSerializer GetActivePluginLogSerializer(IPluginOrderLog p_polPluginOrderLog);

		/// <summary>
		/// Gets the discoverer to use to find the plugins managed by this game mode.
		/// </summary>
		/// <returns>The discoverer to use to find the plugins managed by this game mode.</returns>
		public abstract IPluginDiscoverer GetPluginDiscoverer();

		/// <summary>
		/// Gets the serializer that serializes and deserializes the plugin order
		/// for this game mode.
		/// </summary>
		/// <returns>The serailizer that serializes and deserializes the plugin order
		/// for this game mode.</returns>
		public abstract IPluginOrderLogSerializer GetPluginOrderLogSerializer();

		/// <summary>
		/// Gets the object that validates plugin order for this game mode.
		/// </summary>
		/// <returns>The object that validates plugin order for this game mode.</returns>
		public abstract IPluginOrderValidator GetPluginOrderValidator();

		/// <summary>
		/// Determines if the given plugin is critical to the current game.
		/// </summary>
		/// <remarks>
		/// Critical plugins cannot be reordered, cannot be deleted, cannot be deactivated, and cannot have plugins ordered above them.
		/// </remarks>
		/// <param name="p_plgPlugin">The plugin for which it is to be determined whether or not it is critical.</param>
		/// <returns><c>true</c> if the specified pluing is critical;
		/// <c>false</c> otherwise.</returns>
		public bool IsCriticalPlugin(Plugin p_plgPlugin)
		{
			return OrderedCriticalPluginNames.Contains(p_plgPlugin.Filename.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar), StringComparer.OrdinalIgnoreCase);
		}

		#endregion

		#region Game Specific Value Management

		/// <summary>
		/// Gets the installer to use to install game specific values.
		/// </summary>
		/// <param name="p_modMod">The mod being installed.</param>
		/// <param name="p_ilgInstallLog">The install log to use to log the installation of the game specific values.</param>
		/// <param name="p_tfmFileManager">The transactional file manager to use to interact with the file system.</param>
		/// <returns>The installer to use to manage game specific values, or <c>null</c> if the game mode does not
		/// install any game specific values.</returns>
		/// <param name="p_futFileUtility">The file utility class.</param>
		/// <param name="p_dlgOverwriteConfirmationDelegate">The method to call in order to confirm an overwrite.</param>
		public abstract IGameSpecificValueInstaller GetGameSpecificValueInstaller(IMod p_modMod, IInstallLog p_ilgInstallLog, TxFileManager p_tfmFileManager, FileUtil p_futFileUtility, ConfirmItemOverwriteDelegate p_dlgOverwriteConfirmationDelegate);

		/// <summary>
		/// Gets the installer to use to upgrade game specific values.
		/// </summary>
		/// <param name="p_modMod">The mod being upgraded.</param>
		/// <param name="p_ilgInstallLog">The install log to use to log the installation of the game specific values.</param>
		/// <param name="p_tfmFileManager">The transactional file manager to use to interact with the file system.</param>
		/// <returns>The installer to use to manage game specific values, or <c>null</c> if the game mode does not
		/// install any game specific values.</returns>
		/// <param name="p_futFileUtility">The file utility class.</param>
		/// <param name="p_dlgOverwriteConfirmationDelegate">The method to call in order to confirm an overwrite.</param>
		public abstract IGameSpecificValueInstaller GetGameSpecificValueUpgradeInstaller(IMod p_modMod, IInstallLog p_ilgInstallLog, TxFileManager p_tfmFileManager, FileUtil p_futFileUtility, ConfirmItemOverwriteDelegate p_dlgOverwriteConfirmationDelegate);

		#endregion

		/// <summary>
		/// Gets the updaters used by the game mode.
		/// </summary>
		/// <returns>The updaters used by the game mode.</returns>
		public abstract IEnumerable<IUpdater> GetUpdaters();

		/// <summary>
		/// Adjusts the given path to be relative to the installation path of the game mode.
		/// </summary>
		/// <remarks>
		/// This is basically a hack to allow older FOMods to work. Older FOMods assumed
		/// the installation path of Fallout games to be &lt;games>/data, but this new manager specifies
		/// the installation path to be &lt;games>. This breaks the older FOMods, so this method can detect
		/// the older FOMods (or other mod formats that needs massaging), and adjusts the given path
		/// to be relative to the new instaalation path to make things work.
		/// </remarks>
		/// <param name="p_mftModFormat">The mod format for which to adjust the path.</param>
		/// <param name="p_strPath">The path to adjust</param>
		/// <returns>The given path, adjusted to be relative to the installation path of the game mode.</returns>
		public virtual string GetModFormatAdjustedPath(IModFormat p_mftModFormat, string p_strPath)
		{
			return p_strPath;
		}

		/// <summary>
		/// Creates a game mode descriptor for the current game mode.
		/// </summary>
		/// <returns>A game mode descriptor for the current game mode.</returns>
		protected abstract IGameModeDescriptor CreateGameModeDescriptor();

		#region IDisposable Members

		/// <summary>
		/// Ensures all used resources are released.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Disposes of the unamanged resources.
		/// </summary>
		/// <param name="p_booDisposing">Whether the method is being called from the <see cref="Dispose()"/> method.</param>
		protected virtual void Dispose(bool p_booDisposing)
		{
		}
	}
}
