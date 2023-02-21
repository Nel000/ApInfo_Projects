using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] parts;

    private void Awake()
    {
        StartCoroutine(Build());
    }

    public IEnumerator Build()
    {
        int i = 0;

        for (int j = 0; j < parts.Length; j++)
            parts[j].enabled = false;

        while (i < parts.Length)
        {
            parts[i].enabled = true;
            i++;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
