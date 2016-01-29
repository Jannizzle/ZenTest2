using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// The core communications controller.
	/// </summary>
	public class ZDKDispatcher {
		
		private static string _logTag = "ZDKDispatcher";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}
		
		#if UNITY_EDITOR || !UNITY_IPHONE

		/// <summary>
		/// Enable or disable debug logging output. iOS Only
		/// </summary>
		/// <param name="enabled">YES for debug logging</param>
		public static void SetDebugLoggingiOS(bool enabled) {
			Log("Unity : SetDebugLoggingiOS");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskDispatcherSetDebugLogging(bool enabled);

		public static void SetDebugLoggingiOS(bool enabled) {
			_zendeskDispatcherSetDebugLogging(enabled);
		}

		#endif
	}
}		