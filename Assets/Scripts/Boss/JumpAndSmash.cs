using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndSmash : MonoBehaviour {

    public float jumpPower;

    [SerializeField]
    private EnemyController AI;

    [SerializeField]
    private Rigidbody2D bossBody;

    // Use this for initialization
    void Start () {
        AI = GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        attackMethod();
	}

    public void attackMethod()
    {
        if(AI.isFightingJump)
        {
            bossBody.AddForce(new Vector2((AI.playerPosition.position.x - transform.position.x)*0.5f, jumpPower));
        }
    }
}
