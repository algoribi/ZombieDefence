using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool Fire()
    {
        return Input.GetMouseButton(0);
    }
    public bool CatalogUp()
    {
        return Input.GetAxis("Mouse ScrollWheel") > 0;
    }
    public bool CatalogDown()
    {
        return Input.GetAxis("Mouse ScrollWheel") < 0;
    }
    public bool MoveKey()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }
    public bool WeaponChangeKey(int num)
    {
        switch (num)
        {
            case 1: return Input.GetKeyDown(KeyCode.Alpha1);
            case 2: return Input.GetKeyDown(KeyCode.Alpha2);
            case 3: return Input.GetKeyDown(KeyCode.Alpha3);
        }
        return false;
    }
    public bool Reload()
    {
        return Input.GetKeyDown(KeyCode.R);
    }
    public bool BackToMenu()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }
}
