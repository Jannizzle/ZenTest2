using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// SDK Logger
	/// </summary>
	public class ZDKLogger {
		
		private static string _logTag = "ZDKLogger";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}
		
		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Set logger enabled
		/// </summary>
		/// <param name="enabled">enable ZDKLogger wiht YES, disable with NO</param>
		public static void Enable(bool enabled) {
			Log("Unity : enable");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskLogEnable(bool enabled);

		public static void Enable(bool enabled) {
			_zendeskLogEnable(enabled);
		}
		
		#elif UNITY_ANDROID

		public static void Enable(bool enabled){
			AndroidJavaObject _plugin;
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("enableLogger", enabled);
		}
		
		#endif
	}
}		