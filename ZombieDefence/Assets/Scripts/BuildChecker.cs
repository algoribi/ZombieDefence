using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildChecker : MonoBehaviour
{
    BuildManager buildManager;
    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject removeButton;

    Building currentBuildingInfo;
    public Building getBuildingInfo { get => currentBuildingInfo; }

    public static bool isSelectedBuilding = true;
    public static bool isBuildState = true;
    void Start()
    {
        buildManager = GameObject.Find("BuildManager").GetComponent<BuildManager>();
        isSelectedBuilding = true;
        isBuildState = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!StateManager.isDay)
            return;
        //충돌한 객체가 건물이면 건설버튼 비활성화 후 제거버튼 활성화
        //현재 건물이 위치한 자리에 있는지 체크 (isBuildState)
        if (collision.CompareTag("Building"))
        {
            currentBuildingInfo = collision.gameObject.GetComponent<Building>();
            buildManager.GetRemoveTargetBuliding(collision.GetComponent<Building>());
            buildButton.SetActive(false);
            removeButton.SetActive(true);
            isBuildState = false;
        }
    }
    //충돌한 범위를 나가면 건설버튼 활성화 후 제거버튼 비활성화
    //현재 건물이 위치한 자리에 있는지 체크 (isBuildState)
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!StateManager.isDay)
            return;
        if (collision.CompareTag("Building"))
        {
            buildButton.SetActive(true);
            removeButton.SetActive(false);
            isBuildState = true;
            //건물을 제거했을 때 상점에서 선택된 아이템이 건물이 아니면
            //건설 버튼 비활성화해줌
            if (!isSelectedBuilding)
            {
                buildButton.SetActive(false);
            }
        }
    }
}
