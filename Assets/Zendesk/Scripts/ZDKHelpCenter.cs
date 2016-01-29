using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKHelpCenter {
		
		private static string _logTag = "ZDKHelpCenter";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}
		
		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Displays the Help Center view
		/// </summary>
		public static void ShowHelpCenter() {
			Log("Unity : ShowHelpCenter");
		}
		
		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskShowHelpCenterWithNavController();

		public static void ShowHelpCenter() {
			_zendeskShowHelpCenterWithNavController();
		}
		
		#elif UNITY_ANDROID

		public static void ShowHelpCenter(){
			AndroidJavaObject _plugin;
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("showHelpCenter");
		}

		#endif
	}
}		

