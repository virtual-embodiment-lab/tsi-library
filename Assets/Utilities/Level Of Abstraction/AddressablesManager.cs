using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

[Flags]
public enum abstractionLevel
{
    Family,
    Friends,
    Strangers
}

public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid)
        : base(guid) { }
}

public class AddressablesManager : MonoBehaviour
{
    [SerializeField]
    private AssetReference playerArmatureAssetReference;

    [SerializeField]
    private GameObject playerController;

    private void Start()
    {
        Addressables.InitializeAsync().Completed += AddressablesManager_Compleated;
        ;
    }

    private void AddressablesManager_Compleated(AsyncOperationHandle<IResourceLocator> obj)
    {
        playerArmatureAssetReference.InstantiateAsync().Completed += (OnPlayerLoadDone) =>
        {
            playerController = OnPlayerLoadDone.Result;
        };
    }

    private void OnDestroy()
    {
        Addressables.ReleaseInstance(playerController);
    }
}
