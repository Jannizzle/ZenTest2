using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {
	
	public class ZDKStringUtil {

		private static string _logTag = "ZDKStringUtil";
		
		public static void Log (string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// This method converts an array of strings into a comma separated string of the array's items. 
		/// For example an array with 
		/// three items, "one", "two" and "three" will be converted into the string "one,two,three".
		/// </summary>
		/// <param name="strings">An array of Strings to convert into a comma-separated string</param>
		/// <returns>A comma separated string of the items in the array or an empty string if there were none.</returns>
		public static string CsvStringFromArray(string[] strings) {
			Log("Unity : csvStringFromArray");
			return "";
		}
		
		#elif UNITY_IPHONE
		
		[DllImport("__Internal")]
		private static extern string _zendeskCsvStringFromArray(string[] charArray, int length);
		
		public static string CsvStringFromArray(string[] strings) {
			return _zendeskCsvStringFromArray(strings, strings.Length);
		}
		
		#elif UNITY_ANDROID

		public static string CsvStringFromArray(string[] strings) {
			AndroidJavaObject _plugin;
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			//Empty string is being passed in since string[] bring complications with parameters
			//when translated into java
			return _plugin.Call<String> ("zendeskCsvStringFromArray", strings, "");
		}
		
		#endif
	}
}
