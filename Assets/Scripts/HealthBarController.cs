using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
    }

    public void CreateFloatingText(Transform location)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-.5f, .5f), location.position.y + Random.Range(-.5f, .5f)));

        gameObject.transform.SetParent(canvas.transform, false);
        gameObject.transform.position = screenPosition;
    }
}
