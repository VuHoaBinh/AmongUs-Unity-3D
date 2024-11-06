using System;
using Codice.Client.Common;
using Codice.CM.Common;
using Codice.LogWrapper;
using PlasticGui.WorkspaceWindow.Home;
using Unity.PlasticSCM.Editor.Configuration.CloudEdition.Welcome;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

namespace Unity.PlasticSCM.Editor.Configuration
{
    internal static class AutoConfig
    {
        internal static TokenExchangeResponse PlasticCredentials(
            string unityAccessToken,
            string serverName)
        {
            SetupUnityEditionToken.CreateCloudEditionToken();

            var startTick = Environment.TickCount;

            var tokenExchangeResponse = WebRestApiClient.PlasticScm.TokenExchange(unityAccessToken);

            mLog.DebugFormat("TokenExchange time {0} ms", Environment.TickCount - startTick);

            if (tokenExchangeResponse == null)
            {
                mLog.Warn("Token exchange response null");
                Debug.LogWarning("Token exchange response null");
                return null;
            }

            if (tokenExchangeResponse.Error != null)
            {
                var warning = string.Format("Unable to exchange token: {0} [code {1}]",
                    tokenExchangeResponse.Error.Message, tokenExchangeResponse.Error.ErrorCode);
                mLog.ErrorFormat(warning);
                Debug.LogWarning(warning);
                return tokenExchangeResponse;
            }

            if (string.IsNullOrEmpty(tokenExchangeResponse.AccessToken))
            {
                var warning = string.Format("Access token is empty for user: {0}", 
                    tokenExchangeResponse.User);
                mLog.InfoFormat(warning);
                Debug.LogWarning(warning);
            }
            
            // This creates the client.conf if needed but doesn't overwrite it if it exists already,
            // and it also updates the profiles.conf and tokens.conf with the new AccessToken
            UserAccounts.SaveAccount(
                serverName,
                SEIDWorkingMode.SSOWorkingMode, // Hub sign-in working mode
                tokenExchangeResponse.User,
                tokenExchangeResponse.AccessToken,
                null,
                null,
                null);

            CloudEditionWelcomeWindow.JoinCloudServer(
                serverName,
                tokenExchangeResponse.User);

            return tokenExchangeResponse;
        }

        static readonly ILog mLog = PlasticApp.GetLogger("AutoConfig");
    }
}

