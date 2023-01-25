using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enviornment
{
    public GameObject prefab;
    public bool shown;
}

public class EnviornmentSwapController : MonoBehaviour
{
    [SerializeField] protected GameObject Parent = new GameObject();
    [SerializeField] protected List<Enviornment> Enviornments = new List<Enviornment>();
    [SerializeField] protected List<GameObject> ActiveObjects = new List<GameObject>();


    void Start()
    {
        foreach (var enviornment in Enviornments)
        {
            GameObject e = Instantiate(enviornment.prefab, new Vector3(0, 0, 0), Quaternion.identity);
            e.transform.SetParent(Parent.transform);
            if (!enviornment.shown)
            {
                e.SetActive(false);
            }
            ActiveObjects.Add(e);
        }
    }

    void Update()
    {
        for (int i = 0; i < Enviornments.Count; i++)
        {
            if (Enviornments[i].shown)
            {
                ActiveObjects[i].SetActive(true);
            }
            else
            {
                ActiveObjects[i].SetActive(false);
            }
        }
    }
}
