using UnityEngine;

public class MoveAway : MonoBehaviour
{
    public GameObject player;
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(3f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 4f)
        {
            Vector3 directionToPlayer = transform.position - player.transform.position;
            directionToPlayer = directionToPlayer.normalized;
            directionToPlayer.y = 0;
            transform.position += speed * directionToPlayer * Time.deltaTime;
        }
    }
}
