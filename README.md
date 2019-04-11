# Unity_Slack

<img src="https://github.com/XJINE/Unity_Slack/blob/master/Screenshot.png" width="100%" height="auto" />

``SlackAPI`` and ``SlackManager`` provides easy way to post some messages or upload file into Slack.

## Import to Your Project

You can import this asset from UnityPackage.

- [Slack.unitypackage](https://github.com/XJINE/Unity_Slack/blob/master/Slack.unitypackage)

### Dependencies

You have to import following assets to use this asset.

- [Unity_SingletonMonoBehaviour](https://github.com/XJINE/Unity_SingletonMonoBehaviour)

## How to Use

Set ``SlackManager`` into your object, and set ``Token``, ``Channel``, and ``UserName`` from Inspector.
And then, call various methods which post into your Slack.

- PostMessage
- UploadFile
- UploadTexture
- UploadScreenshot
