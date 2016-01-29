using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	public class ZDKHelpCenterProvider {
		
		private static string _logTag = "ZDKHelpCenterProvider";
		
		public static void Log(string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IPHONE)

		/// <summary>
		/// Fetch a list of categories from a Help Center instance.
		/// </summary>
		/// <param name="callback">Callback that will deliver a list of categories available</param>
		public static void GetCategories(Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:GetCategories");
		}

		/// <summary>
		/// Fetch a list of sections for a given categoryId from a Help Center instance
		/// </summary>
		/// <param name="categoryId">String to specify what sections should be returned, 
		/// only sections belonging to the category will be returned</param>
		/// <param name="callback">Callback that will deliver a list of sections available</param>
		public static void GetSections(string categoryId, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:GetSections");
		}

		/// <summary>
		/// Fetch a list of articles for a given sectionId from a Help Center instance
		/// </summary>
		/// <param name="sectionId">String to specify what articles should be returned, 
		/// only articles belonging to the section will be returned</param>
		/// <param name="callback">Callback that will deliver a list of articles available</param>
		public static void GetArticles(string sectionId, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:GetArticles");
		}

		/// <summary>
		/// This method will search articles in your Help Center.
		/// This method will also sideload categories, sections and users.
		/// </summary>
		/// <param name="query">The query text used to perform the search</param>
		/// <param name="callback">The callback which will be called upon a successful or an erroneous response.</param>
		public static void SearchForArticles(string query, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:SearchForArticles");
		}

		/// <summary>
		/// This method will search articles in your Help Center filtered by an array of labels
		/// </summary>
		/// <param name="query">The query text used to perform the search</param>
		/// <param name="labels">The array of labels used to filter the search results</param>
		/// <param name="callback">The callback which will be called upon a successful or an erroneous response.</param>
		public static void SearchForArticles(string query, string[] labels, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:SearchForArticlesWithLabels");
		}

		/// <summary>
		/// This method will search articles in your Help Center filtered by the parameters in the given ZDKHelpCenterSearch
		/// </summary>
		/// <param name="helpCenterSearch">The search to perform</param>
		/// <param name="callback">The callback which will be called upon a successful or an erroneous response.</param>
		public static void SearchArticles(ZDKHelpCenterSearch helpCenterSearch, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:SearchArticles");
		}

		/// <summary>
		/// This method returns a list of attachments for a single article.
		/// </summary>
		/// <param name="articleId">the identifier to be used to retrieve an article from a Help Center instance</param>
		/// <param name="attachmentType">The file extension of the attachment to get (only on Android)</param>
		/// <param name="callback">The callback which will be called upon a successful or an erroneous response.</param>
		public static void GetAttachment(string articleId, string attachmentType, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:GetAttachment");
		}

		/// <summary>
		/// Fetch a list of articles for a given array of labels from a Help Center instance
		/// </summary>
		/// <param name="labels">An array of labels used to filter articles by</param>
		/// <param name="callback">The callback which will be called upon a successful or an erroneous response.</param>
		public static void GetArticles(string[] labels, Action<ArrayList,ZDKError> callback) {
			Log("Unity : ZDKHelpCenterProvider:GetArticles");
		}

		#elif UNITY_IPHONE

		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderGetCategoriesWithCallback(string gameObjectName, string callbackId);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderGetSectionsForCategoryId(string gameObjectName, string callbackId, string categoryId);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderGetArticlesForSectionId(string gameObjectName, string callbackId, string sectionId);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderSearchForArticlesUsingQuery(string gameObjectName, string callbackId, string query);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderSearchForArticlesUsingQueryAndLabels(string gameObjectName, 
		                                                                                          string callbackId, 
		                                                                                          string query, 
		                                                                                          string[] labelsArray, 
		                                                                                          int labelsLength);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderSearchForArticlesUsingHelpCenterSearch(string gameObjectName, 
		                                                                                            string callbackId,
		                                                                                            string query,
		                                                                                            string[] labelNames,
		                                                                                            int labelNamesLength,
		                                                                                            string locale,
		                                                                                            string[] sideLoads,
		                                                                                            int sideLoadsLength,
		                                                                                            int categoryId,
		                                                                                            int sectionId,
		                                                                                            int page,
		                                                                                            int resultsPerPage);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderGetAttachmentForArticleId(string gameObjectName, string callbackId, string articleId);
		[DllImport("__Internal")]
		private static extern void _zendeskHelpCenterProviderGetArticlesByLabels(string gameObjectName, string callbackId, string[] labelsArray, int labelsLength);

		public static void GetCategories(Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderGetCategoriesWithCallback(ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}
		
		public static void GetSections(string categoryId, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderGetSectionsForCategoryId(ZendeskSDK.ZenExternal.GameObjectName, callbackId, categoryId);
		}
		
		public static void GetArticles(string sectionId, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderGetArticlesForSectionId(ZendeskSDK.ZenExternal.GameObjectName, callbackId, sectionId);
		}
		
		public static void SearchForArticles(string query, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderSearchForArticlesUsingQuery(ZendeskSDK.ZenExternal.GameObjectName, callbackId, query);
		}
		
		public static void SearchForArticles(string query, string[] labels, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderSearchForArticlesUsingQueryAndLabels(ZendeskSDK.ZenExternal.GameObjectName, callbackId, query, labels, labels.Length);
		}

		public static void SearchArticles(ZDKHelpCenterSearch helpCenterSearch, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderSearchForArticlesUsingHelpCenterSearch(ZendeskSDK.ZenExternal.GameObjectName, 
			                                                                 callbackId, 
			                                                                 helpCenterSearch.Query,
			                                                                 helpCenterSearch.LabelNames,
			                                                                 helpCenterSearch.LabelNames != null ? helpCenterSearch.LabelNames.Length : -1,
			                                                                 helpCenterSearch.Locale,
			                                                                 helpCenterSearch.SideLoads,
			                                                                 helpCenterSearch.SideLoads != null ? helpCenterSearch.SideLoads.Length : -1,
			                                                                 helpCenterSearch.CategoryId,
			                                                                 helpCenterSearch.SectionId,
			                                                                 helpCenterSearch.Page,
			                                                                 helpCenterSearch.ResultsPerPage);
		}
		
		public static void GetAttachment(string articleId, string attachmentType, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderGetAttachmentForArticleId(ZendeskSDK.ZenExternal.GameObjectName, callbackId, articleId);
		}
		
		public static void GetArticles(string[] labels, Action<ArrayList,ZDKError> callback) {
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId, callback);
			_zendeskHelpCenterProviderGetArticlesByLabels(ZendeskSDK.ZenExternal.GameObjectName, callbackId, labels, labels.Length);
		}

		#elif UNITY_ANDROID

		public static void GetCategories(Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call("zendeskHelpCenterProviderGetCategoriesWithCallback",ZendeskSDK.ZenExternal.GameObjectName, callbackId);
		}
		
		public static void GetSections(string categoryId, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderGetSectionsForCategoryId",ZendeskSDK.ZenExternal.GameObjectName, callbackId, categoryId);
		}
		
		public static void GetArticles(string sectionId, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderGetArticlesForSectionId",ZendeskSDK.ZenExternal.GameObjectName, callbackId, sectionId);
		}
		
		public static void SearchForArticles(string query, Action<ArrayList,ZDKError> callback) {
			Log("Android : ZDKHelpCenterProvider:SearchForArticles");

			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderSearchForArticlesUsingQuery",ZendeskSDK.ZenExternal.GameObjectName, callbackId, query);
		}
		
		public static void SearchForArticles(string query, string[] labels, Action<ArrayList,ZDKError> callback) {
			Log("Android : ZDKHelpCenterProvider:SearchForArticlesWithLabels");

			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderSearchForArticlesUsingQueryAndLabels",ZendeskSDK.ZenExternal.GameObjectName, callbackId, query, labels, labels.Length);
		}

		public static void SearchArticles(ZDKHelpCenterSearch helpCenterSearch, Action<ArrayList,ZDKError> callback) {
			Log("Android : ZDKHelpCenterProvider:SearchArticles");

			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderSearchForArticlesUsingHelpCenterSearch", ZendeskSDK.ZenExternal.GameObjectName, callbackId,
			              helpCenterSearch.Query, helpCenterSearch.LabelNames, helpCenterSearch.Locale, helpCenterSearch.SideLoads,
			              helpCenterSearch.CategoryId, helpCenterSearch.SectionId, helpCenterSearch.Page, helpCenterSearch.ResultsPerPage);
		}

		public static void GetAttachment(string articleId, string attachmentType, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderGetAttachmentForArticleId",ZendeskSDK.ZenExternal.GameObjectName, callbackId, articleId, attachmentType);
		}
		
		public static void GetArticles(string[] labels, Action<ArrayList,ZDKError> callback) {
			AndroidJavaObject _plugin;
			string callbackId = Guid.NewGuid().ToString();
			ZendeskSDK.ZenExternal.ActionCallbacks.Add(callbackId,callback);
			using(var pluginClass = new AndroidJavaClass("com.zendesk.unity.ZDK_Plugin"))
			{
				_plugin = pluginClass.CallStatic<AndroidJavaObject>("instance");
			}
			_plugin.Call ("zendeskHelpCenterProviderGetArticleByLabels",ZendeskSDK.ZenExternal.GameObjectName, callbackId, labels, labels.Length);
		}
		
		#endif
	}
}		