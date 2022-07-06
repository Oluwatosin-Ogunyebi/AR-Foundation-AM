using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceVirtualObject : MonoBehaviour
{
    public GameObject gameObjectPrefab;

    private GameObject _gameObjectSpawned;
    private ARRaycastManager _aRRaycastManager;
    private Vector2 touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryToGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryToGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (_aRRaycastManager.Raycast(touchPosition,hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (_gameObjectSpawned == null)
            {
                _gameObjectSpawned = Instantiate(gameObjectPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                _gameObjectSpawned.transform.position = hitPose.position;
            }
        }
    }
}
