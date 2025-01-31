﻿

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Tabtale.TTPlugins;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable InconsistentNaming

public class CLIKConfiguration : EditorWindow
{
    private class Constants
    {
        public const string CONFIG_FILE_PATH = "Assets/StreamingAssets/ttp/configurations/";
        
        public const string CONFIG_FN_ANALYTICS = "analytics";
        public const string CONFIG_FN_APPSFLYER = "appsFlyer";
        public const string CONFIG_FN_BANNERS = "banners";
        public const string CONFIG_FN_CRASHTOOL = "crashMonitoringTool";
        public const string CONFIG_FN_ELEPHANT = "elephant";
        public const string CONFIG_FN_GLOBAL = "global";
        public const string CONFIG_FN_INTERSTITIALS = "interstitials";
        public const string CONFIG_FN_POPUPSMGR = "popupsMgr";
        public const string CONFIG_FN_PRIVACY_SETTINGS = "privacySettings";
        public const string CONFIG_FN_RV = "rewardedAds";
        public const string CONFIG_FN_RV_INTER = "rewardedInterstitials";
        public const string CONFIG_FN_RATEUS = "rateUs";
        public const string CONFIG_FN_OPENADS = "openads";

        public const string CONFIG_KEY_INCLUDED = "included";
        
        public const string CONFIG_KEY_FIREBASE = "firebase";
        public const string CONFIG_KEY_FIREBASE_GOOGLE_ID = "googleAppId";
        public const string CONFIG_KEY_FIREBASE_SENDER_ID = "senderId";
        public const string CONFIG_KEY_FIREBASE_CLIENT_ID = "clientId";
        public const string CONFIG_KEY_FIREBASE_DB_URL = "databaseURL";
        public const string CONFIG_KEY_FIREBASE_STORAGE_BUCKET = "storageBucket";
        public const string CONFIG_KEY_FIREBASE_API_KEY = "apiKey";
        public const string CONFIG_KEY_FIREBASE_PROJECT_ID = "projectId";

        public const string CONFIG_KEY_APPSFLYER = "appsFlyerKey";
        
        public const string CONFIG_KEY_HOCKEYAPP = "hockeyAppKey";
        
        public const string CONFIG_KEY_SERVER_DOMAIN = "serverDomain";
        
        public const string CONFIG_KEY_BANNERS_ALIGN_TO_TOP = "alignToTop";
        public const string CONFIG_KEY_BANNERS_AD_DISPLAY_TIME = "adDisplayTime";
        public const string CONFIG_KEY_BANNERS_ADMOB_KEY = "bannersAdMobKey";
        public const string CONFIG_KEY_BANNERS_HOUSEADS_SERVER_DOMAIN = "houseAdsServerDomain";
        
        public const string CONFIG_KEY_GLOBAL_BUNDLE_ID = "bundleId";
        public const string CONFIG_KEY_GLOBAL_TEST_MODE = "testMode";
        public const string CONFIG_KEY_GLOBAL_STORE = "store";
        public const string CONFIG_KEY_GLOBAL_GAME_ENGINE = "gameEngine";
        public const string CONFIG_KEY_GLOBAL_AUDIENCE_MODE = "audienceModeBuildOnly";
        public const string CONFIG_KEY_GLOBAL_ORIENTATION = "orientation";
        public const string CONFIG_KEY_GLOBAL_APPBUILDCONFIG = "appBuildConfig";
        public const string CONFIG_KEY_GLOBAL_ADMOB = "admob";
        public const string CONFIG_KEY_GLOBAL_IS_ADMOB = "isAdMob";
        public const string CONFIG_KEY_GLOBAL_APPLICATION = "application";
        public const string CONFIG_KEY_GLOBAL_CONVERSION_MODEL_TYPE = "conversionModelType";
        
        public const string CONFIG_KEY_INTERSTITIALS= "interstitialsAdMobKey";
        
        public const string CONFIG_KEY_RV= "rewardedAdsAdMobKey";
        public const string CONFIG_KEY_RV_INTER = "rewardedInterAdMobKey";

        public const string CONFIG_KEY_OPENADS = "appOpenAdMobKey";
        
        public const string CONFIG_KEY_POPUPMGR_SESSION_TIME_TO_FIRST_POPUP = "sessionTimeToFirstPopup";
        public const string CONFIG_KEY_POPUPMGR_GAME_TIME_TO_FIRST_POPUP = "gameTimeToFirstPopup";
        public const string CONFIG_KEY_POPUPMGR_RESET_POPUP_TIMER_ON_RV = "resetPopupTimerOnRV";
        public const string CONFIG_KEY_POPUPMGR_POPUP_INTERVALS_BY_SESSION = "popupsIntervalsBySession";
        public const string CONFIG_KEY_POPUPMGR_GAME_TIME_TO_FIRST_POPUP_BY_SESSION = "gameTimeToFirstPopupBySession";
        public const string CONFIG_KEY_POPUPMGR_SESSION_TIME_TO_FIRST_POPUP_BY_SESSION = "sessionTimeToFirstPopupBySession";
        public const string CONFIG_KEY_POPUPMGR_LEVEL_TO_FIRST_POPUP = "levelToFirstPopup";
        public const string CONFIG_KEY_POPUPMGR_FIRST_POPUP_AT_SESSION = "firstPopupAtSession";
        public const string CONFIG_KEY_POPUPMGR_INTERVALS = "popupsIntervals";
        public const string CONFIG_KEY_POPUPMGR_RESET_RV_BY_SESSION = "resetPopupTimerOnRVBySession";
        
        public const string CONFIG_KEY_PRIVACY_SETTINGS_CONSENT_FORM_VERSION = "consentFormVersion";
        public const string CONFIG_KEY_PRIVACY_SETTINGS_CONSENT_FORM_URL = "consentFormURL";
        public const string CONFIG_KEY_PRIVACY_SETTINGS_URL = "privacySettingsURL";
        public const string CONFIG_KEY_PRIVACY_SETTINGS_USE_TTP_POPUPS = "useTTPGDPRPopups";
        
        public const string CONFIG_KEY_RATE_US_ICON_URL = "iconUrl";
        public const string CONFIG_KEY_RATE_US_COOLDOWN = "coolDown";


        public const string ANDROID_MANIFEST_RES_FILE_PATH = "Assets/Plugins/Android/AndroidConfigurations/res/values/google-services.xml";
        public const string ANDROID_MANIFEST_RES_PATH = "//resources";

        public const string GOOGLE_SERVICES_JSON_PATH = "Assets/StreamingAssets/google-services.json";

