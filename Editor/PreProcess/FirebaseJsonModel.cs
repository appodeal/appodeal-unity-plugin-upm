using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper Disable CheckNamespace
namespace AppodealStack.UnityEditor.PreProcess
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public class FirebaseJsonModel
    {
        public ProjectInfo project_info;
        public List<Client> client;
        public string configuration_version;

        [Serializable]
        public class ProjectInfo
        {
            public string project_number;
            public string firebase_url;
            public string project_id;
            public string storage_bucket;
        }

        [Serializable]
        public class Client
        {
            public ClientInfo client_info;
            public List<OauthClient> oauth_client;
            public List<ApiKey> api_key;
            public Services services;

            [Serializable]
            public class ClientInfo
            {
                public string mobilesdk_app_id;
                public AndroidClientInfo android_client_info;

                [Serializable]
                public class AndroidClientInfo
                {
                    public string package_name;
                }
            }

            [Serializable]
            public class OauthClient
            {
                public string client_id;
                public int client_type;
            }

            [Serializable]
            public class ApiKey
            {
                public string current_key;
            }

            [Serializable]
            public class Services
            {
                public AppInviteService appinvite_service;

                [Serializable]
                public class AppInviteService
                {
                    public List<OtherPlatformOauthClient> other_platform_oauth_client;

                    [Serializable]
                    public class OtherPlatformOauthClient
                    {
                        public string client_id;
                        public int client_type;
                        public IosInfo ios_info;

                        [Serializable]
                        public class IosInfo
                        {
                            public string bundle_id;
                        }
                    }
                }
            }
        }
    }
}
