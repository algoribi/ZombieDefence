using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] Sounds = new AudioSource[8];

    void Start()
    {
        SetSoundVolume();
    }
    //사운드들의 볼륨을 설정에서 조정한 볼륨 크기로 조정해줌
    public void SetSoundVolume()
    {
        for (int idx = 0; idx < Sounds.Length; idx++)
        {
            Sounds[idx].volume = Manager.instance.volume_val;
        }
    }
}
