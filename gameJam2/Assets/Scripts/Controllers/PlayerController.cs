using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    [SerializeField] private float speed = 5; // units/s
    [SerializeField] private KeyCode left = KeyCode.LeftArrow;
    [SerializeField] private KeyCode right = KeyCode.RightArrow;
    [SerializeField] private KeyCode up = KeyCode.UpArrow;
    [SerializeField] private KeyCode down = KeyCode.DownArrow;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    void VectorMovement()
    {
        direction = Vector2.zero;
        Transform characterTransform = gameObject.GetComponent<Transform>();
        if (Input.GetKey(left))
            direction += new Vector2(-1, 0);
        if (Input.GetKey(right))
            direction += new Vector2(1, 0);
        if (Input.GetKey(up))
            direction += new Vector2(0, 1);
        if (Input.GetKey(down))
            direction += new Vector2(0, -1);
        direction.Normalize();
        direction *= Time.deltaTime * speed;
        characterTransform.position += (Vector3)direction;
    }

    // Update is called once per frame
    void Update()
    {
        VectorMovement();
    }
}
