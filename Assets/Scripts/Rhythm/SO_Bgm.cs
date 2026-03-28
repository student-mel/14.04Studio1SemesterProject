using UnityEngine;

[CreateAssetMenu(fileName = "ObjBGM_", menuName = "BGM Scriptable Object", order = 0)]
public class SO_Bgm : ScriptableObject
{
    public AudioClip clip;
    [Range(80, 180)]
    public int bpm; 
}
