using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapPlacer : MonoBehaviour
{
    private GameObject toInstantiate;
    private List<GameObject> _objects = new List<GameObject>();
    private ARRaycastManager _arRaycastManager;
    public GameObject Menu;
    public GameObject EiffelTower;
    public GameObject Liberty;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool GetTouchPosition(out Vector2 tapPosition)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tapPosition = Input.GetTouch(0).position;
            return true;
        }

        tapPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HandleObjectPlacement())
            return;
        HandleSelection();
    }

    private bool HandleObjectPlacement()
    {
        if (!GetTouchPosition(out var tapPosition))
            return false;
        if (!_arRaycastManager.Raycast(tapPosition, hits, TrackableType.Planes))
            return false;

        var hitPose = hits[0].pose;

        if (!_objects.Any())
            toInstantiate = Menu;

        if (toInstantiate == null)
            return false;

        _objects.Add(Instantiate(toInstantiate, hitPose.position, Quaternion.identity));
        toInstantiate = null;
        return true;
    }

    private void HandleSelection()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.current.ScreenPointToRay(touch.position);
                var hasHit = Physics.Raycast(ray, out var hit);
                if (!hasHit)
                    return;
                if (hit.transform.CompareTag("menu_eiffel"))
                    toInstantiate = EiffelTower;
                if (hit.transform.CompareTag("menu_liberty"))
                    toInstantiate = Liberty;
                if (hit.transform.CompareTag("menu_clear"))
                {
                    foreach (var obj in _objects.Skip(1))
                        Destroy(obj);
                    var menu = _objects.First();
                    _objects.Clear();
                    _objects.Add(menu);
                }
            }
        }
    }
}
