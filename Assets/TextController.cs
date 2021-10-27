using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextController : MonoBehaviour
{

    public string Tag;
    public string[] Facts;
    public TextMeshPro Text;
    
    // Start is called before the first frame update
    void Start()
    {
        Text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        HandleObjectPlacement();
    }

    private void HandleObjectPlacement()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.current.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out var hit) && hit.transform.CompareTag(Tag))
                {
                    if (Text.text == "")
                    {
                        Text.text = Facts.First();
                        return;
                    }

                    var index = Facts.ToList().IndexOf(Text.text);
                    if (index == Facts.Length - 1)
                    {
                        Text.text = "";
                        return;
                    }
                    
                    Text.text = Facts[index + 1];
                }
            }
        }
    }
}
