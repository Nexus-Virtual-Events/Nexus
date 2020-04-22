using System;
public class EventDetails
{
    // timeUntil
    // scene
    private int timeUntil;
    private string scene;


    public EventDetails(int timeUntil, string scene)
    {
        this.timeUntil = timeUntil;
        this.scene = scene;
    }

    public string GetScene()
    {
        return scene;
    }

    public int GetTimeUntil()
    {
        return timeUntil;
    }

}
