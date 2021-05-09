using UnityEngine;

public class Muzzle : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("SetOff", 0.08f);
    }
    void SetOff()
    {
        gameObject.SetActive(false);
    }
}
