using System;
using UnityEngine;
using System.Collections;
using ZendeskSDK;

public class ZendeskTester: MonoBehaviour
{
	public GameObject inPlayIcon;
	public GameObject inPlayText;
	private Texture2D avatarTexture;

	void Awake() {
		avatarTexture = new Texture2D(60, 60);
	}

	void OnEnable() {
	
	}

	void Update() {

	}

	void OnGUI() {
		GUI.matrix = Matrix4x4.Scale (new Vector3 (5, 5, 5));

		GUI.DrawTexture(new Rect(0.0f,0.0f,60.0f,60.0f), avatarTexture, ScaleMode.ScaleToFit, true);

		if (GUILayout.Button ("Initialize SDK")) {
			ZendeskSDK.ZDKConfig.Instance.InitializeWithAppId ("e5dd7520b178e21212f5cc2751a28f4b5a7dc76698dc79bd",
			                                                  "https://rememberthedate.zendesk.com",
			                                                  "client_for_rtd_jwt_endpoint");
			//ZendeskSDK.ZDKConfig.Instance.AuthenticateAnonymousIdentity("","","");
			ZendeskSDK.ZDKConfig.Instance.AuthenticateJwtUserIdentity ("MyTestID");
		}

		if (GUILayout.Button ("Show Help Center")) {
			ZendeskSDK.ZDKHelpCenter.ShowHelpCenter ();
			//ZendeskSDK.ZDKHelpCenter.ShowHelpCenterFilterByArticleLabels(new string[]{"test", "test2"});
		}

		if (GUILayout.Button ("Show Request Creation")) {
			ZendeskSDK.ZDKRequests.ShowRequestCreation ();
		}

		if (GUILayout.Button ("Show Request Creation Config")) {
			string[] tags = new string[2];
			tags[0] = "Additional Config Tag 0";
			tags[1] = "Additional Config Tag 1";
			ZDKRequestCreationConfig config = new ZDKRequestCreationConfig();
			config.Tags = tags;
			config.AdditionalRequestInfo= "AdditionalRequestInfo TEST";
			ZendeskSDK.ZDKRequests.ShowRequestCreation (config);
		}

		if (GUILayout.Button ("Show Requests List")) {
			ZendeskSDK.ZDKRequests.ShowRequestList ();
		}

		if (GUILayout.Button ("Show Rate My App")) {
			ZendeskSDK.ZDKRMA.ShowAlways ();
		}

		if (GUILayout.Button ("Show Rate My App Config")) {
			string[] additionalTags = new string[2];
			additionalTags[0] = "Additional Config Tag 0";
			additionalTags[1] = "Additional Config Tag 1";
			ZDKRMAAction[] dialogActions = new ZDKRMAAction[3];
			dialogActions[0] = ZDKRMAAction.ZDKRMARateApp;
			dialogActions[1] = ZDKRMAAction.ZDKRMASendFeedback;
			dialogActions[2] = ZDKRMAAction.ZDKRMADontAskAgain;

			ZDKRMAConfigObject config = new ZDKRMAConfigObject();
			config.AdditionalTags = additionalTags;
			config.AdditionalRequestInfo = "AdditionalRequestInfo TEST";
			config.DialogActions = dialogActions;
			config.SuccessImageName = null;
			config.ErrorImageName = null;
			ZendeskSDK.ZDKRMA.Show (config);
		}

		if (GUILayout.Button ("Run Provider Tests")) {
			RunProviderTests ();
		}

		if (GUILayout.Button ("Run Appearance Tests")) {
			RunAppearanceTests ();
		}

		if (GUILayout.Button ("Run SDK Tests")) {
			ZendeskSDK.ZDKLogger.Enable(true);
			ZendeskSDK.ZDKDispatcher.SetDebugLoggingiOS(true);

			// ZDKDeviceInfo Tests
			Debug.Log (string.Format ("Device Type: {0}", ZendeskSDK.ZDKDeviceInfo.DeviceTypeiOS ()));
			Debug.Log (string.Format ("Total Device Memory: {0}", ZendeskSDK.ZDKDeviceInfo.TotalDeviceMemoryiOS ()));
			Debug.Log (string.Format ("Free Disk Space: {0}", ZendeskSDK.ZDKDeviceInfo.FreeDiskspaceiOS ()));
			Debug.Log (string.Format ("Total Disk Space: {0}", ZendeskSDK.ZDKDeviceInfo.TotalDiskspaceiOS ()));
			Debug.Log (string.Format ("Battery Level: {0}", ZendeskSDK.ZDKDeviceInfo.BatteryLeveliOS ()));
			Debug.Log (string.Format ("Region: {0}", ZendeskSDK.ZDKDeviceInfo.RegioniOS ()));
			Debug.Log (string.Format ("Language: {0}", ZendeskSDK.ZDKDeviceInfo.LanguageiOS ()));
			Debug.Log (string.Format ("Device Info String: {0}", ZendeskSDK.ZDKDeviceInfo.DeviceInfoString ()));
			Hashtable deviceInfoDict = ZendeskSDK.ZDKDeviceInfo.DeviceInfoDictionary ();
			Debug.Log (string.Format ("Device Info Dictionary:"));
			foreach (string key in deviceInfoDict.Keys) {
				Debug.Log (string.Format ("{0}: {1}", key, deviceInfoDict [key]));
			}

			// ZDKStringUtil Tests
			string[] strings = new string[2];
			strings [0] = "one";
			strings [1] = "second";
			Debug.Log (string.Format ("CSVStringFromArray: {0}", ZendeskSDK.ZDKStringUtil.CsvStringFromArray (strings)));

			// ZDKLogger Tests
			ZendeskSDK.ZDKLogger.Enable (true);
		}
	}

