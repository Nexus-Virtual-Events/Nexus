using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerInteraction : MonoBehaviour
{

    // All Interaction Menus
    public Transform playerInteractionMenuPrefab;
    public Transform chairInteractionMenuPrefab;

    private Transform _interactionMenu;
    public bool isInstantiated = false;
    private GameObject _interactedObject;
    private float minMenuDist = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool IsCloseEnough(GameObject hit)
    {
        if (hit == null) {
            return false;
        }

        if (!hit.transform) {
            Debug.LogWarning("Hit transfrom doesn't exist.");
            return false;
        }

        if (!ActionRouter.GetLocalAvatar()) {
            Debug.LogWarning("Local Avatar doesn't exist.");
            return false;
        }

        return Vector3.Distance(hit.transform.position, ActionRouter.GetLocalAvatar().transform.position) < minMenuDist;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (IsCloseEnough(hit.transform.gameObject))
                {
                    if (hit.transform.tag == "Player" && !hit.transform.GetComponent<RealtimeView>().isOwnedLocally)
                    {
                        
                        if (isInstantiated && _interactedObject != hit.transform.gameObject)
                        {
                            Destroy(_interactionMenu.gameObject);
                            isInstantiated = false;
                        }

                        if (!isInstantiated)
                        {
                            _interactedObject = hit.transform.gameObject;
                            _interactionMenu = Instantiate(playerInteractionMenuPrefab);
                            _interactionMenu.transform.SetParent(GameObject.Find("Player HUD").transform);
                            isInstantiated = true;
                            ActionRouter.SetCurrentCharacter(hit.transform.gameObject);
                        }
                           
                    }
                    else if (hit.transform.tag == "Chair")
                    {
                        if (isInstantiated && _interactedObject != hit.transform.gameObject)
                        {
                            Destroy(_interactionMenu.gameObject);
                            isInstantiated = false;
                        }

                        if (!isInstantiated)
                        {
                            _interactedObject = hit.transform.gameObject;
                            _interactionMenu = Instantiate(chairInteractionMenuPrefab);
                            _interactionMenu.transform.SetParent(GameObject.Find("Player HUD").transform);
                            isInstantiated = true;
                            ActionRouter.SetCurrentChair(hit.transform.gameObject);
                        }
                    }
                }
            }
        }

        if (isInstantiated)
        {
            if (_interactionMenu == null || !IsCloseEnough(_interactedObject) || _interactedObject == null)
            {
                isInstantiated = false;
                Destroy(_interactionMenu.gameObject);
                _interactedObject = null;
            }
            else
                _interactionMenu.transform.position = Camera.main.WorldToScreenPoint(_interactedObject.transform.position);
        }
    }
}
