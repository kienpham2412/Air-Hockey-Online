using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class IceHockeyBallBehaviour : NetworkBehaviour
{
    private Vector3 forceDirection;
    private Vector3 currentPos;
    private Vector3 playerPos;
    private Vector3 startPos;
    private Rigidbody force;

    [SerializeField] private float strength = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        force = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RpcAddForce(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        switch (tag)
        {
            case "HostGoal":
                Goal(true);
                break;
            case "ClientGoal":
                Goal(false);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Add a force to this gameobject when it collides another gameobject
    /// </summary>
    /// <param name="gameObj">the another gameobject</param>
    [ClientRpc]
    private void RpcAddForce(GameObject gameObj)
    {
        currentPos = transform.position;
        playerPos = gameObj.transform.position;
        forceDirection = currentPos - playerPos;
        force.AddForce(forceDirection * strength, ForceMode.Impulse);
    }

    /// <summary>
    /// Add a score when the ball collide the goal
    /// </summary>
    /// <param name="isHost"> true if it's host's goal and false if it's client's goal </param>
    private void Goal(bool isHost)
    {
        if (isHost)
        {
            Debug.Log("Add a score to host !!!");
            transform.position = startPos;
        }
        else
        {
            Debug.Log("Add a score to client !!!");
            transform.position = startPos;
        }
        force.velocity = new Vector3(0, 0, 0);
        Referee.referee.RpcAddScore(isHost);
    }
}
