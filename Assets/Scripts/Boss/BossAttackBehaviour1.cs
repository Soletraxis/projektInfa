using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour1 : StateMachineBehaviour
{
    [SerializeField]
    private EnemyController AI;
    public float attackDuration = 2.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.GetComponent<EnemyController>();
        AI.enemyBody.velocity = Vector2.zero;
        attackDuration = 2.0f;
        AI.isFightingJump = true;
        AI.chasedPlayerPosition = AI.playerPosition.position;
        AI.direction = AI.chasedPlayerPosition - AI.enemyBody.transform.position;
        AI.enemyBody.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, -AI.direction.x));
        //Jump and smash attack
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackDuration -= Time.deltaTime;
        if (attackDuration <= 0.0f)
        {
            animator.SetBool("Move" + AI.lastAttack, false);
            animator.SetBool("isIdle", true);
            AI.isFightingJump = false;
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
