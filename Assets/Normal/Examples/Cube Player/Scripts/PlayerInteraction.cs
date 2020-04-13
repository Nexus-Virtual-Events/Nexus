﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerInteraction : MonoBehaviour
{

    public Transform interactionMenuPrefab;
    private Transform _interactionMenu;
    private bool _isInstantiated = false;
    private GameObject _interactedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
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
                if (hit.transform.tag == "Player" && !hit.transform.GetComponent<RealtimeView>().isOwnedLocally && _isInstantiated == false)
                {
                    _interactedPlayer = hit.transform.gameObject;
                    _interactionMenu = Instantiate(interactionMenuPrefab, hit.transform.position, hit.transform.rotation);
                    _interactionMenu.transform.SetParent(GameObject.Find("Player HUD").transform);
                    _isInstantiated = true;
                }
            }
        }

        if (_isInstantiated)
        {
            if (_interactionMenu == null)
                _isInstantiated = false;
            else
                _interactionMenu.transform.position = Camera.main.WorldToScreenPoint(_interactedPlayer.transform.position);
        }
    }
}
