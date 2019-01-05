using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChasePlayerBehaviour : StateMachineBehaviour {


    [SerializeField]
    private EnemyController AI;

    public float chaseDuration = 3f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI = animator.GetComponent<EnemyController>();
        chaseDuration = 3f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chaseDuration -= Time.deltaTime;
        if(chaseDuration <= 0.0f)
        {
            animator.SetBool("Move" + AI.lastAttack, false);
            animator.SetBool("isIdle", true);
        }
        else if(chaseDuration > 0.0f)
        {
            AI.chasedPlayerPosition = AI.playerPosition.position;
            AI.direction = AI.chasedPlayerPosition - AI.transform.position;
            AI.transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, AI.direction.z));
            AI.enemyBody.velocity = new Vector2(AI.speed * (1.0f * Mathf.Abs(AI.direction.x) / AI.direction.x), AI.enemyBody.velocity.y);
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
