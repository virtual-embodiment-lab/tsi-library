using System.Collections.Generic;
using UnityEngine;
using UtilityTypes;

public class AvatarCreator : Utility
{
    [SerializeField] protected float density = 1.0f;
    [SerializeField] protected Vector3 positionOffset = new Vector3(0, 0, 0);
    [SerializeField] protected Quaternion rotation = Quaternion.Euler(0, 0, 0);
    [SerializeField] protected GameObject parent = null;
    [SerializeField] private float currentDensity = 0;
    [SerializeField] protected bool createOne = false;

    [SerializeField] protected List<GameObject> sceneAvatars = new List<GameObject>();
    [SerializeField] protected List<GameObject> Chairs = new List<GameObject>();
    [SerializeField] protected List<GameObject> Avatar = new List<GameObject>();

    internal void Setup(AvatarCreatorData avatarCreatorData)
    {
        density = avatarCreatorData.density;
        positionOffset = avatarCreatorData.positionOffset;
        rotation = avatarCreatorData.rotation;
        parent = avatarCreatorData.parent;
        createOne = avatarCreatorData.createOne;
        Avatar = avatarCreatorData.Avatar;
    }

    public override void Setup(UtilityData utilityData)
    {
        base.Setup(utilityData);
    }

    void Start()
    {
        currentDensity = density;
        foreach (GameObject chair in GameObject.FindGameObjectsWithTag("PlayerPosition"))
        {
            Chairs.Add(chair);
        }

        if (createOne)
        {
            GameObject I = Instantiate(Avatar[0], Chairs[0].transform.position + positionOffset, new Quaternion(rotation.x, rotation.y, Chairs[0].transform.rotation.z, rotation.w));
            I.GetComponent<AvatarInstance>().Setup();
            sceneAvatars.Add(I);
            I.transform.SetParent(parent.transform);
        }
        else
        {
            foreach (GameObject chair in Chairs)
            {
                GameObject I = Instantiate(Avatar[0], chair.transform.position + positionOffset, rotation);
                I.GetComponent<AvatarInstance>().Setup();
                sceneAvatars.Add(I);
                I.transform.SetParent(parent.transform);
                if (Random.value >= density)
                {
                    I.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < sceneAvatars.Count; i++)
        {
            sceneAvatars[i].transform.position = Chairs[i].transform.position + positionOffset;
            sceneAvatars[i].transform.rotation = rotation;
        }

        if (currentDensity != density)
        {
            foreach (GameObject avatar in sceneAvatars)
            {
                if (Random.value >= density)
                {
                    avatar.SetActive(false);
                }
                else
                {
                    avatar.SetActive(true);
                }
            }
            currentDensity = density;
        }

    }
}
