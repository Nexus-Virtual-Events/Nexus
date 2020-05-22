using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using UnityEngine.Android;

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class AgoraMainMenu : MonoBehaviour
{

    // Use this for initialization
    private ArrayList permissionList = new ArrayList();
    public static NexusVideo app = null;

    private string HomeSceneName = "Avatar Creator";

    private string PlaySceneName = "The Circle";

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [SerializeField]
    private string AppID = "896e38e8d51c4e8393b1827d6b9caa49";

    void Awake()
    {
		permissionList.Add(Permission.Microphone);         
		permissionList.Add(Permission.Camera);               

        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CheckAppId();
    }

    void Update()
    {
        CheckPermissions();
    }

    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
    }

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
        foreach(string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {                 
				Permission.RequestUserPermission(permission);
			}
        }
    }

    public void onJoinButtonClicked()
    {
        // get parameters (channel name, channel profile, etc.)

        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new NexusVideo(); // create app
            app.loadEngine(AppID); // load engine
        }

        // join channel and jump to next scene
        app.join(PlayerPrefs.GetString("roomName"));
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded
        SceneManager.LoadScene(PlaySceneName, LoadSceneMode.Single);
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app
            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == PlaySceneName)
        {
            if (!ReferenceEquals(app, null))
            {
                app.onTheCircleLoaded(); // call this after scene is loaded
            }
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }
}
