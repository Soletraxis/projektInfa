using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour {

    [SerializeField]
    private EnemyController AI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AI = animator.GetComponent<EnemyController>();

        AI.initialPatrolPosition.position = AI.transform.position;
        AI.enemyWidth = AI.GetComponent<SpriteRenderer>().bounds.extents.x;
        AI.enemyHeight = AI.GetComponent<SpriteRenderer>().bounds.extents.y;

        AI.patrolFrameDuration = 3f;

        AI.patrolRange = Random.Range(3.0f, 5.0f);
        AI.patrolDirection = Random.Range(-1.0f, 1.0f);
        AI.direction = new Vector3(0.0f, 0.0f, AI.patrolDirection);
        AI.enemyBody.transform.rotation = Quaternion.LookRotation(AI.direction);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AI.patrolFrameDuration -= Time.deltaTime;
        if(AI.patrolFrameDuration <= 0)
        {
            animator.SetBool("isPatrolling", false);
            AI.enemyBody.velocity = Vector2.zero;
        }
        //if player in range, start chasing
        if (AI.playerInChaseRange)
        {
            animator.SetBool("isPatrolling", false);
            animator.SetBool("isChasing", true);
        }
        //if not, move left and right, checking not to fall off ledges and not to walk into walls
        AI.groundLineCastPosition = AI.transform.position + Vector3.down / 2 * AI.enemyHeight + Vector3.right*Mathf.Sign(AI.patrolDirection) * AI.enemyWidth;
        AI.wallLineCastPosition = AI.transform.position + Vector3.right * Mathf.Sign(AI.patrolDirection) / 2 * AI.enemyWidth;
        AI.isGrounded = Physics2D.Linecast(AI.groundLineCastPosition, AI.groundLineCastPosition + Vector3.down, AI.groundLayer);
        AI.isFacingWall = Physics2D.Linecast(AI.wallLineCastPosition, AI.wallLineCastPosition + Vector3.right * Mathf.Sign(AI.patrolDirection), AI.wallLayer);
        if (AI.isGrounded == false || AI.isFacingWall == true || Mathf.Abs(AI.transform.position.x - AI.initialPatrolPosition.transform.position.x) >= AI.patrolRange)
        {
            AI.transform.Rotate(Vector3.up * -180);
            AI.patrolDirection = -AI.patrolDirection;
        }
        if (AI.isGrounded == true && AI.isFacingWall == false)
        {
            AI.patrolVect = new Vector2(AI.patrolDirection * AI.speed / 2.0f, AI.enemyBody.velocity.y);
            AI.enemyBody.velocity = AI.patrolVect;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
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