	void RunAppearanceTests() {
		ZenColor testColor1 = new ZenColor(0.9f, 1.0f); // Random Gray color

		ZendeskSDK.ZDKRequestListLoadingTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListLoadingTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRequestListEmptyTableCell.SetMessageFont("system", 14);
		ZendeskSDK.ZDKRequestListEmptyTableCell.SetMessageColor(testColor1);
		ZendeskSDK.ZDKRequestListEmptyTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListEmptyTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKUILoadingView.SetBackgroundColor(testColor1);

		ZendeskSDK.ZDKUIImageScrollView.SetBackgroundColor(testColor1);

		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonFont("system", 14);
		ZendeskSDK.ZDKSupportView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKSupportView.SetTableBackgroundColor(testColor1);
		ZendeskSDK.ZDKSupportView.SetSeparatorColor(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsFoundLabelColor(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsFoundLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonBackground(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonBorderColor(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonTitleColorNormal(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonTitleColorHighlighted(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonTitleColorDisabled(testColor1);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonEdgeInsets(0, 0, 0, 0);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonBorderWidth(5);
		ZendeskSDK.ZDKSupportView.SetNoResultsContactButtonCornerRadius(15);
		ZendeskSDK.ZDKSupportView.SetAutomaticallyHideNavBarOnLandscape(1);
		ZendeskSDK.ZDKSupportView.SetSearchBarStyle(UIBARSTYLE.UIBARSTYLEDEFAULT);
		ZendeskSDK.ZDKSupportView.SetSpinnerUIActivityIndicatorViewStyle(UIACTIVITYINDICATORVIEWSTYLE.UIACTIVITYINDICATORVIEWSTYLEGRAY);
		ZendeskSDK.ZDKSupportView.SetSpinnerColor(testColor1);

		ZendeskSDK.ZDKSupportTableViewCell.SetTitleLabelFont("system", 14);
		ZendeskSDK.ZDKSupportTableViewCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKSupportTableViewCell.SetTitleLabelColor(testColor1);
		ZendeskSDK.ZDKSupportTableViewCell.SetTitleLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportTableViewCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKSupportAttachmentCell.SetTitleLabelFont("system", 14);
		ZendeskSDK.ZDKSupportAttachmentCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKSupportAttachmentCell.SetFileSizeLabelColor(testColor1);
		ZendeskSDK.ZDKSupportAttachmentCell.SetFileSizeLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportAttachmentCell.SetTitleLabelColor(testColor1);
		ZendeskSDK.ZDKSupportAttachmentCell.SetTitleLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportAttachmentCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKSupportArticleTableViewCell.SetArticleParentsLabelFont("system", 14);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetTitleLabelFont("system", 14);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetArticleParentsLabelColor(testColor1);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetArticleParentsLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetTitleLabelColor(testColor1);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetTitleLabelBackground(testColor1);
		ZendeskSDK.ZDKSupportArticleTableViewCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRMAFeedbackView.SetSubheaderFont("system", 14);
		ZendeskSDK.ZDKRMAFeedbackView.SetHeaderFont("system", 14);
		ZendeskSDK.ZDKRMAFeedbackView.SetTextEntryFont("system", 14);
		ZendeskSDK.ZDKRMAFeedbackView.SetButtonFont("system", 14);
		ZendeskSDK.ZDKRMAFeedbackView.SetButtonColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetButtonSelectedColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetButtonBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetSeparatorLineColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetHeaderColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetSubHeaderColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetTextEntryColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetTextEntryBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetPlaceHolderColor(testColor1);
		ZendeskSDK.ZDKRMAFeedbackView.SetSpinnerUIActivityIndicatorViewStyle (UIACTIVITYINDICATORVIEWSTYLE.UIACTIVITYINDICATORVIEWSTYLEGRAY);
		ZendeskSDK.ZDKRMAFeedbackView.SetSpinnerColor(testColor1);

		ZendeskSDK.ZDKRMADialogView.SetHeaderFont("system", 14);
		ZendeskSDK.ZDKRMADialogView.SetButtonFont("system", 14);
		ZendeskSDK.ZDKRMADialogView.SetHeaderBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetHeaderColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetSeparatorLineColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetButtonBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetButtonSelectedBackgroundColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetButtonColor(testColor1);
		ZendeskSDK.ZDKRMADialogView.SetBackgroundColor(testColor1);

		ZendeskSDK.ZDKRequestCommentTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestCommentTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRequestListTableCell.SetDescriptionFont("system", 14);
		ZendeskSDK.ZDKRequestListTableCell.SetCreatedAtFont("system", 14);
		ZendeskSDK.ZDKRequestListTableCell.SetUnreadColor(testColor1);
		ZendeskSDK.ZDKRequestListTableCell.SetDescriptionColor(testColor1);
		ZendeskSDK.ZDKRequestListTableCell.SetCreatedAtColor(testColor1);
		ZendeskSDK.ZDKRequestListTableCell.SetCellBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTableCell.SetVerticalMargin(0);
		ZendeskSDK.ZDKRequestListTableCell.SetDescriptionTimestampMargin(0);
		ZendeskSDK.ZDKRequestListTableCell.SetLeftInset(0);
		ZendeskSDK.ZDKRequestListTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRequestListTable.SetCellSeparatorColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetTableBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetSectionIndexColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetSectionIndexBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetSectionIndexTrackingBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetSeparatorColor(testColor1);
		ZendeskSDK.ZDKRequestListTable.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRequestCommentAttachmentTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestCommentAttachmentTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKRequestCommentAttachmentLoadingTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKRequestCommentAttachmentLoadingTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKEndUserCommentTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetBodyFont("system", 14);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetTimestampFont("system", 14);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetBodyColor(testColor1);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetTimestampColor(testColor1);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetCellBackground(testColor1);
		ZendeskSDK.ZDKEndUserCommentTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKCreateRequestView.SetTextEntryFont("system", 14);
		ZendeskSDK.ZDKCreateRequestView.SetPlaceholderTextColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetTextEntryColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetTextEntryBackgroundColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentButtonBorderColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentButtonBackground(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentButtonCornerRadius(0);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentButtonBorderWidth(0);
		ZendeskSDK.ZDKCreateRequestView.SetAutomaticallyHideNavBarOnLandscape(1);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentActionSheetStyle (UIACTIONSHEETSTYLE.UIACTIONSHEETSTYLEDEFAULT);
		ZendeskSDK.ZDKCreateRequestView.SetSpinnerUIActivityIndicatorViewStyle(UIACTIVITYINDICATORVIEWSTYLE.UIACTIVITYINDICATORVIEWSTYLEGRAY); 
		ZendeskSDK.ZDKCreateRequestView.SetSpinnerColor(testColor1);
		ZendeskSDK.ZDKCreateRequestView.SetAttachmentButtonImage ("testimagename", "png");

		ZendeskSDK.ZDKCommentsListLoadingTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentsListLoadingTableCell.SetLeftInset(0);
		ZendeskSDK.ZDKCommentsListLoadingTableCell.SetCellBackground(testColor1);
		ZendeskSDK.ZDKCommentsListLoadingTableCell.SetSeparatorInset(0, 0, 0, 0);

		ZendeskSDK.ZDKCommentInputView.SetAttachmentButtonBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetTopBorderColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetTextEntryColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetTextEntryBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetTextEntryBorderColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetSendButtonColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetAreaBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentInputView.SetTextEntryFont("system", 14);
		ZendeskSDK.ZDKCommentInputView.SetSendButtonFont("system", 14);

		ZendeskSDK.ZDKCommentEntryView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetTopBorderColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetTextEntryColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetTextEntryBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetTextEntryBorderColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetSendButtonColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetAreaBackgroundColor(testColor1);
		ZendeskSDK.ZDKCommentEntryView.SetTextEntryFont("system", 14);
		ZendeskSDK.ZDKCommentEntryView.SetSendButtonFont("system", 14);

		ZendeskSDK.ZDKAttachmentView.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKAttachmentView.SetCloseButtonBackgroundColor(testColor1);

		ZendeskSDK.ZDKAgentCommentTableCell.SetBackgroundColor(testColor1);
		ZendeskSDK.ZDKAgentCommentTableCell.SetAvatarSize (40);
		ZendeskSDK.ZDKAgentCommentTableCell.SetAgentNameFont("system", 14);
		ZendeskSDK.ZDKAgentCommentTableCell.SetBodyFont("system", 14);
		ZendeskSDK.ZDKAgentCommentTableCell.SetTimestampFont("system", 14);
		ZendeskSDK.ZDKAgentCommentTableCell.SetAgentNameColor(testColor1);
		ZendeskSDK.ZDKAgentCommentTableCell.SetBodyColor(testColor1);
		ZendeskSDK.ZDKAgentCommentTableCell.SetTimestampColor(testColor1);
		ZendeskSDK.ZDKAgentCommentTableCell.SetCellBackground(testColor1);
		ZendeskSDK.ZDKAgentCommentTableCell.SetSeparatorInset(0, 0, 0, 0);
	}

	void RunProviderTests() {

		// ZDKRequestProvider Tests

		string[] tags = new string[0];
		string[] attachments = new string[0];
		ZendeskSDK.ZDKRequestProvider.CreateRequest("test@zendesk.com", "Test Subject", "Test Description", tags, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.CreateRequest - " + error.Description);
			} 
			else {
				Debug.Log("CreateRequest Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.CreateRequest("test@zendesk.com", "Test Subject", "Test Description", tags, attachments, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.CreateRequest - " + error.Description);
			} 
			else {
				Debug.Log("CreateRequest Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.GetAllRequests((result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.GetAllRequests - " + error.Description);
			} 
			else {
				Debug.Log("GetAllRequests Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.GetAllRequests("Test Status", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.GetAllRequests - " + error.Description);
			} 
			else {
				Debug.Log("GetAllRequests Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.GetComments("Test RequestID", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.GetComments - " + error.Description);
			} 
			else {
				Debug.Log("GetComments Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.AddComment("Test Comment", "Test RequestID", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.AddComment - " + error.Description);
			} 
			else {
				Debug.Log("AddComment Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKRequestProvider.AddComment("Test Comment", "Test RequestID", attachments, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKRequestProvider.AddComment - " + error.Description);
			} 
			else {
				Debug.Log("AddComment Successful Callback - " + MakeResultString(result));
			}
		});

		// ZDKHelpCenterProvider Tests

		string[] labels = new string[0];
		ZendeskSDK.ZDKHelpCenterProvider.GetCategories ((result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.GetCategories - " + error.Description);
			} 
			else {
				Debug.Log("GetCategories Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.GetSections("Test categoryId", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.GetSections - " + error.Description);
			} 
			else {
				Debug.Log("GetSections Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.GetArticles("Test sectionId", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.SearchForArticles - " + error.Description);
			} 
			else {
				Debug.Log("SearchForArticles Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.SearchForArticles("Test query", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.SearchForArticles - " + error.Description);
			} 
			else {
				Debug.Log("SearchForArticles Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.SearchForArticles("Test query", labels, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.SearchForArticles - " + error.Description);
			} 
			else {
				Debug.Log("SearchForArticles Successful Callback - " + MakeResultString(result));
			}
		});

		ZDKHelpCenterSearch helpCenterSearch = new ZDKHelpCenterSearch();
		helpCenterSearch.SectionId = 200550945;
		ZendeskSDK.ZDKHelpCenterProvider.SearchArticles(helpCenterSearch, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.SearchArticles - " + error.Description);
			} 
			else {
				Debug.Log("SearchArticles Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.GetAttachment("Test articleId", "txt", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.GetAttachment - " + error.Description);
			} 
			else {
				Debug.Log("GetAttachment Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKHelpCenterProvider.GetArticles(labels, (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKHelpCenterProvider.GetArticles - " + error.Description);
			} 
			else {
				Debug.Log("GetArticles Successful Callback - " + MakeResultString(result));
			}
		});

		// ZDKAvatarProvider Tests

		ZendeskSDK.ZDKAvatarProvider.GetAvatar(
			"https://rememberthedate.zendesk.com/system/photos/5774/2635/Screen_Shot_2014-10-21_at_01.21.51.png", 
			(result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKAvatarProvider.GetAvatar - " + error.Description);
			} 
			else {
				Debug.Log("GetAvatar Successful Callback");
				avatarTexture.LoadImage(result);
			}
		});

		// ZDKSettingsProvider Tests

		ZendeskSDK.ZDKSettingsProvider.GetSdkSettings((result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKSettingsProvider.GetSdkSettings - " + error.Description);
			} 
			else {
				Debug.Log("GetSdkSettings Successful Callback - " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKSettingsProvider.GetSdkSettingsiOS("en", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKSettingsProvider.GetSdkSettingsiOS - " + error.Description);
			} 
			else {
				Debug.Log("GetSdkSettingsiOS Successful Callback - " + MakeResultString(result));
			}
		});

		// ZDKUploadProvider Tests

		ZendeskSDK.ZDKUploadProvider.UploadAttachment("TestAttachment", "TestAttachment.txt", "text", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKUploadProvider.UploadAttachment - " + error.Description);
			}
			else {
				Debug.Log("UploadAttachmentCallbackRan: " + MakeResultString(result));
			}
		});

		ZendeskSDK.ZDKUploadProvider.DeleteUpload("TestUploadToken", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKUploadProvider.DeleteUpload - " + error.Description);
			}
			else {
				Debug.Log("DeleteUploadCallbackRan: " + MakeResultString(result));
			}
		});

		runCreateRequestWithAttachmentUploadTest();
		runUploadImageAndAttachToCreateRequestTest ();
	}

	void runUploadImageAndAttachToCreateRequestTest() {
		string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
	}
		
	void runCreateRequestWithAttachmentUploadTest() {
		ZendeskSDK.ZDKUploadProvider.UploadAttachment("TestAttachment", "TestAttachment.txt", "text", (result, error) => {
			if (error != null) {
				Debug.Log("ERROR: ZDKUploadProvider.UploadAttachment - " + error.Description);
			}
			else {
				Debug.Log("UploadAttachmentCallbackRan: " + MakeResultString(result));
				
				string[] tags = new string[0];
				string[] attachments = new string[1];
				#if UNITY_ANDROID
				attachments[0] = (string) result["token"];
				#else
				attachments[0] = (string) result["uploadToken"];
				#endif

				ZendeskSDK.ZDKRequestProvider.CreateRequest("test@zendesk.com", 
				                                            "Test Attachments1", 
				                                            "Test Description1", 
				                                            tags, 
				                                            attachments, 
				                                            (result2, error2) => {
					if (error2 != null) {
						Debug.Log("ERROR: ZDKRequestProvider.CreateRequest - " + error2.Description);
					} 
					else {
						Debug.Log("CreateRequest Successful Callback - " + MakeResultString(result2));
					}
				});
			}
		});
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			avatarTexture = www.texture;
			byte[] imageData = www.texture.EncodeToPNG();
			string stringifiedImageData = Convert.ToBase64String(imageData);
			
			ZendeskSDK.ZDKUploadProvider.UploadAttachment(stringifiedImageData, "TestAttachment.png", "image/png", (result, error) => {
				if (error != null) {
					Debug.Log("ERROR: ZDKUploadProvider.UploadAttachment - " + error.Description);
				}
				else {
					Debug.Log("UploadAttachmentCallbackRan: " + MakeResultString(result));
					
					string[] tags = new string[0];
					string[] attachments = new string[1];

					#if UNITY_ANDROID
					attachments[0] = (string) result["token"];
					#else
					attachments[0] = (string) result["uploadToken"];
					#endif
					
					ZendeskSDK.ZDKRequestProvider.CreateRequest("tester@zendesk.com", 
					                                            "Test Attachments2", 
					                                            "Test Description2", 
					                                            tags, 
					                                            attachments, 
					                                            (result2, error2) => {
						if (error2 != null) {
							Debug.Log("ERROR: ZDKRequestProvider.CreateRequest - " + error2.Description);
						} 
						else {
							Debug.Log("CreateRequest Successful Callback - " + MakeResultString(result2));
						}
					});
				}
			});
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	string MakeResultString(object obj) {
		if (obj == null) {
			return "null";
		}

		if (obj.GetType().Name == "Hashtable") {
			Hashtable dict = (Hashtable) obj;
			string result = "{";
			foreach(string key in dict.Keys) {
				result += String.Format("{0}: {1}, ", key, MakeResultString(dict[key]));
			}
			result += "}";
			return result;
		}
		else if (obj.GetType().Name == "ArrayList") {
			ArrayList list = (ArrayList) obj;
			string result = "[";
			foreach(object obj2 in list) {
				result += String.Format("{0}, ", MakeResultString(obj2));
			}
			result += "]";
			return result;
		}
		else {
			return String.Format("{0}", obj);
		}
	}

	void OnDisable() {
	}
}


