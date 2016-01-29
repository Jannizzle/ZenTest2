using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// ZDKConfig is responsible for initialization of the SDK and manages the backend configuration.
	/// </summary>
	public sealed class ZDKConfig {
		private ZDKConfig(){}

		/// <summary>
		/// Get the API instance (singleton).
		/// </summary>
		public static ZDKConfig Instance { get { return Nested.instance; } }
		
		private class Nested {
			static Nested(){}
			internal static readonly ZDKConfig instance = new ZDKConfig();
		}
		
		private static string _logTag = "ZDKConfig";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Configure the User Identity with an anonymous user.
		/// </summary>
		/// <param name="name">Username</param>
		/// <param name="email">Email Address</param>
		/// <param name="externalId">Identifier</param>
		public void AuthenticateAnonymousIdentity(string name, string email, string externalId) {
			Log("Unity : InitializeWithAppId");
		}

		/// <summary>
		/// Configure the User Identity with a Jwt Authenticated user.
		/// </summary>
		/// <param name="jwtUserIdentity">JWT Identifier</param>
		public void AuthenticateJwtUserIdentity(string jwtUserIdentity) {
			Log("Unity : InitializeWithAppId");
		}

		/// <summary>
		/// Initialize the SDK.
		/// </summary>
		/// <param name="applicationId">The application id of your SDK app, as found in the web interface</param>
		/// <param name="zendeskUrl">The full URL of your Zendesk instance, https://{subdomain}.zendesk.com</param>
		/// <param name="oAuthClientId">The oAuthClientId required as part of the authentication process</param>
		public void InitializeWithAppId(string applicationId, string zendeskUrl, string oAuthClientId) {
			Log("Unity : InitializeWithAppId");
		}
		
		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskConfigAuthenticateAnonymousIdentity(string name, string email, string externalId);
		[DllImport("__Internal")]
		private static extern void _zendeskConfigAuthenticateJwtUserIdentity(string jwtUserIdentity);
		[DllImport("__Internal")]
		private static extern void _zendeskConfigInitialize(string applicationId, string zendeskUrl, string oAuthClientId);

		public void AuthenticateAnonymousIdentity(string name, string email, string externalId) {
			_zendeskConfigAuthenticateAnonymousIdentity(name, email, externalId);
		}
		
		public void AuthenticateJwtUserIdentity(string jwtUserIdentity) {
			_zendeskConfigAuthenticateJwtUserIdentity(jwtUserIdentity);
		}

		public void InitializeWithAppId(string applicationId, string zendeskUrl, string oAuthClientId) {
			_zendeskConfigInitialize(applicationId, zendeskUrl, oAuthClientId);
		}

		#elif UNITY_ANDROID
		private AndroidJavaObject _plugin;
		private bool initialized = false;
		
		private bool checkInitialized() {
			if (initialized) {
				return true;
			} 
			else {
				Debug.LogError("The Zendesk SDK needs to be initialized");
				return false;
			}
		}
		
		public void AuthenticateAnonymousIdentity(string name, string email, string externalId) {
			if (!checkInitialized())
				return;
			_plugin.Call("authenticateAnonymousIdentity",name, email, externalId);
		}
		
		public void AuthenticateJwtUserIdentity(string jwtUserIdentity){
			if (!checkInitialized())
				return;
			_plugin.Call("authenticateJwtUserIdentity", jwtUserIdentity);
		}

		public void InitializeWithAppId(string applicationId, string zendeskUrl, string oAuthClientId){
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin")){
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("initializeWithAppId", applicationId, zendeskUrl, oAuthClientId);
			initialized = true;
		}

		#endif

		#if UNITY_EDITOR || !UNITY_IPHONE

		/// <summary>
		/// Reload the config from the server, reload will be started if a reload
		/// is not already in progress and the reload interval has passed. This method
		/// will automatically be invoked when the application enters the foreground to
		/// check for updates if due.
		/// iOS Only.
		/// </summary>
		public void ReloadiOS() {
			Log("Unity : ReloadiOS");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskConfigReload();

		public void ReloadiOS() {
			_zendeskConfigReload();
		}
		#endif
	}
}
