using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 movement;

    private RuleManager ruleManager;
    public float normalSpeed = 5f;
    public float slowSpeed = 1.5f;

    public int Facing { get; private set; } = 1; // 1=desni -1=levo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        ruleManager = FindObjectOfType<RuleManager>();
    }

    void Update()
    {
        
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Reverse Controls
        if (ruleManager != null && ruleManager.IsRuleActive("Reverse Controls"))
        {
            movement.x *= -1;
            movement.y *= -1;
        }

        
        if (movement.x < 0) Facing = -1;
        else if (movement.x > 0) Facing = 1;

        
        bool isMoving = movement != Vector2.zero;
        animator.SetBool("isMoving", isMoving);

        if (movement.x < 0) sr.flipX = true;
        else if (movement.x > 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        float currentSpeed = normalSpeed;
        if (ruleManager != null && ruleManager.IsRuleActive("Slow Speed"))
        {
            currentSpeed = slowSpeed;
        }

        rb.MovePosition(rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    public bool IsStandingStill()
    {
        return movement == Vector2.zero;
    }
}