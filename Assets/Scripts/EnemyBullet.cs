using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    float speed;
    Vector2 direction;
    bool isReady;

    // Use this for setting variable values
    private void Awake() {
        speed = .08f;
        isReady = false;
    }


	// Use this for initialization
	void Start () {
		
	}

    public void SetDirection(Vector2 newDirection) {
        SpriteRenderer img = GetComponent<SpriteRenderer>();
        direction = newDirection.normalized;
        if (direction.x < 0) {
            img.flipX = true;
        }
        isReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady) {

            Vector2 position = transform.position;
            position += speed * direction * Time.deltaTime;

            transform.position = position;

            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y)) {
                Destroy(gameObject);
            }
        }
    }
}
