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
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        force = GetComponent<Rigidbody>();
        behav = GetComponent<IceHockeyBallBehaviour>();
        startPos = gameObject.transform.position;
    }
    
    /// <summary>
    /// The event when this gameobject collides with another gameobject
    /// </summary>
    /// <param name="collision">The another gameobject that is collided by this gameobject</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RpcAddForce(collision.gameObject);
        }
        PlayerController.player.RpcPlaySFX("Gameplay");
    }

    /// <summary>
    /// The event when this gameobject collides with the trigger gameobject
    /// </summary>
    /// <param name="other">The trigger gameobject that is collides by this gameobject</param>
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
        PlayerController.player.RpcPlaySFX("Goal");
    }

    /// <summary>
    /// Add a force to this gameobject when it collides another gameobject
    /// </summary>
    /// <param name="gameObj">The another gameobject</param>
    [Server]
    private void RpcAddForce(GameObject gameObj)
    {
        currentPos = transform.position;
        playerPos = gameObj.transform.position;
        forceDirection = currentPos - playerPos;
        force.AddForce(forceDirection * strength, ForceMode.Impulse);
    }

    /// <summary>
    /// Add a score when the ball collides a goal
    /// </summary>
    /// <param name="isHost">True if it's host's goal and false if it's client's goal </param>
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
    /// <param name="isActive">True if it's being shown and false if it's being hided</param>
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
