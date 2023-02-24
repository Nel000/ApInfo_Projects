using System.Collections;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    private GameManager gm;
    private Table table;

    [SerializeField] private SpriteRenderer[] parts;

    [SerializeField] private bool isBuilt, isTable;

    private void Awake()
    {
        gm = null;
        table = null;
        
        if (isTable)
        {
            gm = FindObjectOfType<GameManager>();
            table = GetComponent<Table>();
        }
    }

    public void StartBuild() => StartCoroutine(Build(gm));

    public IEnumerator Build(GameManager gm = null)
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

        if (isTable)
        {
            table.enabled = true;
            gm.OfficialTable();
        }
    }
}
