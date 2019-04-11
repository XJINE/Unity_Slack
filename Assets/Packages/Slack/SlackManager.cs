using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Slack
{
    // NOTE:
    // This is just a simple manager. That is why this class is implemented as Singleton.
    // If you need multiple usernames, channels, or any others, use API directly.

    public class SlackManager : SingletonMonoBehaviour<SlackManager>
    {
        #region Field

        public string token    = "Slack Token";
        public string channel  = "Channel Name";
        public string userName = "Slack Manager";

        #endregion Field

        #region Method

        protected override void OnDestroy()
        {
            base.StopAllCoroutines();
            base.OnDestroy();
        }

        public virtual void PostMessage(string text,
                                        Action<UnityWebRequest> onSuccess = null,
                                        Action<UnityWebRequest> onError   = null)
        {
            StartCoroutine(SlackAPI.PostMessage(this.token,
                                                this.channel,
                                                this.userName,
                                                text,
                                                onSuccess,
                                                onError));
        }

        public virtual void UploadFile(string text,
                                       string filename,
                                       byte[] filedata,
                                       Action<UnityWebRequest> onSuccess = null,
                                       Action<UnityWebRequest> onError   = null)
        {
            StartCoroutine(SlackAPI.UploadFile(this.token,
                                               this.channel,
                                               this.userName,
                                               text,
                                               filename,
                                               filedata,
                                               onSuccess,
                                               onError));
        }

        public virtual void UploadTexture(string text,
                                          string filename,
                                          Texture2D texture,
                                          Action<UnityWebRequest> onSuccess = null,
                                          Action<UnityWebRequest> onError   = null)
        {
            StartCoroutine(SlackAPI.UploadTexture(this.token,
                                                  this.channel,
                                                  this.userName,
                                                  text,
                                                  filename,
                                                  texture,
                                                  onSuccess,
                                                  onError));
        }

        public virtual void UploadScreenshot(string text     = null,
                                             string filename = null,
                                             Action<UnityWebRequest> onSuccess = null,
                                             Action<UnityWebRequest> onError   = null)
        {
            string baseName = System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

            StartCoroutine(SlackAPI.UploadScreenshot(this.token,
                                                     this.channel,
                                                     this.userName,
                                                     text,
                                                     filename ?? baseName,
                                                     onSuccess,
                                                     onError));
        }

        #endregion Method
    }
}