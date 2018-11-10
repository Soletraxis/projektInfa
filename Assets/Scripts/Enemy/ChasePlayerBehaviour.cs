using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerBehaviour : StateMachineBehaviour {


    [SerializeField]
    private EnemyController AI;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.GetComponent<EnemyController>();

        AI.chasedPlayerPosition = AI.playerPosition.position;
        AI.direction = AI.chasedPlayerPosition - AI.transform.position;
        AI.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, AI.direction.z));
        AI.enemyBody.velocity = new Vector2(AI.speed * (1.0f * Mathf.Abs(AI.direction.x) / AI.direction.x), 0.0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI.groundLineCastPosition = AI.transform.position + Vector3.down / 2 * AI.enemyHeight + Vector3.right * Mathf.Sign(AI.direction.x) * AI.enemyWidth;
        AI.isGrounded = Physics2D.Linecast(AI.groundLineCastPosition, AI.groundLineCastPosition + Vector3.down, AI.groundLayer);

        AI.chaseAfterLosingPlayerSightFrameDuration -= Time.deltaTime;
        if (AI.isGrounded)
        {
            if (AI.playerInChaseRange)
            {
                //move towards player
                AI.chasedPlayerPosition = AI.playerPosition.position;
                AI.direction = AI.chasedPlayerPosition - AI.transform.position;
                AI.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, AI.direction.z));
                AI.enemyBody.velocity = new Vector2(AI.speed * (1.0f * Mathf.Abs(AI.direction.x) / AI.direction.x), AI.enemyBody.velocity.y);
            }
            else if (!AI.playerInChaseRange)
            {
                if (AI.sightOfPlayerLost == false)
                {
                    AI.chaseAfterLosingPlayerSightFrameDuration = 2f;
                    //move towards last known player position
                    AI.enemyBody.velocity = new Vector2(AI.speed * (1.0f * Mathf.Abs(AI.direction.x) / AI.direction.x), AI.enemyBody.velocity.y);
                }
                else if(AI.chaseAfterLosingPlayerSightFrameDuration > 0.0f && AI.sightOfPlayerLost == true)
                {
                    //move towards last known player position
                    AI.enemyBody.velocity = new Vector2(AI.speed * (1.0f * Mathf.Abs(AI.direction.x) / AI.direction.x), AI.enemyBody.velocity.y);
                }
                else if (AI.chaseAfterLosingPlayerSightFrameDuration <= 0.0f && AI.sightOfPlayerLost == true)
                {
                    AI.enemyBody.velocity = Vector2.zero;
                    animator.SetBool("isChasing", false);
                    animator.SetBool("isAttacking", false);
                }
            }
            //if player in range of attack, go to Attack
            if (AI.playerInAttackRange)
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            AI.enemyBody.velocity = Vector2.zero;
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttacking", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
