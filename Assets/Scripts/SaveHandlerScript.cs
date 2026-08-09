using Unity.VisualScripting;
using UnityEngine;

public class SaveHandlerScript : MonoBehaviour
{
    public static SaveHandlerScript instance;
    public Bitpacked packData; 
    private SaveData data = new SaveData();
    void Awake()
    {
        instance = this;
        
    }

    public void Save()
    {
        data.PackAll(packData);
        Debug.Log($"packed {packData.start.ToString()} into {packData.start}");
    }

    public void Load()
    {
        data.UnpackAll(packData);
    }

    [ContextMenu("Force Save Now")] public void ForceSave() => Save();
    [ContextMenu("Force Load Now")] public void ForceLoad() => Load();

}
