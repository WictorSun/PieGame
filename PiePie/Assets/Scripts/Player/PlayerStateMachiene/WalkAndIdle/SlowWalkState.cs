using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Ground Movement/Slow Walk State")]
    public class SlowWalkState : State<CharacterCtrl>
    {
        // references to stateRunner and GameManager
        CharacterCtrl _parent;
        GameManager _GM;

        // Player Rigidbody
        private Rigidbody _playerRB;

        //Animation
        private Animator _playerAnim;
        int _moveAnimationID;
        int _moveWithBagID;

        //Transforms
        private Transform _orientation;
        private Transform _cameraPlayer;
        private Transform _thisObject;

        //Vectors
        private Vector3 movement;
        private Vector2 _inputVectorOnGround;

        //Floats
        private float _onGroundMoveForceSlow;
        private float _rotationSpeed;

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;

            _playerAnim = parent.PlayerAnimator;


            _playerRB = parent.PlayerRB;

            _orientation = parent.Orientation;
            _cameraPlayer = parent.CameraPlayer;
            _thisObject = parent.ThisObject;

            _onGroundMoveForceSlow = parent.OnGroundMoveForceSlow;
            _rotationSpeed = parent.RotationSpeed;
            _playerAnim.SetBool("OnBike", false);
        }

        public override void CaptureInput()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {
            _moveAnimationID = _parent.MoveAnimationID;
            _moveWithBagID = _parent.MoveWithBagID;
            _inputVectorOnGround = _parent.IH.InputVectorOnGround;
            //getting reference to GameManager and InputVectorOnGround from stateRunner
            _GM = _parent.GM;

            
                //Moving the player
                movement = new Vector3(_inputVectorOnGround.x, 0f, _inputVectorOnGround.y);
                Vector3 CameraDir = _orientation.transform.forward;
                movement = Vector3.Scale(movement, new Vector3(1, 0, 1)).normalized;
                movement = CameraDir * movement.z + _cameraPlayer.transform.right * movement.x;
                _playerRB.MovePosition((Vector3)_thisObject.transform.position + movement * _onGroundMoveForceSlow * Time.deltaTime);

                //Rotating the player
                Quaternion targetRotation = _thisObject.transform.rotation;
                _thisObject.transform.rotation = targetRotation;
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(0, targetAngle, 0);
                _thisObject.transform.rotation = Quaternion.Lerp(_thisObject.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            
            //Checks which Animation BlendTree to play
            if (!_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveAnimationID, .3f, .1f, Time.deltaTime);
            }
            else if (_GM.hasBag)
            {
                _playerAnim.SetFloat(_moveWithBagID, .3f, .1f, Time.deltaTime);
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
