using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKRequestProvider {
		
		private static string _logTag = "ZDKRequestProvider";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}
		
		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Calls a request service to create a request on behalf of the end-user.
		/// </summary>
		/// <param name="email">End-user's email address (Android only)</param>
		/// <param name="subject">Message describing the subject of the request</param>
		/// <param name="description">More detailed description of a problem</param>
		/// <param name="tags">List of label that mark the request</param>
		/// <param name="callback">callback invoked in response to remote API invokation</param>
		public static void CreateRequest(string email, string subject, string description, string[] tags, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:CreateRequest");
		}

		/// <summary>
		/// Calls a request service to create a request with attachments on behalf of the end-user.
		/// </summary>
		/// <param name="email">End-user's email address (Android only)</param>
		/// <param name="subject">Message describing the subject of the request</param>
		/// <param name="description">More detailed description of a problem</param>
		/// <param name="tags">List of label that mark the request</param>
		/// <param name="attachments">List of ZDKUploadResponse objects</param>
		/// <param name="callback">callback invoked in response to remote API invokation</param>
		public static void CreateRequest(string email, string subject, string description, string[] tags, string[] attachments, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:CreateRequestWithAttachments");
		}

		/// <summary>
		/// Gets all requests that user has opened.
		/// </summary>
		/// <param name="callback">callback invoked in response to remote API invokation</param>
		public static void GetAllRequests(Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:GetAllRequests");
		}

		/// <summary>
		/// Filters requests that user has opened by a status.
		/// </summary>
		/// <param name="status">A comma separated list of status to filter the results by</param>
		/// <param name="">The callback to invoke which will return a list of requests</param>
		public static void GetAllRequests(string status, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:GetAllRequestsByStatus");
		}

		/// <summary>
		/// Gets all comments for a request.
		/// </summary>
		/// <param name="requestId">Id of a request</param>
		/// <param name="callback">callback invoked in response to remote API invokation</param>
		public static void GetComments(string requestId, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:GetCommentsWithRequestId");
		}

		/// <summary>
		/// Add a comment message to a request.
		/// </summary>
		/// <param name="comment">The text of the comment to create</param>
		/// <param name="requestId">Id of a request to add this comment to</param>
		/// <param name="callback">Callback that will deliver a ZDKComment</param>
		public static void AddComment(string comment, string requestId, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:AddComment");
		}

		/// <summary>
		/// Add a comment message to a request with attachments on behalf of the end-user.
		/// </summary>
		/// <param name="comment">The text of the comment to create</param>
		/// <param name="requestId">Id of a request to add this comment to</param>
		/// <param name="attachments">List of ZDKUploadResponse objects</param>
		/// <param name="callBack">Callback that will deliver a ZDKComment.</param>
		public static void AddComment(string comment, string requestId, string[] attachments, Action<Hashtable,ZDKError> callback) {
			Log("Unity : ZDKRequestProvider:AddCommentWithAttachments");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderCreateRequest(string gameObjectName, string callbackId, string subject, string description, string[] tags, int tagsLength);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderCreateRequestWithAttachments(string gameObjectName, string callbackId, string subject, string description, string[] tags, int tagsLength, string[] attachments, int attachmentsLength);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderGetAllRequests(string gameObjectName, string callbackId);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderGetAllRequestsByStatus(string gameObjectName, string callbackId, string status);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderGetCommentsWithRequestId(string gameObjectName, string callbackId, string requestId);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderAddComment(string gameObjectName, string callbackId, string comment, string requestId);
		[DllImport("__Internal")]
		private static extern void _zendeskRequestProviderAddCommentWithAttachments(string gameObjectName, string callbackId, string comment, string requestId, string[] attachments, int attachmentsLength);

		public static void CreateRequest(string email, string subject, string description, string[] tags, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderCreateRequest(ZendeskSDK.ZenExternal.GameObjectName, callbackId, subject, description, tags, tags.Length);
		}

		public static void CreateRequest(string email, string subject, string description, string[] tags, string[] attachments, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderCreateRequestWithAttachments(ZendeskSDK.ZenExternal.GameObjectName, callbackId, subject, description, tags, tags.Length, attachments, attachments.Length);
		}

		public static void GetAllRequests(Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderGetAllRequests(ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}

		public static void GetAllRequests(string status, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderGetAllRequestsByStatus(ZendeskSDK.ZenExternal.GameObjectName, callbackId, status);
		}

		public static void GetComments(string requestId, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderGetCommentsWithRequestId(ZendeskSDK.ZenExternal.GameObjectName, callbackId, requestId);
		}

		public static void AddComment(string comment, string requestId, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderAddComment(ZendeskSDK.ZenExternal.GameObjectName, callbackId, comment, requestId);
		}

		public static void AddComment(string comment, string requestId, string[] attachments, Action<Hashtable,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskRequestProviderAddCommentWithAttachments(ZendeskSDK.ZenExternal.GameObjectName, callbackId, comment, requestId, attachments, attachments.Length);
		}

		#elif UNITY_ANDROID

		public static void CreateRequest(string email, string subject, string description, string[] tags, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderCreateRequest", ZendeskSDK.ZenExternal.GameObjectName, callbackId, email, subject, description, tags, tags.Length);
		}
		
		public static void CreateRequest(string email, string subject, string description, string[] tags, string[] attachments, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderCreateRequestWithAttachments",ZendeskSDK.ZenExternal.GameObjectName, callbackId, email, subject, description, tags, tags.Length, attachments, attachments.Length);
		}
		
		public static void GetAllRequests(Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderGetAllRequests",ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}
		
		public static void GetAllRequests(string status, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderGetAllRequestsByStatus",ZendeskSDK.ZenExternal.GameObjectName, callbackId, status);
		}

		public static void GetComments(string requestId, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderGetCommentsWithRequestId",ZendeskSDK.ZenExternal.GameObjectName, callbackId, requestId);
		}
		
		public static void AddComment(string comment, string requestId, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderAddComment",ZendeskSDK.ZenExternal.GameObjectName, callbackId, comment, requestId);
		}
		
		public static void AddComment(string comment, string requestId, string[] attachments, Action<Hashtable,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskRequestProviderAddCommentWithAttachments",ZendeskSDK.ZenExternal.GameObjectName, callbackId, comment, requestId, attachments, attachments.Length);
		}

		#endif
	}
}		