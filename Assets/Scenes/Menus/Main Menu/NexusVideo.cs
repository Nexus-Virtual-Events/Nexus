using UnityEngine;
using UnityEngine.UI;
using TMPro;

using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.Playables;


// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class NexusVideo
{

    // instance of agora engine
    public IRtcEngine mRtcEngine;

    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");

        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);
        mRtcEngine.RegisterLocalUserAccount(appId, PlayerPrefs.GetString("playerName"));

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void join(string channel)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;

        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.EnableSoundPositionIndication(true);

        // join channel
        mRtcEngine.JoinChannelWithUserAccount(null, channel, PlayerPrefs.GetString("playerName"));
        //mRtcEngine.JoinChannelWithUserAccount(null, "Admin", PlayerPrefs.GetString("playerName"));
        //mRtcEngine.JoinChannel(channel, null, 0);

        // Optional: if a data stream is required, here is a good place to create it
        int streamID = mRtcEngine.CreateDataStream(true, true);
        Debug.Log("initializeEngine done, data stream id = " + streamID);
    }

    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        if (ver == "2.9.1.45")
        {
            ver = "2.9.2";  // A conversion for the current internal version#
        }
        else
        {
            if (ver == "2.9.1.46")
            {
                ver = "2.9.2.2";  // A conversion for the current internal version#
            }
        }
        return ver;
    }

    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }

    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }


    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo)
            {
                mRtcEngine.EnableVideo();
            }
            else
            {
                mRtcEngine.DisableVideo();
            }
        }
    }

    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onTheCircleLoaded()
    {
        // Attach the SDK Script VideoSurface for video rendering
        GameObject quad = GameObject.Find("Webcam");
        if (ReferenceEquals(quad, null))
        {
            Debug.Log("BBBB: failed to find Quad");
            return;
        }
        else
        {
            quad.AddComponent<VideoSurface>();
        }
    }

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        //GameObject textVersionGameObject = GameObject.Find("VersionText");
        //textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        UserInfo newUser = mRtcEngine.GetUserInfoByUid(uid);
        // this is called in main thread

        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        //VideoSurface videoSurface = makeImageSurface(newUser.userAccount);
        VideoSurface planeVideoSurface = makePlaneSurface(newUser.userAccount);

        //if (!ReferenceEquals(videoSurface, null))
        //{
        //    // configure videoSurface
        //    videoSurface.SetForUser(uid);
        //    videoSurface.SetEnable(true);
        //    videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        //    videoSurface.SetGameFps(30);
        //}

        if (!ReferenceEquals(planeVideoSurface, null))
        {
            // configure videoSurface
            planeVideoSurface.SetForUser(uid);
            planeVideoSurface.SetEnable(true);
            planeVideoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
            planeVideoSurface.SetGameFps(30);
        }
    }

    public VideoSurface makePlaneSurface(string goName)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        if (plane == null) return null;

        plane.name = goName;
        plane.transform.Rotate(90.0f, 0.0f, 0.0f);
        plane.transform.position = new Vector3(0.111f, 2.186f, 0.01f);
        plane.transform.localScale = new Vector3(0.1185186f, 1f, 0.06666667f);

        VideoSurface videoSurface = plane.AddComponent<VideoSurface>();
        return videoSurface;
    }

    private const float Offset = 100;
    public VideoSurface makeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;

        // to be renderered onto
        go.AddComponent<RawImage>();

        // make the object draggable
        go.AddComponent<UIElementDragger>();
        GameObject canvas = GameObject.Find("Agora Panel");
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
        }
        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        RectTransform videoRectTransform = go.GetComponent<RectTransform>();
        videoRectTransform.sizeDelta = new Vector2(320, 180);
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        // configure videoSurface
        VideoSurface videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }
    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        // this is called in main thread
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }
}
