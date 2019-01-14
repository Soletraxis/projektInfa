using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    //TO DO:
    //while attacking the enemy stops for a frame only

    [SerializeField]
    private EnemyController AI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.GetComponent<EnemyController>();
        AI.enemyBody.velocity = Vector2.zero;
        //Attack

        AI.chasedPlayerPosition = AI.playerPosition.position;
        AI.direction = AI.chasedPlayerPosition - AI.transform.position;
        AI.enemyBody.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, AI.direction.x));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if player in range of attacking, attack again
        if (AI.playerInAttackRange)
        {
            AI.enemyBody.velocity = Vector2.zero;
            //Attack
            Debug.Log("Attack");
            AI.isFighting = true;

            AI.chasedPlayerPosition = AI.playerPosition.position;
            AI.direction = AI.chasedPlayerPosition - AI.transform.position;
            AI.enemyBody.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, AI.direction.x));
        }
        //if player out of attack range, go to chasing
        else if (!AI.playerInAttackRange)
        {
            AI.isFighting = false;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isChasing", true);
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
