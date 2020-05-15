using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class StateSync : RealtimeComponent {

    // private MeshRenderer   _meshRenderer;
    private StateSyncModel _model;

    private void Start() {
        // Get a reference to the mesh renderer
        // _meshRenderer = GetComponent<MeshRenderer>();
    }

    private StateSyncModel model {
        set {
            if (_model != null) {
                // Unregister from events
                _model.stateDidChange -= StateDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null) {
                // Update the mesh render to match the new model
                UpdateModel();

                // Register for events so we'll know if the color changes later
                _model.stateDidChange += StateDidChange;
            }
        }
    }

    private void StateDidChange(StateSyncModel model, string value) {
        // Update the mesh renderer
        Debug.Log("state did change!");
        UpdateModel();
    }

    
    public GameObject diplomaPrefab;
    private GameObject diploma;

    private GameObject rightHand;
    private void GetDiploma()
    {
        if (rightHand == null)
        {
            rightHand = GetChildWithName(gameObject, "RightHand");
        }

        diploma = Instantiate(diplomaPrefab, rightHand.transform, false);
        diploma.transform.parent = rightHand.transform;
        diploma.transform.rotation = diploma.transform.rotation * Quaternion.Euler(0f, 0f, 90f);
        diploma.transform.Translate(new Vector3(0f, 0.1f, 0f), Space.Self);
        diploma.transform.rotation = diploma.transform.rotation * Quaternion.Euler(90f, 0f, 0f);
        diploma.transform.Translate(new Vector3(0f, -0.05f, 0f), Space.Self);

        Debug.Log(diploma);

        // Animator diplomaAnimator = diplomaUI.GetComponent<Animator>();
        // if(diplomaAnimator != null) diplomaAnimator.SetBool("open", true);
    }

    private void GiveDiploma()
    {
        Destroy(diploma);
    }
    private GameObject chest;
    private GameObject rosette;
    private void GetPinned(){
        chest = GetChildWithName(gameObject, "RightOuterBreast");
        rosette = Instantiate(diplomaPrefab, chest.transform, false);
        rosette.transform.parent = chest.transform;
        rosette.transform.rotation *= Quaternion.Euler(0f, 0f, 90f);
        rosette.transform.Translate(0,0.04f, 0);
    }

    private void Unpin(){
        if(rosette){
            Destroy(rosette);
        }
    }

    public GameObject GetChildWithName(GameObject fromGameObject, string withName)
    {
        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }

    public string GetState(){
        if(_model.state != ""){
            return _model.state;
        }
        else{
            Debug.Log("model.state missing");
            return "";
        }
    }

    

    private void UpdateModel() {
        // Get the color from the model and set it on the mesh renderer.
        // _meshRenderer.material.color = _model.color;
        if(_model.state == null || _model.state == ""){
            return;
        }

        string[] parameters = _model.state.Split('_');

        if(parameters[1] == "1"){
            Debug.Log("getdiploma from updatemodel");
            GetDiploma();
        }
        if(parameters[1] == "0"){
            GiveDiploma();
        }
        if(parameters[1] == "2"){
            Unpin();
            GetPinned();
        }
    }

    public void SetState(string state) {
        // Debug.Log("set state" + state);
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        _model.state = state;
    }
}