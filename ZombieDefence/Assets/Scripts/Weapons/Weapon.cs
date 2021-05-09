using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Animator gunFlash;
    [SerializeField] protected Transform startPos;
    protected float fireRate;
    protected float reloadSpeed;

    public void OnFlash()
    {
        gunFlash.gameObject.SetActive(true);
    }
    public void OffFlash()
    {
        gunFlash.gameObject.SetActive(false);
    }
}
public interface IWeapon
{
    void Attack();
    void OnFlash();
    void OffFlash();
}