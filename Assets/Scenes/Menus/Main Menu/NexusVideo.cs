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
    public IRtcEngine broadcastRtcEngine;
    public bool isAdmin;

    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngines");

        if (broadcastRtcEngine != null)
        {
            Debug.Log("Engines exist. Please unload it first!");
            return;
        }

        mRtcEngine = IRtcEngine.GetEngine(appId);
        broadcastRtcEngine = IRtcEngine.GetEngine(appId);
        broadcastRtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_LIVE_BROADCASTING);

        mRtcEngine.RegisterLocalUserAccount(appId, PlayerPrefs.GetString("playerName"));
        broadcastRtcEngine.RegisterLocalUserAccount(appId, PlayerPrefs.GetString("playerName"));

        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
        broadcastRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void join(string channel)
    {
        isAdmin = channel == "admin";
        Debug.Log("calling join (channel = " + channel + ")");

        if (broadcastRtcEngine == null)
            return;

        if(!isAdmin)
        {
            mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
            mRtcEngine.OnUserJoined = onUserJoined;
            mRtcEngine.OnUserOffline = onUserOffline;
        }
        
        broadcastRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        broadcastRtcEngine.OnUserJoined = onUserJoined;
        broadcastRtcEngine.OnUserOffline = onUserOffline;

        // enable video
        if (!isAdmin)
        {
            broadcastRtcEngine.SetClientRole(CLIENT_ROLE.AUDIENCE);
            mRtcEngine.EnableVideo();
            mRtcEngine.EnableVideoObserver();
            mRtcEngine.EnableSoundPositionIndication(true);
        }
        else
        {
            broadcastRtcEngine.SetClientRole(CLIENT_ROLE.BROADCASTER);
            broadcastRtcEngine.EnableVideoObserver();
        }

        broadcastRtcEngine.EnableVideo();

        // join channel
        if(!isAdmin)
        {
            mRtcEngine.JoinChannelWithUserAccount(null, channel, PlayerPrefs.GetString("playerName"));
            int streamID = mRtcEngine.CreateDataStream(true, true);
            Debug.Log("initializeEngine done, data stream id = " + streamID);
        }
        
        broadcastRtcEngine.JoinChannelWithUserAccount(null, "Admin", PlayerPrefs.GetString("playerName"));
        int broadcastStreamID = broadcastRtcEngine.CreateDataStream(true, true);
        Debug.Log("initializeEngine done, data stream id = " + broadcastStreamID);

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

        if (broadcastRtcEngine == null)
            return;

        // leave channel
        if(!isAdmin) mRtcEngine.LeaveChannel();
        broadcastRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        if (!isAdmin) mRtcEngine.DisableVideoObserver();
        broadcastRtcEngine.DisableVideoObserver();
    }

    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (broadcastRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
            broadcastRtcEngine = null;
        }
    }

    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null || broadcastRtcEngine != null)
        {
            if (!pauseVideo)
            {
                if (!isAdmin) mRtcEngine.EnableVideo();
                broadcastRtcEngine.EnableVideo();
            }
            else
            {
                if (!isAdmin) mRtcEngine.DisableVideo();
                broadcastRtcEngine.DisableVideo();
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
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        UserInfo newUser = broadcastRtcEngine.GetUserInfoByUid(uid);
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
            planeVideoSurface.SetGameFps(60);
        }
    }

    public VideoSurface makePlaneSurface(string goName)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.GetComponent<MeshCollider>().enabled = false;

        if (plane == null) return null;

        plane.name = goName;
        plane.transform.Rotate(90.0f, 0.0f, 0.0f);
        plane.transform.position = new Vector3(0f, -10f, 0.01f);
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
