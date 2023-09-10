using UnityEngine;
using Fusion.KCC;
using Fusion;

[OrderAfter(typeof(KCC))]
public class SimpleAnimator : NetworkBehaviour
{
    private NetworkMecanimAnimator netAnim;
    private KCC kcc;

    private Vector3 inputDirection;
    private bool isGrounded;
    private bool isJump;
    private bool isMoving;
    private bool isSprint;
    private bool dance1;
    private bool dance2;

    private static readonly float IDLE = 0f;
    private static readonly float WALK = 0.5f;
    private static readonly float RUN = 1f;
    private static readonly float DANCE = 2f;

    private static readonly int IS_GROUNDED_PARAM_HASH = Animator.StringToHash(RPMConstant.GROUNDED);
    private static readonly int SPEED_PARAM_HASH = Animator.StringToHash(RPMConstant.MOVESPEED);
    private static readonly int JUMP_PARAM_HASH = Animator.StringToHash(RPMConstant.JUMP);
    private static readonly int FALL_PARAM_HASH = Animator.StringToHash(RPMConstant.FALL);
    private static readonly int DANCE_PARAM_HASH = Animator.StringToHash("dance1");
    private static readonly int DANCE_PARAM_HASH2 = Animator.StringToHash("dance2");

    private void Awake()
    {
        if (TryGetComponent(out NetworkMecanimAnimator anim))
        {
            netAnim = anim;
        }
        else
        {
            Debug.LogError("Could not find NetworkMecanimAnimator component on object " + gameObject.name);
        }

        if (TryGetComponent(out KCC kccComponent))
        {
            kcc = kccComponent;
        }
        else
        {
            Debug.LogError("Could not find KCC component on object " + gameObject.name);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsForward || IsProxy)
            return;

        isGrounded = kcc.FixedData.IsGrounded;
        isJump = kcc.FixedData.HasJumped;
        isSprint = kcc.FixedData.Sprint;

        CheckKCC();
        AnimatorBridge();
    }

    private void CheckKCC()
    {
        KCCData fixedData = kcc.FixedData;

        inputDirection = fixedData.InputDirection;
        isMoving = !inputDirection.IsZero();

        isGrounded = fixedData.IsGrounded;
        isJump = fixedData.HasJumped;
        isSprint = fixedData.Sprint;

        dance1 = Input.GetKeyDown(KeyCode.Alpha1);
        dance2 = Input.GetKeyDown(KeyCode.Alpha2);
    }

    private void AnimatorBridge()
    {
        Animator animator = netAnim.Animator;

        animator.SetBool(IS_GROUNDED_PARAM_HASH, isGrounded);

        if (isJump)
        {
            animator.SetBool(JUMP_PARAM_HASH, true);
        }

        if (isGrounded)
        {
            animator.SetBool(JUMP_PARAM_HASH, false);
            animator.SetBool(FALL_PARAM_HASH, false);

            if (!isMoving)
            {
                if (dance1)
                {
                    animator.SetFloat(SPEED_PARAM_HASH, DANCE);
                    animator.SetBool(DANCE_PARAM_HASH, true);
                }
                else if (dance2) // Play the second dance animation
                {
                    animator.SetFloat(SPEED_PARAM_HASH, DANCE);
                    animator.SetBool(DANCE_PARAM_HASH2, true);
                }
                else
                {
                    animator.SetFloat(SPEED_PARAM_HASH, IDLE);
                    animator.SetBool(DANCE_PARAM_HASH, false);
                    animator.SetBool(DANCE_PARAM_HASH2, false);
                }
            }
            else if (isSprint)
            {
                animator.SetFloat(SPEED_PARAM_HASH, RUN);
                animator.SetBool(DANCE_PARAM_HASH, false);
                animator.SetBool(DANCE_PARAM_HASH2, false);
            }
            else
            {
                animator.SetFloat(SPEED_PARAM_HASH, WALK);
                animator.SetBool(DANCE_PARAM_HASH, false);
                animator.SetBool(DANCE_PARAM_HASH2, false);
            }
        }
        else if (!isJump)
        {
            animator.SetBool(FALL_PARAM_HASH, true);
            animator.SetBool(DANCE_PARAM_HASH, false);
            animator.SetBool(DANCE_PARAM_HASH2, false);
        }
    }
}