        public const string RES_NAME_ADMOB_APP_ID = "admob_app_id";
        public const string RES_NAME_FIREBASE_DB = "firebase_database_url";
        public const string RES_NAME_SENDER_ID = "gcm_defaultSenderId";
        public const string RES_NAME_STORAGE_BUCKET = "google_storage_bucket";
        public const string RES_NAME_PROJECT_ID = "project_id";
        public const string RES_NAME_API_KEY = "google_api_key";
        public const string RES_NAME_APP_ID = "google_app_id";
        public const string RES_NAME_CLIENT_ID = "client_id";

        public const string PLAYER_PREFS_KEY_FIRST_CONFIGURATION = "CLIK-firstTimeConfiguration";


    }
   
    [MenuItem("CLIK/Configure...")]
    private static void Init()
    {

        var firstTimeConfiguration = PlayerPrefs.GetInt(Constants.PLAYER_PREFS_KEY_FIRST_CONFIGURATION, 0);
        if (firstTimeConfiguration == 0)
        {
            if (!EditorUtility.DisplayDialog("CLIK Configuration", "Thanks for installing CLIK! To begin, " +
                                                                  "please choose the zip file that was provided by your publishing manager",
                "Choose ZIP", "Cancel"))
            {
                return;
            }
            if (!UnzipAnLoadConfiguration())
            {
                return;
            }
        }

        // Get existing open window or if none, make a new one:
        var window = GetWindow<CLIKConfiguration>(true, "CLIK Configuration");
        window.Show();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.Space();
        var isAndroid = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
        GUILayout.Label("Mode: " + (_configuration.globalConfig.testMode ? "Test" : "Production"));
        var currentId =
            PlayerSettings.GetApplicationIdentifier(isAndroid ? BuildTargetGroup.Android : BuildTargetGroup.iOS);
        if (_configuration.globalConfig.bundleId != currentId)
        {
            GUILayout.Label("Application Id does not match Configuration Bundle Id. Current Id = " + currentId, _redLabel);
            if (GUILayout.Button("Change Application Id"))
            {
                PlayerSettings.SetApplicationIdentifier(isAndroid ? BuildTargetGroup.Android : BuildTargetGroup.iOS, _configuration.globalConfig.bundleId);
            }
        }
        if ((isAndroid && _configuration.globalConfig.store != "google") ||
            (!isAndroid && _configuration.globalConfig.store != "apple"))
        {
            GUILayout.Label("Configuration does not match current build target!" ,_redLabel);
        }
        if (_configuration.popUpMgrConfig.included)
        {
            GUILayout.Label ("Pop Up Manager", EditorStyles.boldLabel);
            _configuration.popUpMgrConfig.popupsInterval = EditorGUILayout.LongField("Time Between Popups (sec)", _configuration.popUpMgrConfig.popupsInterval);
            _configuration.popUpMgrConfig.gameTimeToFirstPopup = EditorGUILayout.LongField("Game time to first popup (sec)", _configuration.popUpMgrConfig.gameTimeToFirstPopup);
            _configuration.popUpMgrConfig.sessionTimeToFirstPopup = EditorGUILayout.LongField("Session time to first popup (sec)", _configuration.popUpMgrConfig.sessionTimeToFirstPopup);
            _configuration.popUpMgrConfig.firstPopupAtSession = EditorGUILayout.LongField("First popup in session", _configuration.popUpMgrConfig.firstPopupAtSession);
            _configuration.popUpMgrConfig.levelToFirstPopup = EditorGUILayout.LongField("Level to first popup", _configuration.popUpMgrConfig.levelToFirstPopup);
            _configuration.popUpMgrConfig.resetPopupTimerOnRV = EditorGUILayout.Toggle("Reset timer on RV", _configuration.popUpMgrConfig.resetPopupTimerOnRV);
            EditorGUILayout.Separator();
        }
        IndicateConfiguration("Analytics", _configuration.analyticsConfig.IsValid(), _configuration.analyticsConfig.included);
        IndicateConfiguration("Appsflyer", _configuration.appsflyerConfig.IsValid(), _configuration.appsflyerConfig.included);
        IndicateConfiguration("Crash Tool", _configuration.crashToolConfig.IsValid(), _configuration.crashToolConfig.included);
        IndicateConfiguration("Banners", _configuration.globalConfig.IsValid() && _configuration.bannersConfig.IsValid(), _configuration.bannersConfig.included);
        IndicateConfiguration("Interstitials", _configuration.globalConfig.IsValid() &&_configuration.interstitialsConfig.IsValid(), _configuration.interstitialsConfig.included);
        IndicateConfiguration("Rewarded Ads", _configuration.globalConfig.IsValid() && _configuration.rewardedAdsConfig.IsValid(), _configuration.rewardedAdsConfig.included);
        IndicateConfiguration("Rewarded Interstitials", _configuration.globalConfig.IsValid() && _configuration.rewardedInterConfig.IsValid(), _configuration.rewardedInterConfig.included);
        IndicateConfiguration("Open Ads", _configuration.globalConfig.IsValid() && _configuration.openAdsConfig.IsValid(), _configuration.openAdsConfig.included);
        IndicateConfiguration("Rate Us", true, _configuration.rateUsConfig.included);
        IndicateConfiguration("Privacy Settings", true, _configuration.privacySettingsConfig.included);
        
        GUILayout.FlexibleSpace();
        
        
        if (GUILayout.Button("Load Configuration"))
        {
            UnzipAnLoadConfiguration();
        }
        if (_configuration != null && _configuration.popUpMgrConfig.included && GUILayout.Button("Save Pop Up Mgr Configuration"))
        {
            _configuration.SaveConfigurations();
        }
        GUILayout.Space(10);
    }
    
    
    private interface IConfig
    {
        Dictionary<string, object> ToDict();
        string GetServiceName();
        void LoadFromFile();
    }

