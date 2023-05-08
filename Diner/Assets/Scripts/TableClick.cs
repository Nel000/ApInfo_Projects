using UnityEngine;

public class TableClick : MonoBehaviour
{
    [SerializeField] private Component obj;

    private void Start()
    {
        obj = GetComponentInParent(typeof(IClickableObject));
    }

    private void OnMouseUpAsButton()
    {
        ((IClickableObject)obj).Click();
    }
}
