using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Slack
{
    public static class SlackAPI
    {
        #region Field

        public static readonly string UsersListURL   = "https://slack.com/api/users.list";
        public static readonly string PostMessageURL = "https://slack.com/api/chat.postMessage";
        public static readonly string UploadFileURL  = "https://slack.com/api/files.upload";

        private static readonly string RegexID   = "\"id\":\"(.+?)\"";
        private static readonly string RegexName = "\"name\":\"(.+?)\"";

        #endregion Field

        #region Method

        // NOTE:
        // Generate~ methods are implemented to make some small custom WWWForm.
        // Developers are enabled to add some custom fields into them.

        public static WWWForm GenerateBaseForm(string token)
        {
            WWWForm wwwForm = new WWWForm();

            wwwForm.AddField("token", token); // Required

            return wwwForm;
        }

        public static WWWForm GenerateUserListForm(string token)
        {
            // NOTE:
            // https://api.slack.com/methods/users.list

            return SlackAPI.GenerateBaseForm(token);
        }

        public static WWWForm GeneratePostMessageForm(string token,
                                                      string channel,
                                                      string username,
                                                      string text)
        {
            // NOTE:
            // This implementation is a small set. There is more info in API.
            // https://api.slack.com/methods/chat.postMessage

            // NOTE:
            // "link_names" is needed to make "#channel" text into link.

            WWWForm wwwForm = SlackAPI.GenerateBaseForm(token);

            wwwForm.AddField("channel",    channel);  // Required
            wwwForm.AddField("text",       text);     // Required
            wwwForm.AddField("username",   username); // Optional
            wwwForm.AddField("link_names", 1);        // Optional

            return wwwForm;
        }

        public static WWWForm GenerateUploadFileForm(string token,
                                                     string channel,
                                                     string username,
                                                     string text,
                                                     string filename,
                                                     byte[] filedata)
        {
            // NOTE:
            // This implementation is a small set. There is more info in API.
            // https://api.slack.com/methods/files.upload

            WWWForm wwwForm = SlackAPI.GenerateBaseForm(token);

            wwwForm.AddField("channels",        channel);  // Required
            wwwForm.AddField("username",        username); // Optional
            if(text != null)
            wwwForm.AddField("initial_comment", text);     // Optional
            wwwForm.AddField("link_names",      1);        // Optional

            wwwForm.AddBinaryData("file", filedata, filename); // Optional

            return wwwForm;
        }

        public static IEnumerator GetUsersList(string token,
                                               Action<UnityWebRequest> onSuccess = null,
                                               Action<UnityWebRequest> onError   = null)
        {
            yield return SlackAPI.PostToSlack(SlackAPI.UsersListURL,
                                              SlackAPI.GenerateUserListForm(token),
                                              onSuccess,
                                              onError);
        }

        public static IEnumerator PostMessage(string token,
                                              string channel,
                                              string username,
                                              string text,
                                              Action<UnityWebRequest> onSuccess = null,
                                              Action<UnityWebRequest> onError   = null)
        {
            yield return SlackAPI.PostToSlack
            (SlackAPI.PostMessageURL,
             SlackAPI.GeneratePostMessageForm(token, channel, username, text),
             onSuccess,
             onError);
        }

        public static IEnumerator UploadFile(string token,
                                             string channel,
                                             string username,
                                             string text,
                                             string filename,
                                             byte[] filedata,
                                             Action<UnityWebRequest> onSuccess = null,
                                             Action<UnityWebRequest> onError   = null)
        {
            yield return PostToSlack
            (SlackAPI.UploadFileURL,
             SlackAPI.GenerateUploadFileForm(token, channel, username, text, filename, filedata),
             onSuccess,
             onError);
        }

        public static IEnumerator UploadTexture(string token,
                                                string channel,
                                                string username,
                                                string text,
                                                string filename,
                                                Texture2D texture,
                                                Action<UnityWebRequest> onSuccess = null,
                                                Action<UnityWebRequest> onError   = null)
        {
            yield return UploadFile
            (token, channel, username, text, filename, texture.EncodeToPNG(),
             onSuccess,
             onError);
        }

        public static IEnumerator UploadScreenshot(string token,
                                                   string channel,
                                                   string username,
                                                   string text,
                                                   string filename,
                                                   Action<UnityWebRequest> onSuccess = null,
                                                   Action<UnityWebRequest> onError = null)
        {
            yield return new WaitForEndOfFrame();

            int width  = Screen.width;
            int height = Screen.height;

            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            yield return UploadFile
            (token, channel, username, text, filename, texture.EncodeToPNG(),
             onSuccess,
             onError);

            GameObject.Destroy(texture);
        }

        public static IEnumerator PostToSlack(string url, WWWForm wwwForm,
                                              Action<UnityWebRequest> onSuccess = null,
                                              Action<UnityWebRequest> onError   = null)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, wwwForm);

            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                onError?.Invoke(request);

                yield break;
            }
            else
            {
                onSuccess?.Invoke(request);
            }
        }

        public static void DecodeUserListToDictionary
            (string userList, Dictionary<string, string> userListDictionary)
        {
            userListDictionary.Clear();
            
            var ids   = Regex.Matches(userList, SlackAPI.RegexID);
            var names = Regex.Matches(userList, SlackAPI.RegexName);

            for (int i = 0; i < ids.Count; i++)
            {
                userListDictionary.Add(names[i].Groups[1].Value,
                                         ids[i].Groups[1].Value);
            }
        }

        #endregion Method
    }
}