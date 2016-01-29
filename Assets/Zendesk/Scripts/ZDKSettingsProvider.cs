using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKSettingsProvider {
		
		private static string _logTag = "ZDKSettingsProvider";
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Get SDK Settings from Zendesk instance
		/// </summary>
		/// <param name="callback">block callback invoked on success and error states</param>
		public static void GetSdkSettings(Action<Hashtable,ZDKError> settingsCallback) {
			Log("Unity : ZDKSettingsProvider:GetSdkSettings");

		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskSettingsProviderGetSettings(string gameObjectName, string callbackId);

		public static void GetSdkSettings(Action<Hashtable,ZDKError> settingsCallback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, settingsCallback);
			_zendeskSettingsProviderGetSettings(ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}

		#elif UNITY_ANDROID

		public static void GetSdkSettings(Action<Hashtable,ZDKError> settingsCallback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,settingsCallback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskSettingsProviderGetSettings", ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}

		#endif

		#if UNITY_EDITOR || !UNITY_IPHONE

		/// <summary>
		/// Get SDK Settings from Zendesk instance using the specified locale. iOS only
		/// </summary>
		/// <param name="locale">IETF language code. Config returned from server will contain 
		/// this string if the local is supported, will be the default locale otherwise</param>
		/// <param name="callback">block callback invoked on success and error states</param>
		public static void GetSdkSettingsiOS(string locale, Action<Hashtable,ZDKError> settingsCallback) {
			Log("Unity : ZDKSettingsProvideriOS:GetSdkSettingsWithLocale");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskSettingsProviderGetSettingsWithLocale(string gameObjectName, string callbackId, string locale);
		
		public static void GetSdkSettingsiOS(string locale, Action<Hashtable,ZDKError> settingsCallback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, settingsCallback);
			_zendeskSettingsProviderGetSettingsWithLocale(ZendeskSDK.ZenExternal.GameObjectName, callbackId, locale);
		}
		
		#endif
	}
}		