    private class GlobalConfig : IConfig
    {
        public string admobAppId;
        public string bundleId;
        public bool testMode;
        public string store;
        public string conversionModelType = "A";
        public bool isAdMob = true;
        
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_GLOBAL_BUNDLE_ID, bundleId},
                {Constants.CONFIG_KEY_GLOBAL_IS_ADMOB, isAdMob},
                {Constants.CONFIG_KEY_GLOBAL_TEST_MODE, testMode},
                {Constants.CONFIG_KEY_GLOBAL_STORE, store},
                {Constants.CONFIG_KEY_GLOBAL_GAME_ENGINE, "unity"},
                {Constants.CONFIG_KEY_GLOBAL_AUDIENCE_MODE, "non-children"},
                {Constants.CONFIG_KEY_GLOBAL_ORIENTATION, "portrait"},
                {Constants.CONFIG_KEY_GLOBAL_CONVERSION_MODEL_TYPE, conversionModelType},
                {Constants.CONFIG_KEY_GLOBAL_APPBUILDCONFIG, new Dictionary<string, object>()
                {
                    {Constants.CONFIG_KEY_GLOBAL_ADMOB, new Dictionary<string, object>()
                        {
                            {Constants.CONFIG_KEY_GLOBAL_APPLICATION, admobAppId}
                        }
                    }}
                }
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_GLOBAL;
        }
        

        public void LoadFromFile()
        {
            var fp = Constants.CONFIG_FILE_PATH + GetServiceName() + ".json";
            if (File.Exists(fp))
            {
                var json = File.ReadAllText(fp);
                Deserialize(json);
#if UNITY_IOS
                if (!string.IsNullOrEmpty(conversionModelType))
                {
                    var conversionRulesUrl = "http://promo-images.ttpsdk.info/conversionJS/"+conversionModelType+"/conversion.js";
                    TTPEditorUtils.DownloadStringToFile(conversionRulesUrl, "Assets/StreamingAssets/ttp/conversion/conversion.js");
                }
#endif
            }
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(admobAppId);
        }

        private void Deserialize(string json)
        {
            if (json != null)
            {
                if (TTPJson.Deserialize(json) is Dictionary<string, object> dict)
                {
                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_STORE) &&
                        dict[Constants.CONFIG_KEY_GLOBAL_STORE] is string ttpStore)
                    {
                        store = ttpStore;
                    }

                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_BUNDLE_ID) &&
                        dict[Constants.CONFIG_KEY_GLOBAL_BUNDLE_ID] is string bid)
                    {
                        bundleId = bid;
                    }

                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_TEST_MODE) &&
                        dict[Constants.CONFIG_KEY_GLOBAL_TEST_MODE] is bool tm)
                    {
                        testMode = tm;
                    }
                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_IS_ADMOB) &&
                       dict[Constants.CONFIG_KEY_GLOBAL_IS_ADMOB] is bool is_admob)
                    {
                        isAdMob = is_admob;
                    }
                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_APPBUILDCONFIG) &&
                        dict[Constants.CONFIG_KEY_GLOBAL_APPBUILDCONFIG] is Dictionary<string, object> appBuildConfigDict &&
                        appBuildConfigDict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_ADMOB) && appBuildConfigDict[Constants.CONFIG_KEY_GLOBAL_ADMOB] is Dictionary<string, object> admobDict &&
                        admobDict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_APPLICATION) && admobDict[Constants.CONFIG_KEY_GLOBAL_APPLICATION] is string applicationStr)
                    {
                        admobAppId = applicationStr;
                    }

                    if (dict.ContainsKey(Constants.CONFIG_KEY_GLOBAL_CONVERSION_MODEL_TYPE) && dict[Constants.CONFIG_KEY_GLOBAL_CONVERSION_MODEL_TYPE] is string conversionModelTypeVal)
                    {
                        conversionModelType = conversionModelTypeVal;
                    }
                }
            }
        }
    }

    [Serializable]
    private class AppsflyerConfig : IConfig
    {
        public bool included;
        public string appsFlyerKey;

        private bool _isValid;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_APPSFLYER, appsFlyerKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_APPSFLYER;
        }
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(appsFlyerKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }
    
    [Serializable]
    private class CrashToolConfig : IConfig
    {
        public bool included;
        public string hockeyAppKey;

        private bool _isValid;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_HOCKEYAPP, hockeyAppKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_CRASHTOOL;
        }
        
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(hockeyAppKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }
    
    [Serializable]
    private class ElephantConfig : IConfig
    {
        public bool included;
        private string serverDomain;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_SERVER_DOMAIN, serverDomain}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_ELEPHANT;
        }
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
        }
    }
    
    private class PopUpMgrConfig : IConfig
    {
        public bool included;
        public long sessionTimeToFirstPopup;
        public long gameTimeToFirstPopup;
        public long levelToFirstPopup;
        public long firstPopupAtSession;
        public bool resetPopupTimerOnRV;
        public long popupsInterval;
        private Dictionary<string,object> popupsIntervalsBySession;
        private Dictionary<string,object> gameTimeToFirstPopupBySession;
        private Dictionary<string,object> sessionTimeToFirstPopupBySession;
        private Dictionary<string,object> resetPopupTimerOnRVBySession;

        public Dictionary<string, object> ToDict()
        {
            var dic = new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_POPUPMGR_SESSION_TIME_TO_FIRST_POPUP, sessionTimeToFirstPopup},
                {Constants.CONFIG_KEY_POPUPMGR_GAME_TIME_TO_FIRST_POPUP, gameTimeToFirstPopup},
                {Constants.CONFIG_KEY_POPUPMGR_LEVEL_TO_FIRST_POPUP, levelToFirstPopup},
                {Constants.CONFIG_KEY_POPUPMGR_FIRST_POPUP_AT_SESSION, firstPopupAtSession},
                {Constants.CONFIG_KEY_POPUPMGR_RESET_POPUP_TIMER_ON_RV, resetPopupTimerOnRV},
                {Constants.CONFIG_KEY_POPUPMGR_INTERVALS, new List<object>(){popupsInterval}},
                {Constants.CONFIG_KEY_POPUPMGR_POPUP_INTERVALS_BY_SESSION, new Dictionary<string,List<object>>(){ {"1", new List<object>(){popupsInterval}}}},
                {Constants.CONFIG_KEY_POPUPMGR_GAME_TIME_TO_FIRST_POPUP_BY_SESSION, new Dictionary<string,long>(){ {"1", gameTimeToFirstPopup}}},
                {Constants.CONFIG_KEY_POPUPMGR_SESSION_TIME_TO_FIRST_POPUP_BY_SESSION, new Dictionary<string,long>(){ {"1", sessionTimeToFirstPopup}}},
                {Constants.CONFIG_KEY_POPUPMGR_RESET_RV_BY_SESSION, new Dictionary<string,bool>(){ {"1", resetPopupTimerOnRV}}},
            };
            return dic;

        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_POPUPSMGR;
        }
        
        public void LoadFromFile()
        {
            var fp = Constants.CONFIG_FILE_PATH + GetServiceName() + ".json";
            if (File.Exists(fp))
            {
                var json = File.ReadAllText(fp);
                Deserialize(json);
            }
        }

        private object GetInnerObject(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) &&
                dict[key] is Dictionary<string, object>
                    innerDict)
            {
                if (innerDict.ContainsKey("1") && innerDict["1"] is List<object> arr)
                {
                    if (arr.Count > 0)
                    {
                        return arr[0];
                    }
                }
            }
            return null;
        }
        
        private int GetInnerInt(Dictionary<string, object> dict, string key)
        {
            return Convert.ToInt32(GetInnerObject(dict, key));
        }
        
        private bool GetInnerBool(Dictionary<string, object> dict, string key)
        {
            return Convert.ToBoolean(GetInnerObject(dict, key));
        }
        
        private void Deserialize(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (TTPJson.Deserialize(json) is Dictionary<string, object> dict)
                {
                    included = dict.ContainsKey(Constants.CONFIG_KEY_INCLUDED) && (bool)dict[Constants.CONFIG_KEY_INCLUDED];
                    popupsInterval = GetInnerInt(dict, Constants.CONFIG_KEY_POPUPMGR_POPUP_INTERVALS_BY_SESSION);
                    gameTimeToFirstPopup = GetInnerInt(dict,
                        Constants.CONFIG_KEY_POPUPMGR_GAME_TIME_TO_FIRST_POPUP_BY_SESSION);
                    sessionTimeToFirstPopup = GetInnerInt(dict,
                        Constants.CONFIG_KEY_POPUPMGR_SESSION_TIME_TO_FIRST_POPUP_BY_SESSION);
                    resetPopupTimerOnRV = GetInnerBool(dict, Constants.CONFIG_KEY_POPUPMGR_RESET_RV_BY_SESSION);
                    levelToFirstPopup = dict.ContainsKey(Constants.CONFIG_KEY_POPUPMGR_LEVEL_TO_FIRST_POPUP) ? (long)dict[Constants.CONFIG_KEY_POPUPMGR_LEVEL_TO_FIRST_POPUP] : 0;
                    firstPopupAtSession = dict.ContainsKey(Constants.CONFIG_KEY_POPUPMGR_FIRST_POPUP_AT_SESSION) ? (long)dict[Constants.CONFIG_KEY_POPUPMGR_FIRST_POPUP_AT_SESSION] : 0;
                }
            }
        }
    }
    
    [Serializable]
    private class PrivacySettingsConfig : IConfig
    {
        public bool included;
        public string consentFormVersion;
        public string consentFormURL;
        public string privacySettingsURL;
        public bool useTTPGDPRPopups;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_PRIVACY_SETTINGS_CONSENT_FORM_VERSION, consentFormVersion},
                {Constants.CONFIG_KEY_PRIVACY_SETTINGS_CONSENT_FORM_URL, consentFormURL},
                {Constants.CONFIG_KEY_PRIVACY_SETTINGS_URL, privacySettingsURL},
                {Constants.CONFIG_KEY_PRIVACY_SETTINGS_USE_TTP_POPUPS, useTTPGDPRPopups}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_PRIVACY_SETTINGS;
        }
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
        }
    }
    
    private class AnalyticsConfig : IConfig
    {
        public bool included;
        public string googleAppId;
        public string senderId;
        public string clientId;
        public string storageBucket;
        public string apiKey;
        public string projectId;

        private bool _isValid;

        public Dictionary<string,object> ToDict()
        {
            return new Dictionary<string, object>
            {
                {Constants.CONFIG_KEY_FIREBASE, 
                    new Dictionary<string,object>(){
                        {Constants.CONFIG_KEY_FIREBASE_GOOGLE_ID, googleAppId},
                        {Constants.CONFIG_KEY_FIREBASE_SENDER_ID, senderId},
                        {Constants.CONFIG_KEY_FIREBASE_CLIENT_ID, clientId},
                        {Constants.CONFIG_KEY_FIREBASE_STORAGE_BUCKET, storageBucket},
                        {Constants.CONFIG_KEY_FIREBASE_API_KEY, apiKey},
                        {Constants.CONFIG_KEY_FIREBASE_PROJECT_ID, projectId}
                    }
                }
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_ANALYTICS;
        }
        
        public void LoadFromFile()
        {
            var fp = Constants.CONFIG_FILE_PATH + GetServiceName() + ".json";
            if (File.Exists(fp))
            {
                var json = File.ReadAllText(fp);
                Deserialize(json);
            }
        }
        
        private void Deserialize(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (TTPJson.Deserialize(json) is Dictionary<string, object> dict &&
                    dict.ContainsKey(Constants.CONFIG_KEY_FIREBASE) &&
                    dict[Constants.CONFIG_KEY_FIREBASE] is Dictionary<string,object> firebaseDict)
                {
                    included = dict.ContainsKey(Constants.CONFIG_KEY_INCLUDED) && (bool)dict[Constants.CONFIG_KEY_INCLUDED];
                    googleAppId = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_GOOGLE_ID) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_GOOGLE_ID] as string : "";
                    senderId = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_SENDER_ID) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_SENDER_ID] as string : "";
                    clientId = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_CLIENT_ID) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_CLIENT_ID] as string : "";
                    storageBucket = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_STORAGE_BUCKET) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_STORAGE_BUCKET] as string : "";
                    apiKey = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_API_KEY) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_API_KEY] as string : "";
                    projectId = firebaseDict.ContainsKey(Constants.CONFIG_KEY_FIREBASE_PROJECT_ID) ? firebaseDict[Constants.CONFIG_KEY_FIREBASE_PROJECT_ID] as string : "";
                    
                    _isValid = !string.IsNullOrEmpty(googleAppId) &&
                               !string.IsNullOrEmpty(senderId) &&
                               !string.IsNullOrEmpty(clientId) &&
                               !string.IsNullOrEmpty(storageBucket) &&
                               !string.IsNullOrEmpty(apiKey) &&
                               !string.IsNullOrEmpty(projectId);
                }
            }
        }

        public bool IsValid()
        {
            return _isValid;
        }
        
    }
    
    [Serializable]
    private class RateUsConfig : IConfig
    {
        public bool included;
        public string iconUrl = "";
        public int coolDown = 3;

        private bool _isValid;
        
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_RATE_US_ICON_URL, iconUrl},
                {Constants.CONFIG_KEY_RATE_US_COOLDOWN, coolDown}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_RATEUS;
        }

        

        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(iconUrl);
            if (_isValid)
            {
                var extenstion = iconUrl.Substring(iconUrl.LastIndexOf(".", System.StringComparison.InvariantCultureIgnoreCase)+1);
                TTPEditorUtils.DownloadFile(iconUrl, "Assets/StreamingAssets/ttp/rateus/game_icon." + extenstion);
            }
        }
        
    }

    [Serializable]
    private class BannersConfig : IConfig
    {
        public bool included;
        public bool alignToTop;
        public long adDisplayTime;
        public string bannersAdMobKey;
        public string houseAdsServerDomain;

        private bool _isValid;
        
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_BANNERS_ALIGN_TO_TOP, alignToTop},
                {Constants.CONFIG_KEY_BANNERS_AD_DISPLAY_TIME, adDisplayTime},
                {Constants.CONFIG_KEY_BANNERS_ADMOB_KEY, bannersAdMobKey},
                {Constants.CONFIG_KEY_BANNERS_HOUSEADS_SERVER_DOMAIN, houseAdsServerDomain},
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_BANNERS;
        }
        
       
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(bannersAdMobKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }

    [Serializable]
    private class OpenAdsConfig : IConfig
    {
        public bool included;
        public string appOpenAdMobKey;
        
        private bool _isValid;
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_OPENADS, appOpenAdMobKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_OPENADS;
        }

        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(appOpenAdMobKey);
        }
        
        public bool IsValid()
        {
            return _isValid;
        }
    }

    [Serializable]
    private class InterstitialsConfig : IConfig
    {
        public bool included;
        public string interstitialsAdMobKey;
        
        private bool _isValid;
        
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_INTERSTITIALS, interstitialsAdMobKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_INTERSTITIALS;
        }
        
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(interstitialsAdMobKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }
    
    
    [Serializable]
    private class RewardedAdsConfig : IConfig
    {
        public bool included;
        public string rewardedAdsAdMobKey;
        
        private bool _isValid;
        
        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_RV, rewardedAdsAdMobKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_RV;
        }
        
        
        
        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(rewardedAdsAdMobKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }

    [Serializable]
    private class RewardedInterConfig : IConfig
    {
        public bool included;
        public string rewardedInterAdMobKey;

        private bool _isValid;

        public Dictionary<string, object> ToDict()
        {
            return new Dictionary<string, object>()
            {
                {Constants.CONFIG_KEY_RV_INTER, rewardedInterAdMobKey}
            };
        }

        public string GetServiceName()
        {
            return Constants.CONFIG_FN_RV_INTER;
        }



        public void LoadFromFile()
        {
            LoadConfigFromFile(this);
            _isValid = !string.IsNullOrEmpty(rewardedInterAdMobKey);
        }

        public bool IsValid()
        {
            return _isValid;
        }
    }

    private class PlatformConfiguration
    {
        public AppsflyerConfig appsflyerConfig = new AppsflyerConfig();
        public AnalyticsConfig analyticsConfig = new AnalyticsConfig();
        public BannersConfig bannersConfig = new BannersConfig();
        public InterstitialsConfig interstitialsConfig = new InterstitialsConfig();
        public CrashToolConfig crashToolConfig = new CrashToolConfig();
        public RewardedAdsConfig rewardedAdsConfig = new RewardedAdsConfig();
        public RewardedInterConfig rewardedInterConfig = new RewardedInterConfig();
        public PopUpMgrConfig popUpMgrConfig = new PopUpMgrConfig();
        public PrivacySettingsConfig privacySettingsConfig = new PrivacySettingsConfig();
        public RateUsConfig rateUsConfig = new RateUsConfig();
        public OpenAdsConfig openAdsConfig = new OpenAdsConfig();
        public GlobalConfig globalConfig = new GlobalConfig();

        private List<IConfig> _configs;
        private int _platformCode;

        public PlatformConfiguration(int platformCode)
        {
            _platformCode = platformCode;
            _configs = new List<IConfig>();
            _configs.Add(globalConfig);
            _configs.Add(appsflyerConfig);
            _configs.Add(analyticsConfig);
            _configs.Add(bannersConfig);
            _configs.Add(interstitialsConfig);
            _configs.Add(crashToolConfig);
            _configs.Add(rewardedAdsConfig);
            _configs.Add(rewardedInterConfig);
            _configs.Add(popUpMgrConfig);
            _configs.Add(privacySettingsConfig);
            _configs.Add(rateUsConfig);
            _configs.Add(openAdsConfig);
        }
        
        
        public void LoadConfigurationsFromFile()
        {
            foreach (var config in _configs)
            {
                config.LoadFromFile();
            }
        }
        
        public void SaveConfigurations()
        {
            SaveConfigurationToFile(popUpMgrConfig);
            UpdateAndroidRes(globalConfig,analyticsConfig);
        }

        private void SaveConfigurationToFile(IConfig config)
        {
            var json = TTPJson.Serialize(config.ToDict());
            var fp = Path.Combine(Constants.CONFIG_FILE_PATH, config.GetServiceName() + ".json");
            if (File.Exists(fp))
            {
                File.Delete(fp);
            }

            if (!Directory.Exists(Constants.CONFIG_FILE_PATH))
            {
                Directory.CreateDirectory(Constants.CONFIG_FILE_PATH);
            }

            File.WriteAllText(fp, json);
        }
    }
    

    private static void LoadConfigFromFile(IConfig config)
    {
        var fp = Constants.CONFIG_FILE_PATH + config.GetServiceName() + ".json";
        if (File.Exists(fp))
        {
            var json = File.ReadAllText(fp);
            if (!string.IsNullOrEmpty(json))
            {
                EditorJsonUtility.FromJsonOverwrite(json,config);
            }
        }
    }

    private static void UpdateAndroidRes(GlobalConfig globalConfig, AnalyticsConfig analyticsConfig)
    {
        var dic = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(globalConfig.admobAppId))
        {
            dic[Constants.RES_NAME_ADMOB_APP_ID] = globalConfig.admobAppId;
        }
        if (analyticsConfig.IsValid())
        {
            dic[Constants.RES_NAME_SENDER_ID] = analyticsConfig.senderId;
            dic[Constants.RES_NAME_STORAGE_BUCKET] = analyticsConfig.storageBucket;
            dic[Constants.RES_NAME_PROJECT_ID] = analyticsConfig.projectId;
            dic[Constants.RES_NAME_API_KEY] = analyticsConfig.apiKey;
            dic[Constants.RES_NAME_APP_ID] = analyticsConfig.googleAppId;
            dic[Constants.RES_NAME_CLIENT_ID] = analyticsConfig.clientId;
        }
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(Constants.ANDROID_MANIFEST_RES_FILE_PATH);
        var resPath = xmlDoc.SelectSingleNode(Constants.ANDROID_MANIFEST_RES_PATH);
        foreach (var kvp in dic)
        {
            var node = GetXMlNode(xmlDoc ,resPath, kvp.Key);
            var nameAttribute = xmlDoc.CreateAttribute("name");
            nameAttribute.Value = kvp.Key;
            var translatableAttribute = xmlDoc.CreateAttribute("translatable");
            translatableAttribute.Value = "false";
            node.Attributes.Append(nameAttribute);
            node.Attributes.Append(translatableAttribute);
            node.InnerText = kvp.Value;
            resPath.AppendChild(node);
        }
        
        xmlDoc.Save(Constants.ANDROID_MANIFEST_RES_FILE_PATH);
    }
    
    private static XmlNode GetXMlNode(XmlDocument xmlDocument, XmlNode xmlNode, string nameAttrVal, string type = "string")
    {
        var nodes = xmlNode.SelectNodes(type);
        if (nodes != null)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes?["name"] != null && node.Attributes["name"].Value == nameAttrVal)
                {
                    return node;
                }
            }
        }
        return xmlDocument.CreateNode(XmlNodeType.Element, type, null);
    }
    
    private static PlatformConfiguration _configuration = new PlatformConfiguration(0);
    
    private int _selectedPlatform = 0;

    

    private void OnEnable()
    {
        _greenIndicator.alignment = TextAnchor.MiddleRight;
        _greenIndicator.fixedHeight = 25;
        _greenIndicator.padding.right = 10;
        _greenIndicator.padding.top = 5;
        _okTexture = GetTexture("Assets/Tabtale/TTPlugins/CLIK/Editor/ok.png");
        _xTexture = GetTexture("Assets/Tabtale/TTPlugins/CLIK/Editor/x.png");
        _okGuiContent = new GUIContent(_okTexture, "Configured");
        _xGuiContent = new GUIContent(_xTexture, "Not Configured");
        _redLabel.normal.textColor = Color.red;
        _redLabel.padding.left = 10;
        _greenLabel.normal.textColor = Color.green;
        _greenLabel.padding.left = 10;
        _nonbreakingLabelStyle.wordWrap = false;
        _nonbreakingLabelStyle.normal.textColor = Color.white;
        _nonbreakingLabelStyle.alignment = TextAnchor.MiddleCenter;
        _configuration.LoadConfigurationsFromFile();
    }

    private Texture GetTexture(string path)
    {
        Texture t = new Texture2D( 1, 1 );
        ((Texture2D)t).LoadImage( System.IO.File.ReadAllBytes( path) );
        ((Texture2D)t).Apply();
        return t;
    }
    

    private GUIContent _okGuiContent;
    private GUIContent _xGuiContent;
    private Texture _okTexture;
    private Texture _xTexture;
    
    
    GUIStyle _nonbreakingLabelStyle = new GUIStyle();
   
    private GUIStyle _redLabel = new GUIStyle();
    private GUIStyle _greenLabel = new GUIStyle();
    private GUIStyle _greenIndicator = new GUIStyle();

    private static bool UnzipAnLoadConfiguration()
    {
        var zipPath = EditorUtility.OpenFilePanel("Choose Configuration Zip", "", "zip");
        if (string.IsNullOrEmpty(zipPath))
        {
            return false;
        }
        if (Directory.Exists(Constants.CONFIG_FILE_PATH))
        {
            Directory.Delete(Constants.CONFIG_FILE_PATH, true);
        }
        PlayerPrefs.SetInt(Constants.PLAYER_PREFS_KEY_FIRST_CONFIGURATION,1);
        ZipUtil.Unzip(zipPath,Constants.CONFIG_FILE_PATH);
        _configuration = new PlatformConfiguration(0);
        _configuration.LoadConfigurationsFromFile();
        ModulateClik();
        return true;
    }

    private void IndicateConfiguration(string labelText, bool valid, bool included)
    {
        if(!included)
            return;
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label (labelText, EditorStyles.boldLabel);
        if (valid)
        {
            GUILayout.Label(_okGuiContent, _greenIndicator);
        }
        else
        {
            GUILayout.Label(_xGuiContent, _greenIndicator);
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private static TTPIncludedServicesScriptableObject GetInclusionScriptableObject()
    {
        var path = "Assets/Tabtale/TTPlugins/CLIK/Resources/ttpIncludedServices.asset";
        if (File.Exists(path))
        {
            Debug.Log("GetInclusionScriptableObject ::  1");
            return AssetDatabase.LoadAssetAtPath<TTPIncludedServicesScriptableObject>("Assets/Tabtale/TTPlugins/CLIK/Resources/ttpIncludedServices.asset");
        }
        else
        {
            Debug.Log("GetInclusionScriptableObject ::  2");
            var includedServicesScriptableObject = ScriptableObject.CreateInstance<TTPIncludedServicesScriptableObject>();
            if (!Directory.Exists("Assets/Tabtale/TTPlugins/CLIK/Resources"))
            {
                Directory.CreateDirectory("Assets/Tabtale/TTPlugins/CLIK/Resources");
            }
            AssetDatabase.CreateAsset(includedServicesScriptableObject, path);
            AssetDatabase.SaveAssets();
            return includedServicesScriptableObject;
        }
        
    }

    private static void SaveInclusionScriptableObject(TTPIncludedServicesScriptableObject objToCpy)
    {
        var curObj = GetInclusionScriptableObject();
        curObj.analytics = objToCpy.analytics;
        curObj.appsFlyer = objToCpy.appsFlyer;
        curObj.crashTool = objToCpy.crashTool;
        curObj.banners = objToCpy.banners;
        curObj.interstitials = objToCpy.interstitials;
        curObj.rvs = objToCpy.rvs;
        curObj.privacySettings = objToCpy.privacySettings;
        curObj.rateUs = objToCpy.rateUs;
        curObj.openAds = objToCpy.openAds;
        EditorUtility.SetDirty(curObj);
        AssetDatabase.SaveAssets();
    }
    
    private static void ModulateClik()
    {
        Debug.Log("ModulateClik");
        var ttpIncludedServices = GetInclusionScriptableObject();
        ttpIncludedServices.appsFlyer = _configuration.appsflyerConfig.included;
        ttpIncludedServices.analytics = _configuration.analyticsConfig.included;
        ttpIncludedServices.crashTool = _configuration.crashToolConfig.included;
        ttpIncludedServices.banners = _configuration.bannersConfig.included;
        ttpIncludedServices.interstitials = _configuration.interstitialsConfig.included;
        ttpIncludedServices.rvs = _configuration.rewardedAdsConfig.included;
        ttpIncludedServices.rvInter = _configuration.rewardedInterConfig.included;
        ttpIncludedServices.privacySettings = _configuration.privacySettingsConfig.included;
        ttpIncludedServices.rateUs = _configuration.rateUsConfig.included;
        ttpIncludedServices.openAds = _configuration.openAdsConfig.included;

        var dic = new Dictionary<string, bool>()
        {
            {"ttpIncludedServices.appsFlyer", _configuration.appsflyerConfig.included},
            {"ttpIncludedServices.analytics", _configuration.analyticsConfig.included},
            {"ttpIncludedServices.crashTool", _configuration.crashToolConfig.included},
            {"ttpIncludedServices.banners", _configuration.bannersConfig.included},
            {"ttpIncludedServices.interstitials", _configuration.interstitialsConfig.included},
            {"ttpIncludedServices.rvs",_configuration.rewardedAdsConfig.included},
            {"ttpIncludedServices.rvInter",_configuration.rewardedInterConfig.included},
            {"ttpIncludedServices.privacySettings", _configuration.privacySettingsConfig.included},
            {"ttpIncludedServices.rateUs", _configuration.rateUsConfig.included},
            {"ttpIncludedServices.openAds", _configuration.openAdsConfig.included}
        };

        var msg = "";   
        foreach (var kvp in dic)
        {
            msg += kvp.Key + "=" + kvp.Value + "\n";
        }
        Debug.Log(msg);
        SaveInclusionScriptableObject(ttpIncludedServices);
        
    }

    
    
    private class AndroidGradlePreprocess : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            var ttpVersion = File.ReadAllText("Assets/Tabtale/TTPlugins/version.txt");
            var ttpVersionGradleProperty = "ttp_version=" + ttpVersion ?? "null";
            File.AppendAllLines(Path.Combine(path, "gradle.properties"), new string[]
            {
               "\nandroid.useAndroidX=true",
               "android.enableJetifier=true",
               "android.enableD8.desugaring=true",
               "android.enableIncrementalDesugaring=false",
               ttpVersionGradleProperty
            });
            File.AppendAllLines(Path.Combine(path, Path.Combine("..","gradle.properties")), new string[]
            {
                "\nandroid.useAndroidX=true",
                "android.enableJetifier=true",
                "android.enableD8.desugaring=true",
                "android.enableIncrementalDesugaring=false",
                ttpVersionGradleProperty
            });

            var buildGradlePath = Path.Combine(path, "build.gradle");
            Debug.Log(path + ", " + Application.productName + ", \n" + buildGradlePath);
            var currGradle = File.ReadAllText(buildGradlePath);
            Debug.Log("currGradle = " + currGradle);
            var globalConfig = new GlobalConfig();
            globalConfig.LoadFromFile();
            var modifiedGradle = DetermineIncludedServices(currGradle, globalConfig.isAdMob);
            Debug.Log("modifiedGradle = " + modifiedGradle);
            File.WriteAllText(buildGradlePath,modifiedGradle);
        }
        
        private static string DetermineIncludedServices(string mainGradle, bool isAdMob)
        {
            var includedServices = GetInclusionScriptableObject();
            if (includedServices == null)
            {
                Debug.LogError("CreatePodDependencies:: included services is null! will not build correctly.");
                return null;
            }
            
            var exclusionsList = new List<string>
            {
                "TT_Plugins_Billing",
                "TT_Plugins_CrossDevicePersistency",
                "TT_Plugins_CrossPromotion",
                "TT_Plugins_DeltaDnaAgent",
                "TT_Plugins_FlurryAgent",
                "TT_Plugins_NativeCampaign",
                "TT_Plugins_Promotion",
                "TT_Plugins_Share",
                "TT_Plugins_Social",

            };
            if (!includedServices.analytics)
            {
                exclusionsList.Add("TT_Plugins_Analytics");
                exclusionsList.Add("TT_Plugins_FirebaseAgent");
            }
            if (!includedServices.appsFlyer)
            {
                exclusionsList.Add("TT_Plugins_AppsFlyer");
            }
            if (!includedServices.banners)
            {
                exclusionsList.Add("TT_Plugins_Banners_Admob");
                exclusionsList.Add("TT_Plugins_Banners_Mopub");
                exclusionsList.Add("TT_Plugins_Banners_MoPub");
                exclusionsList.Add("TT_Plugins_Elephant");
            } else {
                if (isAdMob) {
                    exclusionsList.Add("TT_Plugins_Banners_Mopub");
                    exclusionsList.Add("TT_Plugins_Banners_MoPub");
                } else {
                   exclusionsList.Add("TT_Plugins_Banners_Admob");
                }
            }
            if (!includedServices.rvs)
            {
                exclusionsList.Add("TT_Plugins_RewardedAds_Admob");
                exclusionsList.Add("TT_Plugins_RewardedAds_Mopub");
                exclusionsList.Add("TT_Plugins_RewardedAds_MoPub");
            } else {
                if (isAdMob) {
                    exclusionsList.Add("TT_Plugins_RewardedAds_Mopub");
                    exclusionsList.Add("TT_Plugins_RewardedAds_MoPub");
                } else {
                   exclusionsList.Add("TT_Plugins_RewardedAds_Admob");
                }
            }
            if (!includedServices.interstitials)
            {
                exclusionsList.Add("TT_Plugins_Interstitials_Admob");
                exclusionsList.Add("TT_Plugins_Interstitials_Mopub");
                exclusionsList.Add("TT_Plugins_Interstitials_MoPub");
            } else {
                if (isAdMob) {
                    exclusionsList.Add("TT_Plugins_Interstitials_Mopub");
                    exclusionsList.Add("TT_Plugins_Interstitials_MoPub");
                } else {
                   exclusionsList.Add("TT_Plugins_Interstitials_Admob");
                }
            }
            if (!includedServices.openAds || !isAdMob)
            {
                exclusionsList.Add("TT_Plugins_OpenAds");
            }
            if (!includedServices.rvInter || !isAdMob)
            {
                exclusionsList.Add("TT_Plugins_RewardedInterstitials");
            }
            if (!includedServices.banners && !includedServices.rvs && !includedServices.openAds && !includedServices.rvInter)
            {
                exclusionsList.Add("TT_Plugins_PopupMgr");
            }
            if (!includedServices.crashTool)
            {
                exclusionsList.Add("TT_Plugins_CrashTool");
            }
            if (!includedServices.privacySettings)
            {
                exclusionsList.Add("TT_Plugins_Privacy_Settings");
            }
            if (!includedServices.openAds && !includedServices.interstitials && !includedServices.rvs && !includedServices.banners)
            {
                exclusionsList.Add("TT_Plugins_ECPM");
            }
            var exclusions = "";
            foreach (var excludedService in exclusionsList)
            {
                exclusions += "exclude module: '" + excludedService + "'\n";
            }
            var implementationStr = "implementation ('com.tabtale.tt_plugins.android:TT_Plugins:'+ttp_version){\n"
                                    + exclusions
                                    + "}\n";
            return mainGradle.Replace("@@TT_PLUGINS_DEPENDENCIES@@", implementationStr);
        }
    }
