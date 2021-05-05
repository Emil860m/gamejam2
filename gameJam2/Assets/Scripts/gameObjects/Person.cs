using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class Person : MonoBehaviour
{

    private bool cought = false;
    private GameObject followObject;
    [SerializeField] private float happyness = 100;
    [SerializeField] private float maxHappyness = 100;
    [SerializeField] private float happynessLoss = 0.1f;
    private AIPath aIPath;

    public Slider happynessBar;

    private bool inRoom = false;
    public void SetFollowObject(GameObject gameObject)
    {
        followObject = gameObject;
        happynessBar.value = happyness;
    }
    

    public void Catch()
    {
        aIPath.enabled = false;
        cought = true;
        // attach position to player position
    }

    public void LeaveInRoom()
    {
        cought = false;
        followObject = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        aIPath = GetComponent<AIPath>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        happynessBar.value = CalculateHappyness();
        if (cought && followObject != null)
        {
            transform.position = Vector3.Lerp(transform.position, followObject.transform.position, 5 * Time.fixedDeltaTime);
        }
        if (inRoom && Random.Range(0, 1000) == 1)
        {
            aIPath.enabled = true;
        }
    }

    private float CalculateHappyness()
    {
        if (happyness <= 0)
        {
            happyness = 0;
            // Lose()
        }
        if (happyness > 0 && !inRoom)
            happyness -= happynessLoss;
        if (happyness > maxHappyness)
            happyness = maxHappyness;
        return happyness / maxHappyness;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Respawn"))
            Catch();
        if (collision.gameObject.CompareTag("Room"))
            inRoom = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
            inRoom = false;
    }

}
