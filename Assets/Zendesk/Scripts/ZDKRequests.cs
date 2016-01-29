using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// Request creation config object.
	/// </summary>
	public class ZDKRequestCreationConfig {
		private static string _logTag = "ZDKRequestCreationConfig";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		/// <summary>
		/// Tags to be included when creating a request.
		/// </summary>
		public string[] Tags;

		/// <summary>
		/// Additional free text to be appended to the request description.
		/// </summary>
		public string AdditionalRequestInfo;

		/// <summary>
		/// The subject of the request. Android only 
		/// </summary>
		public string RequestSubject;
		
		public ZDKRequestCreationConfig() {
			Tags = null;
			AdditionalRequestInfo = null;
			RequestSubject = null;
		}
	}

	/// <summary>
	/// Core SDK class providing access to request deflection, creation and lists.
	/// </summary>
	public class ZDKRequests {
		
		private static string _logTag = "ZDKRequests";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Displays a simple request creation modal.
		/// </summary>
		public static void ShowRequestCreation() {
			Log("Unity : ShowRequestCreation");
		}

		/// <summary>
		/// Displays a simple request creation modal.
		/// </summary>
		/// <param name="config">Configuration for request creation</param>
		public static void ShowRequestCreation(ZDKRequestCreationConfig config) {
			Log("Unity : ShowRequestCreationWithConfig");
		}

		/// <summary>
		/// Displays a request list.
		/// </summary>
		public static void ShowRequestList() {
			Log("Unity : ShowRequestList");
		}

		#elif UNITY_IPHONE
		
		[DllImport("__Internal")]
		private static extern void _zendeskShowRequestCreationWithNavController();
		[DllImport("__Internal")]
		private static extern void _zendeskShowRequestCreationWithNavControllerWithConfig(string[] tags,
		                                                                                  int tagsLength,
		                                                                                  string additionalRequestInfo);
		[DllImport("__Internal")]
		private static extern void _zendeskShowRequestListWithNavController();

		public static void ShowRequestCreation() {
			_zendeskShowRequestCreationWithNavController();
		}

		public static void ShowRequestCreation(ZDKRequestCreationConfig config) {
			_zendeskShowRequestCreationWithNavControllerWithConfig(config.Tags,
			                                                       config.Tags.Length,
			                                       				   config.AdditionalRequestInfo);
		}

		public static void ShowRequestList() {
			_zendeskShowRequestListWithNavController();
		}
		
		#elif UNITY_ANDROID
		private static AndroidJavaObject _plugin;
		
		public static void ShowRequestCreation(){
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("showRequestCreation");
		}

		public static void ShowRequestCreation(ZDKRequestCreationConfig config) {
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("showRequestCreationWithConfig", config.Tags, config.AdditionalRequestInfo, config.RequestSubject);
		}

		public static void ShowRequestList(){
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("showRequestList");
		}
		
		#endif
	}
}		