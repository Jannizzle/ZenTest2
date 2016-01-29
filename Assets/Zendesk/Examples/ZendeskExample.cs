using System;
using UnityEngine;
using System.Collections;
using ZendeskSDK;

// <summary>
// A simple example demonstrating Zendek configuration and displaying the
// Help Center, Requests and Rate My App views.
// </summary>
public class ZendeskExample: MonoBehaviour
{

	public GameObject inPlayIcon;
	public GameObject inPlayText;
	private Texture2D avatarTexture;

	// <summary>
	// Replace configuration options with your app's settings.
	// </summary>
	/// <param name="ApplicationId">The application id of your SDK app, as found in the web interface</param>
	/// <param name="ZendeskUrl">The full URL of your Zendesk instance, https://{subdomain}.zendesk.com</param>
	/// <param name="oAuthClientID">The oAuthClientId required as part of the authentication process</param>

	void Awake() {
		
		ZendeskSDK.ZDKConfig.Instance.InitializeWithAppId ("4f228c53d5cd8fe1612f4aff523d3a321dfefe015b13ee27",
		                                                   "https://fdgentertainment.zendesk.com",
		                                                   "mobile_sdk_client_d807b817b0feae2fe2c2");
		ZendeskSDK.ZDKConfig.Instance.AuthenticateAnonymousIdentity("HansWurscht", "hw@aol.com", "<EXTERNAL_ID>");
	}



	void OnEnable() {
	
	}

	void Update() {

	}

	void OnGUI() {
		GUI.matrix = Matrix4x4.Scale (new Vector3 (5, 5, 5));

		if (GUILayout.Button ("Help Center")) {
			ZendeskSDK.ZDKHelpCenter.ShowHelpCenter ();
		}

		if (GUILayout.Button ("Request Creation")) {
			ZendeskSDK.ZDKRequests.ShowRequestCreation ();
		}

		if (GUILayout.Button ("Requests List")) {
			ZendeskSDK.ZDKRequests.ShowRequestList ();
		}

		if (GUILayout.Button ("Rate My App")) {
			ZendeskSDK.ZDKRMA.ShowAlways ();
		}
	}

	void OnDisable() {
	}
}


