using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZendeskSDK {

	/// <summary>
	/// Attach to Unity GameObject to handle callbacks from Zendesk SDK
	/// provider interactions.
	/// </summary>
	public class ZenExternal: MonoBehaviour {
		public static string GameObjectName;
		public static Hashtable ActionCallbacks = new Hashtable();

		private static string _logTag = "ZenExternal";
		
		public static void Log (string message) {
			if(Debug.isDebugBuild)
				Debug.Log(_logTag + "/" + message);
		}

		// Result parsers

		byte[] parseByteArray(Hashtable resultsDict) {
			byte[] result = null;
			if (resultsDict["result"] != null) {
				result = System.Convert.FromBase64String((string)resultsDict["result"]);
			}
			return result;
		}

		ZDKError parseZDKError(Hashtable resultsDict) {
			ZDKError error = null;
			if (resultsDict ["error"] != null) {
				error = new ZDKError ((Hashtable)ZenJSON.Deserialize ((string)resultsDict ["error"]));
			}
			return error;
		}

		Hashtable parseHashtable(Hashtable resultsDict) {
			Hashtable result = null;
			if (resultsDict["result"] != null) {
				result = (Hashtable)ZenJSON.Deserialize((string)resultsDict["result"]);
			}
			return result;
		}

		ArrayList parseArrayList(Hashtable resultsDict) {
			ArrayList result = null;
			if (resultsDict["result"] != null) {
				result = (ArrayList)ZenJSON.Deserialize((string)resultsDict["result"]);
			}
			return result;
		}

		// Game Message Callbacks

		void didAvatarProviderGetAvatar(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<byte[],ZDKError> callback = (Action<byte[],ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseByteArray(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didAvatarProviderGetAvatar - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderGetCategories(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderGetCategories - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderGetSectionsForCategoryId(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderGetSectionsForCategoryId - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderGetArticlesForSectionId(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderGetArticlesForSectionId - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderSearchForArticlesUsingQuery(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderSearchForArticlesUsingQuery - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderSearchForArticlesUsingQueryAndLabels(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderSearchForArticlesUsingQueryAndLabels - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderSearchForArticlesUsingHelpCenterSearch(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderSearchForArticlesUsingHelpCenterSearch - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterProviderGetAttachmentForArticleId(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterProviderGetAttachmentForArticleId - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didHelpCenterGetArticlesByLabels(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didHelpCenterGetArticlesByLabels - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderCreateRequest(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderCreateRequest - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderCreateRequestWithAttachments(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderCreateRequestWithAttachments - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderGetAllRequests(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderGetAllRequests - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderGetAllRequestsByStatus(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderGetAllRequestsByStatus - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderGetCommentsWithRequestId(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<ArrayList,ZDKError> callback = (Action<ArrayList,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseArrayList(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderGetCommentsWithRequestId - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderAddComment(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderAddComment - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didRequestProviderAddCommentWithAttachments(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didRequestProviderAddCommentWithAttachments - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didSettingsProviderGetSettings(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didSettingsProviderGetSettings - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didSettingsProviderGetSettingsWithLocale(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR: didSettingsProviderGetSettingsWithLocale - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didUploadProviderUploadAttachment(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR:  - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		void didUploadProviderDeleteUpload(string results) {
			Hashtable resultsDict = (Hashtable)ZenJSON.Deserialize(results);
			if (ActionCallbacks.ContainsKey(resultsDict["callbackId"])) {
				Action<Hashtable,ZDKError> callback = (Action<Hashtable,ZDKError>) ActionCallbacks[resultsDict["callbackId"]];
				ActionCallbacks.Remove(resultsDict["callbackId"]);
				callback (parseHashtable(resultsDict), parseZDKError(resultsDict));
			}
			else {
				Debug.Log("ERROR:  - Missing callbackId for action in results.  Key = " + resultsDict["callbackId"]);
			}
		}

		//////////////////////////////////////////////////////
		/// Monobehaviour Lifecycle functionality
		//////////////////////////////////////////////////////
		void Awake() {
			GameObjectName = gameObject.name;
			DontDestroyOnLoad(gameObject);
		}
	}
}