#if UNITY_IOS 
    private class PostProcess : IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        public void OnPostprocessBuild(BuildReport report)
        {
            var plistPath = Path.Combine(report.summary.outputPath, "Info.plist");
            var plist = new PlistDocument();
            var globalConfig = new GlobalConfig();
            globalConfig.LoadFromFile();
            plist.ReadFromFile(plistPath);
            plist.root.SetString("GADApplicationIdentifier", globalConfig.admobAppId);
            plist.root.SetString("AppLovinSdkKey", "yRHC8kgWwG5S4lOh7Dx_pZB2iEBLVWMSzde5MKbGahifQ6MTKIT7tk9ZzLvTsFwptZvDuVTTBB8cHU9bohkeQu");
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
#endif
    private class ConfigurationSaverPreprocess : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("OnPreprocessBuild " + report.summary.platform);
            var analyticsConfig = new AnalyticsConfig();
            var globalConfig = new GlobalConfig();
            analyticsConfig.LoadFromFile();
            globalConfig.LoadFromFile();
            UpdateAndroidRes(globalConfig,analyticsConfig);

            if (report.summary.platform == BuildTarget.iOS)
            {
                CreatePodDependencies(globalConfig.isAdMob);
            }
            
        }
        
        private static void CreatePodDependencies(bool isAdMob)
        {
            var includedServices = GetInclusionScriptableObject();
            if (includedServices == null)
            {
                Debug.LogError("CreatePodDependencies:: included services is null! will not build correctly.");
                return;
            }

            var services = new List<string>
            {
                "TT_Plugins_Core",

            };
            if (includedServices.appsFlyer)
            {
                services.Add("TT_Plugins_AppsFlyer");
            }
            if (includedServices.crashTool)
            {
                services.Add("TT_Plugins_CrashTool");
            }
            if (includedServices.privacySettings)
            {
                services.Add("TT_Plugins_Privacy_Settings");
            }
            bool popUpMgrIncluded = false;
            if (includedServices.banners)
            {
                popUpMgrIncluded = true;
                if (isAdMob)
                {
                    services.Add("TT_Plugins_Banners_Admob");
                }
                else
                {
                    services.Add("TT_Plugins_Banners_MoPub");
                }
                services.Add("TT_Plugins_Elephant");
            }
            if (includedServices.interstitials)
            {
                popUpMgrIncluded = true;
                if (isAdMob)
                {
                    services.Add("TT_Plugins_Interstitials_Admob");
                }
                else
                {
                    services.Add("TT_Plugins_Interstitials_MoPub");
                }
            }
            if (includedServices.rvs)
            {
                popUpMgrIncluded = true;
                if (isAdMob)
                {
                    services.Add("TT_Plugins_RewardedAds_Admob");
                }
                else
                {
                    services.Add("TT_Plugins_RewardedAds_MoPub");
                }
            }
            if (includedServices.openAds && isAdMob)
            {
                popUpMgrIncluded = true;
                services.Add("TT_Plugins_OpenAds");
            }
            if (includedServices.rvInter && isAdMob)
            {
                popUpMgrIncluded = true;
                services.Add("TT_Plugins_RewardedInterstitials");
            }
            if (includedServices.rvs || includedServices.interstitials || includedServices.banners || includedServices.openAds || includedServices.rvInter)
            {
                services.Add("TT_Plugins_ECPM");
            }
            if (popUpMgrIncluded)
            {
                services.Add("TT_Plugins_PopupMgr");
            }
            if (includedServices.analytics)
            {
                services.Add("TT_Plugins_Analytics");
                services.Add("TT_Plugins_FirebaseAgent");
                services.Add("TT_Plugins_Remote_Config");
                
            }
            
            var json = File.ReadAllText("Assets/Tabtale/TTPlugins/TT_Plugins.json");
            if (json != null)
            {
                var dic = TTPJson.Deserialize(json) as Dictionary<string, object>;
                var xmlDoc = new XmlDocument();
                xmlDoc.Load("Assets/Editor/TTPDependencies.xml");
                var iosPods = xmlDoc.SelectSingleNode("//dependencies/iosPods");
                if (iosPods != null)
                {
                    var toRemove = new List<string>();
                    foreach (var kvp in dic)
                    {
                        if (kvp.Key.Contains("TT_Plugins") && !services.Contains(kvp.Key))
                        {
                            
                            toRemove.Add(kvp.Key);
                        }
                    }

                    foreach (var key in toRemove)
                    {
                        XmlNode nodeToRemove = null;
                        foreach (XmlNode childNode in iosPods.ChildNodes)
                        {
                            if (childNode.Attributes?["name"] != null && childNode.Attributes["name"].Value == key+"_Pod")
                            {
                                Debug.Log("removing " + key);
                                nodeToRemove = childNode;
                            }
                        }
                        if(nodeToRemove != null)
                            iosPods.RemoveChild(nodeToRemove);
                        dic.Remove(key);
                    }
                    
                    foreach (var kvp in dic)
                    {
                        Debug.Log(kvp.Key + ", " + kvp.Value);
                        if (services.Contains(kvp.Key))
                        {
                            var podName = kvp.Key + "_Pod";
                            var node = GetXMlNode(xmlDoc, iosPods, podName, "iosPod");
                            var nameAttribute = xmlDoc.CreateAttribute("name");
                            var versionAttribute = xmlDoc.CreateAttribute("version");
                            var minTargetSdkAttribute = xmlDoc.CreateAttribute("minTargetSdk");
                            nameAttribute.Value = podName;
                            versionAttribute.Value = kvp.Value as string;
                            minTargetSdkAttribute.Value = "10.0";
                            node.Attributes.Append(nameAttribute);
                            node.Attributes.Append(versionAttribute);
                            node.Attributes.Append(minTargetSdkAttribute);
                            iosPods.AppendChild(node);
                        }
                    }
                }
                xmlDoc.Save("Assets/Editor/TTPDependencies.xml");
            }
        }
    }
}
