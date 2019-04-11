using Slack;
using UnityEngine;

public class Sample : MonoBehaviour
{
    #region Field

    private string message = "Your message.";

    #endregion Field

    #region Method

    protected virtual void OnGUI() 
    {
        GUILayout.BeginArea(new Rect(10, 10, 500, 100));

        this.message = GUILayout.TextField(this.message);

        if (GUILayout.Button("Post Your Message"))
        {
            SlackManager.Instance.PostMessage(this.message,
                                             (request) => { Debug.Log("SUCCESS"); },
                                             (request) => { Debug.Log("ERROR : " + request.error); });
        }

        if (GUILayout.Button("Upload Screenshot"))
        {
            SlackManager.Instance.UploadScreenshot();
        }

        GUILayout.EndArea();
    }

    #endregion Method
}