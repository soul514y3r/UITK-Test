using UnityEngine;


[CreateAssetMenu(fileName = "BitpackedData", menuName = "Custom/Bitpacked Data")]
public class Bitpacked : ScriptableObject
{
    
    [BitpackInt(2)] public bool startflag;
        [BitpackInt(2)] public bool stopflag;
        [BitpackInt(2)] public bool stoflag;
        [BitpackInt(2)] public bool stflag;
        [BitpackInt(2)] public bool sflag;
        [BitpackInt(2)] public bool flag;
        [BitpackInt(2)] public bool lag;
        // [BitpackInt(2)] public bool ag;
        // [BitpackInt(2)] public bool g;
        [BitpackInt(2)] public byte start;

    [BitpackInt(1)] public bool straped;
    [BitpackInt(1)] public bool loaded;
    [BitpackInt(1)] public byte Ready;
}
