using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

    [SerializeField]
    private PlayerController player;

    public float cameraVelocity = 2.0f;

    public static CameraFollowScript followScriptInstance;


    private void Awake()
    {
        if (followScriptInstance == null)
        {
            followScriptInstance = this;
        }
        else if (followScriptInstance != this)
        {
            Destroy(gameObject);
            followScriptInstance.player = FindObjectOfType<PlayerController>();
        }

        DontDestroyOnLoad(gameObject);
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update () {
        CameraFollow();
	}

    private void CameraFollow()
    {
        Vector2 followVector = new Vector2(player.GetComponent<Transform>().position.x - transform.position.x, player.GetComponent<Transform>().position.y - transform.position.y);
        GetComponent<Rigidbody2D>().velocity = followVector * cameraVelocity;
    }
}
