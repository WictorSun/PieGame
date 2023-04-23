using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumping : MonoBehaviour
{
    public PlayerMovementBigActionMap _PMBA;
    [SerializeField] private GameManager _GM;
    public InputHandler _IH { get; private set; }
    [SerializeField] public Rigidbody _playerRb;

    [SerializeField] Animator _playerAnim;
    [SerializeField] AmIGrounded _AIG;
    [SerializeField] IsFacingWall _IFW;

    [Header("Jumping")]
    public float _jumpForce = 10f;
    public float _maxJumpTime = 1f;
    public float _fallMultiplier = 2.5f;
    public float _jumpCooldown = 0.2f;
    public int _maxJumps = 2;
    private int _jumpCount = 0;
    private float _lastJumpTime = -Mathf.Infinity;
    public bool _isJumpingPressed;
    public bool _jump;

    public bool _isJumpingNow;

    [SerializeField] private float _jumpBufferTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _IH = GetComponent<InputHandler>();
        _AIG = GetComponent<AmIGrounded>();
        _IFW = GetComponent<IsFacingWall>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerAnim.SetBool("Jump", false);

        // Check if enough time has passed since last jump to allow a new jump
        if (Time.time - _lastJumpTime > _jumpCooldown && _IH.Jump && _jumpCount < _maxJumps && _AIG.IsGrounded() && !_IFW._isFacingClimbableWall() )
        {
            Jump();
            _isJumpingNow = true;
        }

        GravityScaling();
        if (_AIG.IsGrounded())
        {
            _isJumpingNow = false;
            _jumpCount = 0;
        }
    }

    private void Jump()
    {
        
        _jumpCount++;
        _playerAnim.SetBool("Jump", true);
        _lastJumpTime = Time.time;  // Update time of last jump
        _playerRb.velocity = new Vector3(_playerRb.velocity.x, 0f, _playerRb.velocity.z); // Remove any existing vertical velocity
        _playerRb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);  // apply the jump force
    }

    private void GravityScaling()
    {
        if (_playerRb.velocity.y < 0 && !_AIG.IsGrounded())
        {
            _playerRb.AddForce(Vector3.down * _fallMultiplier, ForceMode.Force);
        }
    }
}