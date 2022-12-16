using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LeoController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;

    float horizontal;
    float speed = 8f;
    float jumpPower = 6f;
    bool isFacingRight = true;
    public bool isAttack;
    public bool isRoll;

    void Update()
    {
        if(!isRoll)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if(!isFacingRight && horizontal > 0 && !isRoll || isFacingRight && horizontal < 0 && !isRoll)
            Flip();
        AnimatorValues();
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        if (context.canceled && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttack)
            isAttack = true;
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (context.performed && !isRoll && IsGrounded())
        {
            isRoll = true;
            rb.velocity = new Vector2(speed * transform.localScale.x * 2, jumpPower);
        }
    }

    void AnimatorValues()
    {
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("jump", isRoll ? false : !IsGrounded());
        animator.SetBool("attack", isAttack);
        animator.SetBool("roll", isRoll);
    }
}
