using System.Collections;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CameraLimits camLimits;

    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector2 speed = Vector2.one;
    [SerializeField] private Rect cameraLimits;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();

        cameraLimits = camLimits.CamLimits;

        //target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 newPos;
            
            newPos.x = target.position.x + offset.x;
            newPos.y = target.position.y + (target.position.y * offset.y);    
            newPos.z = transform.position.z;

            Vector3 delta = newPos - transform.position;

            newPos.x = transform.position.x + delta.x * Time.deltaTime / speed.x;
            newPos.y = transform.position.y + delta.y * Time.deltaTime / speed.y;

            if (newPos.x > cameraLimits.xMax) newPos.x = cameraLimits.xMax;
            else if (newPos.x < cameraLimits.xMin) newPos.x = cameraLimits.xMin;

            if (newPos.y > cameraLimits.yMax) newPos.y = cameraLimits.yMax;
            else if (newPos.y < cameraLimits.yMin) newPos.y = cameraLimits.yMin;

            transform.position = newPos;
        }
    }

    /*private void OnDrawGizmos()
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
    }*/
}
