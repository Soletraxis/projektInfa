using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour {


    [SerializeField]
    private EnemyController AI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.GetComponent<EnemyController>();
        //stop movement
        AI.enemyBody.velocity = Vector2.zero;
        AI.idleFrameDuration = 1f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI.idleFrameDuration -= Time.deltaTime;
        //if player in range, chase
        AI.playerInChaseRange = Physics2D.CircleCast(AI.transform.position, AI.sightRange, Vector2.up, AI.sightRange, AI.playerLayer);
        if (AI.playerInChaseRange)
        {
            animator.SetBool("isChasing", true);
        }
        //if not, start patrolling
        if (AI.idleFrameDuration <= 0)
        {
            animator.SetBool("isPatrolling", true);
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
