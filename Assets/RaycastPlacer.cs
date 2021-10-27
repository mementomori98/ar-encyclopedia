using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastPlacer : MonoBehaviour
{
    public GameObject Prefab;
    private ARRaycastManager rayManager;
    private GameObject placer;

    // Start is called before the first frame update
    void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f), hits, TrackableType.Planes);
        if (!hits.Any())
            return;
        
        placer ??= Instantiate(Prefab);

        var pos = hits[0].pose;
        placer.transform.position = pos.position;
        placer.transform.rotation = pos.rotation;
    }
}
