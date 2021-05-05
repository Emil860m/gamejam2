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
    [SerializeField] private KeyCode catchButton = KeyCode.Space;
    PlayerData player = new PlayerData();
    private bool catchEnabled = false;
    private GameObject coughtPerson;
    private bool escortingPerson = false;

    private bool movePositiveY = true;
    private bool moveNegativeY = true;
    private bool movePositiveX = true;
    private bool moveNegativeX = true;

    // Start is called before the first frame update
    void Start()
    {
        AstarPath.active.Scan();
    }

    void VectorMovement()
    {
        direction = Vector2.zero;
        Transform characterTransform = gameObject.GetComponent<Transform>();
        if (Input.GetKey(left) && moveNegativeX)
            direction += new Vector2(-1, 0);
        if (Input.GetKey(right) && movePositiveX)
            direction += new Vector2(1, 0);
        if (Input.GetKey(up) && movePositiveY)
            direction += new Vector2(0, 1);
        if (Input.GetKey(down) && moveNegativeY)
            direction += new Vector2(0, -1);
        direction.Normalize();
        direction *= Time.deltaTime * speed;
        characterTransform.position += (Vector3)direction;
    }

    // Update is called once per frame
    void Update()
    {
        VectorMovement();
        Catch();
    }

    private void Catch()
    {
        if (catchEnabled && !escortingPerson)
        {
            if (Input.GetKeyDown(catchButton) && coughtPerson != null)
            {
                coughtPerson.GetComponent<Person>().Catch();
                coughtPerson.GetComponent<Person>().SetFollowObject(gameObject);
                escortingPerson = true;
            }
        }
        else if (escortingPerson)
        {

            if (Input.GetKeyDown(catchButton) && coughtPerson != null)
            {
                coughtPerson.GetComponent<Person>().LeaveInRoom();
                coughtPerson = null;
                escortingPerson = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Person") && !escortingPerson)
        {
            catchEnabled = true;
            coughtPerson = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                movePositiveX = false;
            else if (collision.gameObject.transform.position.x < transform.position.x)
                moveNegativeX = false;
        }
        if (collision.gameObject.CompareTag("Roof"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
                movePositiveY = false;
            else if (collision.gameObject.transform.position.y < transform.position.y)
                moveNegativeY = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Person") && !escortingPerson)
        {
            catchEnabled = false;
            coughtPerson = null;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                movePositiveX = true;
            else if (collision.gameObject.transform.position.x < transform.position.x)
                moveNegativeX = true;
        }
        if (collision.gameObject.CompareTag("Roof"))
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
                movePositiveY = true;
            else if (collision.gameObject.transform.position.y < transform.position.y)
                moveNegativeY = true;
        }
    }
}
