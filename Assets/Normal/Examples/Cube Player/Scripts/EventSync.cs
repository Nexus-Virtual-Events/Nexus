using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class EventSync : RealtimeComponent
{

    public RealtimeEventsScript realtimeAdmin;

    private EventSyncModel _model;

    private EventSyncModel model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.eventsDidChange -= EventsDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                UpdateName();

                // Register for events so we'll know if the color changes later
                _model.eventsDidChange += EventsDidChange;
            }
        }
    }

    private void EventsDidChange(EventSyncModel model, string value)
    {
        // Update the mesh renderer
        UpdateName();
    }

    private void UpdateName()
    {
        // Get the color from the model and set it on the mesh renderer.
        _playerNameText.text = _model.events;
    }

    public void SetEvent(string eventString)
    {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.events = eventString;
    }
}
