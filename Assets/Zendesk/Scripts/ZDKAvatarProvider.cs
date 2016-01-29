using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKAvatarProvider {
		
		private static string _logTag = "ZDKAvatarProvider";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}
		
		#if UNITY_EDITOR || !UNITY_IPHONE

		/// <summary>
		/// Get the image/avatar data for a given URL.
		/// iOS only
		/// </summary>
		/// <param name="avatarUrl">String url of the image to be fetched.</param>
		/// <param name="callback">block callback executed on error or success states</param>
		public static void GetAvatar(string avatarUrl, Action<byte[],ZDKError> callback) {
			Log("Unity : ZDKAvatarProvider:GetAvatar");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskAvatarProviderGetAvatar(string gameObjectName, string callbackId, string avatarUrl);

		public static void GetAvatar(string avatarUrl, Action<byte[],ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskAvatarProviderGetAvatar(ZendeskSDK.ZenExternal.GameObjectName, callbackId, avatarUrl);
		}

		#endif
	}
}		