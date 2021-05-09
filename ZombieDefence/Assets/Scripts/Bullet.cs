using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    float speed = 30;

    float delta = 0; //초세기
    float spanTime = 0.5f; //초간격

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        //초세기값이 간격타임을 넘으면 비활성화
        delta += Time.deltaTime;
        if (spanTime < delta)
        {
            gameObject.SetActive(false);
        }
    }
    //비활성화시 초기화
    void OnDisable()
    {
        delta = 0; //초세기값 초기화
        transform.position = new Vector3(0, 100, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //적 피격 실행
            collision.GetComponent<Zombie>().Hit(damage);
            gameObject.SetActive(false);
        }
    }
}
