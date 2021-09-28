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
    public static IceHockeyBallBehaviour behav;

    [SerializeField] private float strength = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        force = GetComponent<Rigidbody>();
        behav = GetComponent<IceHockeyBallBehaviour>();
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
                Goal(false);
                break;
            case "ClientGoal":
                Goal(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Add a force to this gameobject when it collides another gameobject
    /// </summary>
    /// <param name="gameObj">the another gameobject</param>
    [Server]
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
    [Server]
    private void Goal(bool isHost)
    {
        if (isHost)
        {
            Debug.Log("Add a score to host !!!");
        }
        else
        {
            Debug.Log("Add a score to client !!!");
        }
        RpcSetActive(false);
        Referee.referee.AddScore(isHost);
    }

    /// <summary>
    /// Hide the ball
    /// </summary>
    /// <param name="isActive">true if it's being shown and false if it's being hided</param>
    [ClientRpc]
    public void RpcSetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// Place the ball at start position
    /// </summary>
    [ClientRpc]
    public void RpcSetDefaultPos()
    {
        transform.position = startPos;
        force.velocity = new Vector3(0, 0, 0);
    }
}
