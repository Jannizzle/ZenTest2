using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// ZDKRMA contains interfaces for configuring and displaying a Rate My App dialog.
	/// </summary>
	public class ZDKRMA {
		
		private static string _logTag = "ZDKRMA";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// To show the ZDKRMA dialog in a view, call this methed
		/// </summary>
		public static void Show() {
			Log("Unity : Show");
		}

		/// <summary>
		/// To show the ZDKRMA dialog in a view, call this methed.
		/// This method will always show a dialog irrespective of
		/// the settings in ZDKRMAConfigObject.
		/// </summary>
		public static void ShowAlways() {
			Log("Unity : ShowAlways");
		}

		/// <summary>
		/// To show the ZDKRMA dialog in a view, call this methed
		/// </summary>
		/// <param name="config">A ZDKRMAConfigObject to configure the Rate My App interface.</param>
		public static void Show(ZDKRMAConfigObject config) {
			Log("Unity : ShowWithConfiguration");
		}
		
		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskRMAShowInView();
		[DllImport("__Internal")]
		private static extern void _zendeskRMAShowAlwaysInView();
		[DllImport("__Internal")]
		private static extern void _zendeskRMAShowInViewWithConfiguration(string[] additionalTags,
		                                                                  int additionalTagsLength,
		                                                                  string additionalRequestInfo,
		                                                                  ZDKRMAAction[] dialogActions,
		                                                                  int dialogActionsLength,
		                                                                  string successImageName,
		                                                                  string errorImageName);


		public static void Show() {
			_zendeskRMAShowInView();
		}

		public static void ShowAlways() {
			_zendeskRMAShowAlwaysInView();
		}

		public static void Show(ZDKRMAConfigObject config) {
			_zendeskRMAShowInViewWithConfiguration(config.AdditionalTags,
			                              		   config.AdditionalTags.Length,
			                               	 	   config.AdditionalRequestInfo,
			                             		   config.DialogActions,
			                                 	   config.DialogActions.Length,
			                                 	   config.SuccessImageName,
			                                 	   config.ErrorImageName);
		}

		#elif UNITY_ANDROID

		private static AndroidJavaObject _plugin;

		public static void Show() {
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")) {
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			//boolean to not call the showAlways method in the Activity
			_plugin.Call("showInView", false);
		} 

		public static void ShowAlways() {
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")) {
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			//boolean to call showAlways in the Activity
			_plugin.Call("showInView", true);
		}

		public static void Show(ZDKRMAConfigObject config) {

			//An array of booleans that represent which button to include in the dialog
			bool[] dialogBools = new bool[3];
			if (config.DialogActions != null) {
				for (int i = 0; i < config.DialogActions.Length; i++) {
					if (config.DialogActions [i] == ZDKRMAAction.ZDKRMARateApp) {
						dialogBools [0] = true;
					} else if (config.DialogActions [i] == ZDKRMAAction.ZDKRMASendFeedback) {
						dialogBools [1] = true;
					} else if (config.DialogActions [i] == ZDKRMAAction.ZDKRMADontAskAgain) {
						dialogBools [2] = true;
					}
				}
			}
			//If the config.DialogActions was null then the default is to add all the buttons
			else {
				for (int i = 0; i < dialogBools.Length; i++) {
					dialogBools[i] = true;
				}
			}
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")) {
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("showInViewWithConfig", false, config.AdditionalTags,
			             config.AdditionalRequestInfo,
			             dialogBools,
			             config.RequestSubject);
		}

		#endif

		#if UNITY_EDITOR || !UNITY_IPHONE
		
		/// <summary>
		/// Log a visit. The first call to logVisit sets the initial visitCount and sets the 
		/// initialCheckDate. visitCount and initialCheckDate are passed to the shouldShowBlock. 
		/// The default shouldShowBlock checks that a threshold of 15 visits has been reached 
		/// and that 7 days have past since the initialCheckDate. iOS only
		///
		/// You should call LogVisit where you want to track user visits.
		/// If you use the default ZDKRMA setup you need to include a call to logVisit somewhere in your code.
		/// </summary>
		public static void LogVisitiOS() {
			Log("Unity : LogVisitiOS");
		}
		
		#elif UNITY_IPHONE
		
		[DllImport("__Internal")]
		private static extern void _zendeskRMALogVisit();
		
		public static void LogVisitiOS() {
			_zendeskRMALogVisit();
		}
		
		#endif
	}
}		