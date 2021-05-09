using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject PlayerUpgradeBG;
    [SerializeField] Text HPText;
    [SerializeField] Text SpeedText;
    [SerializeField] Text RecoverText;
    [SerializeField] Text RemainingPointText;
    [SerializeField] Text NoticePointText;

    public void JoinUpBg()
    {
        PlayerUpgradeBG.SetActive(true);
        RemainingPointText.text = "포인트 : " + StateManager.point;
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    public void ExitUpBg()
    {
        PlayerUpgradeBG.SetActive(false);
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }
    public void IncreaseHP()
    {
        if (StateManager.point > 0)
        {
            StateManager.playerHPMult += 0.1f;
            HPText.text = "체력 증가 " + Mathf.Round(StateManager.playerHPMult * 100) + "%";
            UsePoint();
        }
    }
    public void IncreaseSpeed()
    {
        if (StateManager.point > 0)
        {
            StateManager.playerSpeedMult += 0.05f;
            SpeedText.text = "속도 증가 " + Mathf.Round(StateManager.playerSpeedMult * 100) + "%";
            UsePoint();
        }
    }
    public void DownRecoverDelay()
    {
        if (StateManager.playerRecoverMinus < 2.9)
        {
            if (StateManager.point > 0)
            {
                StateManager.playerRecoverMinus += 0.075f;
                RecoverText.text = "회복 속도 " + (3 - StateManager.playerRecoverMinus) + "초";
                UsePoint();
            }
        }
    }
    void UsePoint()
    {
        StateManager.point--;
        RemainingPointText.text = "포인트 : " + StateManager.point;
        if (StateManager.point <= 0)
            NoticePointText.gameObject.SetActive(false);
        player.SetPlayerInfo();
        GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void G_BackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (GameObject.Find("InputManager").GetComponent<InputManager>().BackToMenu())
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
