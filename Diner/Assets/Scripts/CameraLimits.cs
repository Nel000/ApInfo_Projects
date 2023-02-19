using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimits : MonoBehaviour
{
    [SerializeField] private Rect cameraLimits;
    public Rect CamLimits => cameraLimits;

    private float width, height;

    private void Start()
    {
        Camera camera = GetComponent<Camera>();

        float height = camera.orthographicSize;
        float width = height * camera.aspect;
    }

    public void DefineLimits(float x, float y, float w, float h)
    {
        cameraLimits = new Rect(x, y, w, h);
    }

    private void OnDrawGizmos()
    {
        Camera camera = GetComponent<Camera>();

        float height = camera.orthographicSize;
        float width = height * camera.aspect;
        
        Vector3 p1 = new Vector3(
            cameraLimits.xMin - width, cameraLimits.yMin - height, 0);
        Vector3 p2 = new Vector3(
            cameraLimits.xMax + width, cameraLimits.yMin - height, 0);
        Vector3 p3 = new Vector3(
            cameraLimits.xMax + width, cameraLimits.yMax + height, 0);
        Vector3 p4 = new Vector3(
            cameraLimits.xMin - width, cameraLimits.yMax + height, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
