using UnityEngine;

[CreateAssetMenu(fileName = "RockBGM", menuName = "Rock BGM Scriptable Object")]
public class RockBGM : ScriptableObject
{
    public AudioClip clip;
    [Range(80, 180)]
    public int bpm;
}
