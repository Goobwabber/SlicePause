﻿using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Zenject;
using PauseCoob.HarmonyPatches;
using PauseCoob.Installers;
using System.Net.Http;
using IPALogger = IPA.Logging.Logger;

namespace PauseCoob
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		public static readonly string HarmonyId = "com.github.Goobwabber.PauseCoob";

		internal static Plugin Instance { get; private set; } = null!;
		internal static PluginMetadata PluginMetadata = null!;
		internal static IPALogger Log { get; private set; } = null!;
		internal static PluginConfig Config = null!;

		internal static HttpClient HttpClient { get; private set; } = null!;
		internal static Harmony? _harmony;
		internal static Harmony Harmony
		{
			get
			{
				return _harmony ??= new Harmony(HarmonyId);
			}
		}

		[Init]
		public Plugin(IPALogger logger, Config conf, Zenjector zenjector, PluginMetadata pluginMetadata)
		{
			Instance = this;
			PluginMetadata = pluginMetadata;
			Log = logger;
			Config = conf.Generated<PluginConfig>();

			zenjector.OnMenu<MenuInstaller>();
			zenjector.OnGame<EffectInstaller>();
			zenjector.OnGame<GameInstaller>().ShortCircuitForMultiplayer();
			zenjector.Register<WarmupInstaller>().On<ShaderWarmupSceneSetup>();
		}

		[OnStart]
		public void OnApplicationStart()
		{
			HarmonyManager.ApplyDefaultPatches();
		}

		[OnExit]
		public void OnApplicationQuit()
		{

		}
	}
}
