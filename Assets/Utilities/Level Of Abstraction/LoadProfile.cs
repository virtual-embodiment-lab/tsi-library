//using UnityEngine;
//using UnityEditor;
//using UnityEngine.AddressableAssets;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEditor.AddressableAssets.Settings;

//public class LoadProfile : MonoBehaviour
//{
//    [SerializeField] private string profileGroupFilePath;

//    public void SelectProfile()
//    {
//        //string selectedFile = EditorUtility.OpenFilePanel("Select a profile", "", "");
//        //profileGroupFilePath = selectedFile;
//        //Debug.Log("Selected profile: " + selectedFile);

//        LoadProfileGroup();
//    }

//    public void LoadProfileGroup()
//    {
//        Addressables.LoadAssetAsync<AddressableAssetGroup>(profileGroupFilePath).Completed += OnProfileGroupLoaded;
//    }

//    private void OnProfileGroupLoaded(AsyncOperationHandle<AddressableAssetGroup> obj)
//    {
//        if (obj.Status == AsyncOperationStatus.Succeeded)
//        {
//            // Use the loaded profile group
//            AddressableAssetGroup profileGroup = obj.Result;
//            Debug.Log("Profile group loaded successfully");
//        }
//        else
//        {
//            Debug.LogError("Failed to load profile group: " + obj.OperationException.ToString());
//        }
//    }
//}

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadProfile : MonoBehaviour
{
    public string profileGroupFilePath;

    public void SelectProfile()
    {
        //string selectedFile = EditorUtility.OpenFilePanel("Select a profile", "", "");
        //profileGroupFilePath = selectedFile;
        //Debug.Log("Selected profile: " + selectedFile);

        LoadProfileGroup();
    }

    public void LoadProfileGroup()
    {
        Addressables.LoadAssetAsync<AddressableAssetGroup>(profileGroupFilePath).Completed += OnProfileGroupLoaded;
    }

    private void OnProfileGroupLoaded(AsyncOperationHandle<AddressableAssetGroup> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            // Use the loaded profile group
            AddressableAssetGroup profileGroup = obj.Result;
            Debug.Log("Profile group loaded successfully");
            // Instantiate each reference on the group
            foreach (AddressableAssetEntry entry in profileGroup.entries)
            {
                Addressables.InstantiateAsync(entry.AssetPath, Vector3.zero, Quaternion.identity).Completed += OnInstantiateCompleted;
            }
        }
        else
        {
            Debug.LogError("Failed to load profile group: " + obj.OperationException.ToString());
        }
    }

    private void OnInstantiateCompleted(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Asset instantiated successfully: " + obj.Result.name);
        }
        else
        {
            Debug.LogError("Failed to instantiate asset: " + obj.OperationException.ToString());
        }
    }
}
