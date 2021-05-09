using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject OptionBG;
    [SerializeField] SoundManager soundManager;

    void Awake()
    {
        volumeSlider.value = Manager.instance.volume_val;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    //게임시작
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    //환경설정 활성화 또는 비활성화
    public void SetOption(bool isOn)
    {
        OptionBG.SetActive(isOn);
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    //볼륨 조정 슬라이드
    public void SetVolumeSize()
    {
        Manager.instance.volume_val = volumeSlider.value;
        soundManager.SetSoundVolume();
    }
    //전체화면 또는 창모드
    public void SetScreenMode(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    //게임종료
    public void QuitGame()
    {
        Application.Quit();
    }
}
