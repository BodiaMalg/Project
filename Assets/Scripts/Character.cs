using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// klasą z elementami dla gracza i elementów, które wpływają na gracza
/// </summary>
public class Character : Unit
{  
    
    /// <summary>
    /// Klasa tworzy hp dla bohatera
    /// </summary>
    [SerializeField]
    private int lives = 5;

    public int Lives
    {
        get { return lives; }
        set
        {
           if (value < 5) lives = value;
            livesBar.Refresh();
            if(lives < 1)
            SceneManager.LoadScene("Level");


        }
    }
    /// <summary>
    /// Klasa dodaje i odejmuje hp bohatera
    /// </summary>
    private LivesBar livesBar;
    
    /// <summary>
    /// klasa tworzenia siły prędkości
    /// </summary>
    [SerializeField]
    private float speed = 3.0F;
    

    /// <summary>
    /// klasa tworzenia siły skoku
    /// </summary>
   [SerializeField]
    private float jumpForce = 15.0F;

    /// <summary>
    /// kontrola dystansu obiektów do bohatera
    /// </summary>
    private bool isGrounded = false;

  
    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }



    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;
    private object levelManager;

    private void Awake()
    {
        livesBar = FindObjectOfType<LivesBar>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }
    /// <summary>
    /// ruch bohatera
    /// </summary>
    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (isGrounded) State = CharState.Run;
    }
    /// <summary>
    /// skoki bohatera
    /// </summary>
    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Szkoda bohatera
    /// </summary>
    public override void ReceiveDamage()
    {
        Lives--;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }
    /// <summary>
    ///  kontrola dystansu obiektów do bohatera
    /// </summary>
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
        }
    }
}


public enum CharState
{
    Idle,
    Run,
    Jump
}