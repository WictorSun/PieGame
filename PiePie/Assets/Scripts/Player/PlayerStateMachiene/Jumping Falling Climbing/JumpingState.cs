using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Jumping")]
    public class JumpingState : State<CharacterCtrl>
    {
        CharacterCtrl _parent;
        GameManager _GM;
        AmIGrounded _AIG;

        //Player Rigidbody
        private Rigidbody _playerRB;

        //Animation
        private Animator _playerAnim;
        

        //Transforms
      

        //Bool
        private bool _jump;
        private bool _isGrounded;
     

        //Floats
        private float _jumpTimer;
        private float _jumpForce;
        private float _maxJumpTime;
        private float _fallMultiplier;


        //ints
        private int _jumpCount;
        private int _maxJumps;


        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _playerAnim = parent.PlayerAnimator;

            _parent = parent;

            _playerRB = parent.PlayerRB;

            _isGrounded = parent.ISGrounded;

            _jumpTimer = parent.JumpTimer;

            _jumpForce = parent.JumpForce;

            _maxJumpTime = parent.MaxJumpTime;

            _fallMultiplier = parent.FallMultiplier;

            _jumpCount = parent.JumpCount;
            _maxJumps = parent.MaxJumps;

            _playerAnim.SetBool("Jump", true);
            _jumpTimer = 0f;  // reset the jump timer
            _jumpCount++;
            _playerRB.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);  // apply the initial jump force

        }
        
        public override void CaptureInput()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {
            _isGrounded = _parent.AIG.Grounded;
            _jump = _parent.IH.Jump;
            Debug.Log(_isGrounded);
            //Debug.Log(_parent.IH.jump);
            _GM = _parent.GM;
            // Check if the jump button is pressed and the Rigidbody is on the ground





            if (_jump && _jumpCount < _maxJumps && _isGrounded)
            {
                _jumpTimer = 0f;  // reset the jump timer
                _jumpCount++;
                _playerRB.velocity = new Vector3(_playerRB.velocity.x, 0f, _playerRB.velocity.z);
                _playerRB.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);  // apply the initial jump force
                _playerAnim.SetBool("Isgrounded", false);
            }

            // Increase the jump timer if the jump button is held down and the maximum jump time has not been reached
            if (_jump && _jumpTimer < _maxJumpTime && _isGrounded)
            {
                _jumpTimer += Time.deltaTime;
            }

            // Reduce the jump force as the jump timer goes up
            float jumpMultiplier = 1f - (_jumpTimer / _maxJumpTime);
            jumpMultiplier = Mathf.Clamp01(jumpMultiplier);  // make sure the jump multiplier is between 0 and 1
            Vector3 jumpVelocity = _playerRB.velocity;
            jumpVelocity.y *= jumpMultiplier;
            _playerRB.velocity = jumpVelocity;

            // Increase the falling speed if the Rigidbody's velocity is zero
            if (!_isGrounded)
            {
                _jump = false;

                _playerRB.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
            }
        }
        public override void ChangeState()
        {

        }
        public override void Exit()
        {

        }
       
    }

}