using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{

    [CreateAssetMenu(menuName = "States/Player/Vehicle/Climb")]
    public class ClimbState : State<CharacterCtrl>
    {

        
        // references to stateRunner and GameManager
        CharacterCtrl _parent;
        GameManager _GM;


        //Generic Transforms
        private Transform _bikeOrientation;
        private Transform _cameraPlayer;
        private Transform _thisObject;

        //Rigidbody off the player
        private Rigidbody _playerRB;

        //Vectors
        private Vector2 _InputVectorOnClimb;
        

        // Floats
        [SerializeField] private float _climbspeed;
        [SerializeField] private float _wallDistanceOffset;
        [SerializeField] private float _sideRaycastOffset;

        // LayerMasks
        [SerializeField] private LayerMask _climbLayer;

        // Animator
        private Animator _playerAnim;

        // Bools
        private bool _cancleClimb;

        public override void Init(CharacterCtrl parent)
        {
            base.Init(parent);
            _parent = parent;
            
            _playerAnim = parent.PlayerAnimator;
            _playerRB = parent.PlayerRB;
            

            _bikeOrientation = parent.BikeOrientation;
            _cameraPlayer = parent.CameraPlayer;
            _thisObject = parent.ThisObject;

           
        }

        public override void CaptureInput()
        {

        }
        public override void Update()
        {

        }
        public override void FixedUpdate()
        {
            _InputVectorOnClimb = _parent.IH.InputVectorOnClimb;
            _cancleClimb = _parent.IH.CanleClimbing;

            _GM = _parent.GM;
            _playerAnim.SetBool("IsClimbing", true);

            float h = _InputVectorOnClimb.x;
            float v = _InputVectorOnClimb.y;
            Vector2 input = SquareToCircle(new Vector2(h, v));

            RaycastHit hit;
            if (Physics.Raycast(_thisObject.transform.position, _thisObject.transform.forward, out hit, 20, _climbLayer))
            {
                Debug.DrawRay(_thisObject.transform.position, _thisObject.transform.forward * hit.distance, Color.green); // Draw the forward raycast in green
                _thisObject.transform.forward = -hit.normal;
                float targetDistance = hit.distance - _wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
                _playerRB.position = Vector3.Lerp(_playerRB.position, hit.point + hit.normal * targetDistance, 30f * Time.fixedDeltaTime);
            }
            else
            {
                _GM.isReadyToClimb = false;
            }

            // Draw side raycasts to visualize wall detection
            Vector3 leftRaycastOrigin = _thisObject.transform.position + _thisObject.transform.right * -_sideRaycastOffset;
            Vector3 rightRaycastOrigin = _thisObject.transform.position + _thisObject.transform.right * _sideRaycastOffset;
            RaycastHit leftHit;
            RaycastHit rightHit;
            if (Physics.Raycast(leftRaycastOrigin, -_thisObject.transform.up, out leftHit, 0.3f, _climbLayer) && Physics.Raycast(rightRaycastOrigin, -_thisObject.transform.up, out rightHit, 0.3f, _climbLayer))
            {
                Debug.DrawRay(leftRaycastOrigin, -_thisObject.transform.up * leftHit.distance, Color.blue); // Draw the left side raycast in blue
                Debug.DrawRay(rightRaycastOrigin, -_thisObject.transform.up * rightHit.distance, Color.red); // Draw the right side raycast in red

                // Determine which wall to climb based on the side raycast hits
                if (leftHit.distance < rightHit.distance)
                {
                    // Climb on the left wall
                    float targetDistance = leftHit.distance - _wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
                    _playerRB.position = Vector3.Lerp(_playerRB.position, leftHit.point + leftHit.normal * targetDistance, 10f * Time.fixedDeltaTime);
                }
                else
                {
                    // Climb on the right wall
                    float targetDistance = rightHit.distance - _wallDistanceOffset; // Calculate the target distance from the hit point minus the offset
                    _playerRB.position = Vector3.Lerp(_playerRB.position, rightHit.point + rightHit.normal * targetDistance, 10f * Time.fixedDeltaTime);
                }
            }

            _playerRB.velocity = _thisObject.transform.TransformDirection(input) * _climbspeed;
            Vector2 SquareToCircle(Vector2 input)
            {

                return (input.sqrMagnitude >= 1f) ? input.normalized : input;
            }
        }
        public override void ChangeState()
        {
            if (_cancleClimb)
            {
                _runner.SetState(typeof(IdleState));
            }
        }
        public override void Exit()
        {
            _playerAnim.SetBool("IsClimbing", false);

        }
    }

}