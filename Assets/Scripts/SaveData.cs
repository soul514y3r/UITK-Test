using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

[AttributeUsage(AttributeTargets.Field)]
public class BitpackInt : Attribute
{
public int Group;
public BitpackInt(int group) => Group = group;
}
public static class PackEvents
{
    public static event Action<string> OnPackFailed;
    public static void RaisePackFailed(string group) => OnPackFailed?.Invoke(group);
}
public class SaveData
{
    // run first to cache bools
    static readonly FieldInfo[] BoolFieldsCache = typeof(Bitpacked).GetFields().Where(f => f.FieldType == typeof(bool)).ToArray();
    static readonly FieldInfo[] ByteFieldsCache = typeof(Bitpacked).GetFields().Where(f => f.FieldType == typeof(byte)).ToArray();
    
    public FieldInfo[] boolFields(int group)
    {
        return BoolFieldsCache.Where(f => f.GetCustomAttribute<BitpackInt>()?.Group == group).ToArray();
    }
    public FieldInfo byteField(int group)
    {
        return (FieldInfo)ByteFieldsCache.FirstOrDefault(f => f.GetCustomAttribute<BitpackInt>()?.Group == group);
    }

    public static byte Package(params bool[] Bools)
    {
        byte Package = 0;

        for (int i = 0; i < Bools.Length; i++)
        if (Bools[i]) Package |= (byte)(1 << i);

        return Package;
    }

    public void Pack(Bitpacked targetInstance, int group)
    {
        FieldInfo[] boolForGroup = boolFields(group);
        FieldInfo byteForGroup = byteField(group);
    
    bool[] values = new bool[boolForGroup.Length];

    if(boolForGroup.Length > 8)
        {
        PackEvents.RaisePackFailed(byteForGroup.Name); 
        throw new InvalidOperationException($"Group {group} has {boolForGroup.Length} bools, max is 8.");

        }
    
    for (int i = 0; i < boolForGroup.Length; i++)
    {
        values[i] = (bool)boolForGroup[i].GetValue(targetInstance);
    }

    

    byte Byte = Package(values);

    byteForGroup.SetValue(targetInstance, Byte);

    }

    public void PackAll(Bitpacked targetInstance)
{
    var allGroups = BoolFieldsCache
        .Select(f => f.GetCustomAttribute<BitpackInt>()?.Group)
        .Where(g => g.HasValue)
        .Select(g => g.Value)
        .Distinct();

    foreach (int group in allGroups)
        Pack(targetInstance, group);
}

    public void Unpack(Bitpacked targetInstance, int group)
    {
        FieldInfo byteForGroup = byteField(group);
        FieldInfo[] boolForGroup = boolFields(group);

        byte packedByte = (byte)byteForGroup.GetValue(targetInstance);

        for (int i = 0; i < boolForGroup.Length; i++)
    {
        bool bitValue = (packedByte & (1 << i)) != 0;
        boolForGroup[i].SetValue(targetInstance, bitValue);
    }

     #if UNITY_EDITOR
        // This forces the Unity Engine to visually update the checkboxes in your Inspector 
        // and flags the ScriptableObject as changed so it saves correctly.
        UnityEditor.EditorUtility.SetDirty(targetInstance);
        #endif

    }
    public void UnpackAll(Bitpacked targetInstance)
{
    var allGroups = BoolFieldsCache
        .Select(f => f.GetCustomAttribute<BitpackInt>()?.Group)
        .Where(g => g.HasValue)
        .Select(g => g.Value)
        .Distinct();

    foreach (int group in allGroups)
        Unpack(targetInstance, group);
}

}
