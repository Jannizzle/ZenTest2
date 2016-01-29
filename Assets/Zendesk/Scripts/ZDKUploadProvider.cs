using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKUploadProvider {
		
		private static string _logTag = "ZDKUploadProvider";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Upload an image to Zendesk, returns a token in the response that can be used to attach the file to a request
		/// </summary>
		/// <param name="attachment">The attachment to upload</param>
		/// <param name="filename">The file name you wan't to store the image as.</param>
		/// <param name="contentType">The content type of the data, i.e: "image/png".</param>
		/// <param name="callback">Block callback executed on request error or success.</param>
		public static void UploadAttachment(string attachment, string filename, string contentType, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKUploadProvider:UploadAttachment");
		}

		/// <summary>
		/// Delete an upload from Zendesk. Will only work if the upload has not been associated with a request/ticket.
		/// </summary>
		/// <param name="uploadToken">Upload token of file to delete</param>
		/// <param name="callback">Block callback executed on request error or success.</param>
		public static void DeleteUpload(string uploadToken, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKUploadProvider:DeleteUpload");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskZDKUploadProviderUploadAttachment(string gameObjectName, string callbackId, string attachment, string filename, string contentType);
		[DllImport("__Internal")]
		private static extern void _zendeskZDKUploadProviderDeleteUpload(string gameObjectName, string callbackId, string uploadToken);

		public static void UploadAttachment(string attachment, string filename, string contentType, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskZDKUploadProviderUploadAttachment(ZendeskSDK.ZenExternal.GameObjectName, callbackId, attachment, filename, contentType);
		}

		public static void DeleteUpload(string uploadToken, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskZDKUploadProviderDeleteUpload(ZendeskSDK.ZenExternal.GameObjectName, callbackId, uploadToken);
		}

		#elif UNITY_ANDROID

		public static void UploadAttachment(string attachment, string filename, string contentType, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("uploadAttachment",ZendeskSDK.ZenExternal.GameObjectName, callbackId, attachment, filename, contentType);
		}
		
		public static void DeleteUpload(string uploadToken, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("deleteUpload",ZendeskSDK.ZenExternal.GameObjectName, callbackId, uploadToken);
		}

		#endif
	}
}		