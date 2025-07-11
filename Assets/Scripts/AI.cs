using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AI : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterController characterController;
    private GameManager GM;
    public float detectionDistance = 5f;
    public LayerMask attackTargetLayer;
    public LayerMask thrustTargetLayer;
    [Range(0f,1f)]
    public float AttackstupidNess;
    [Range(0f, 1f)]
    public float ThruststupidNess;
    private bool WillAttack = false;
    private bool WillThrust = false;
    [Tooltip("more attack time more stupidness")]
    public float attackDuration = 5f;
    public float timeBtwThrusts = 2f;
    private float detectDis;
    private float detectRad;
    public float radius = 1f; 
    private float distance = 1f;
    private bool isAbleToThrust;
    private bool canDetectPlayer = true;
    private bool canThrustPlayer = true;
    private bool isFacingTarget = false;
    private float currentAngle;
    private float tmpCurrentAngle;
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody2D>();
        detectDis = detectionDistance;
        detectRad = radius;
    }

    void Update()
    {
        if (GM.gameOn)
        {
            //calculate the rotations made
            float angle = characterController.rotateSpeed * Time.deltaTime;
            currentAngle += angle;
            if (currentAngle >= tmpCurrentAngle + 360f)
            {
                canDetectPlayer = true;
            }

            //attack behaviour
            if (Physics2D.Raycast(transform.position, transform.up, detectionDistance, attackTargetLayer) && canDetectPlayer)
            {
                isFacingTarget = true;
                tmpCurrentAngle = currentAngle;
            }
            //take desision if going to attack while facing target or not
            if (isFacingTarget)
            {
                canDetectPlayer = false;
                float x = Random.Range(0f, 1f);
                WillAttack = x <= AttackstupidNess;
                isFacingTarget = false;
            }
            if (WillAttack)
            {
                detectionDistance = 0;
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                characterController.Push(0);
                //this.transform.RotateAround(transform.position, new Vector3(0, 0, 1), rotateSpeed * Time.deltaTime);
            }

            IEnumerator AttackCoroutine()
            {
                characterController.Push(1);

                yield return new WaitForSeconds(attackDuration);
                WillAttack = false;
                detectionDistance = detectDis;
            }


            //thurst behaviour
            if (Physics2D.CircleCast(transform.position, radius, transform.right, distance, thrustTargetLayer) && canThrustPlayer)
            {
                isAbleToThrust = true;
            }
            else
            {
                characterController.Thrust(0);
                isAbleToThrust = false;
            }

            if (isAbleToThrust)
            {
                canThrustPlayer = false;
                StartCoroutine(waittime());
                float x = Random.Range(0f, 1f);
                WillThrust = x <= ThruststupidNess;
                isAbleToThrust = false;
            }

            if (WillThrust)
            {
                characterController.isPushing = false;
                characterController.Thrust(1);
                WillThrust = false;
            }
            else
            {
                characterController.Thrust(0);
            }
            IEnumerator waittime()
            {
                yield return new WaitForSeconds(timeBtwThrusts);
                canThrustPlayer = true;
            }
        }
        else
        {
            return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * detectionDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
