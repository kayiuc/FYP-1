using UnityEngine;
using CnControls;

namespace SwordWorld
{
    public class player_movement
        : MonoBehaviour
    {
        public float WalkSpeed = 8f;
        public float RunSpeed = 12f;
        public float TurnSmooth = 3.0f;
        public float JumpHeight = 5.0f;
        public float JumpCooldown = 1.0f;
        private bool IsJump;
        private bool IsWalk;
        private bool IsRun;
        private Vector3 movement;
        private Animator animator;
        private Rigidbody playerRigidbody;
        private Transform cameraTransform;
        private float h;
        private float v;
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject.transform);
            animator = GetComponent<Animator>();
            playerRigidbody = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            h = CnInputManager.GetAxis("Horizontal");
            v = CnInputManager.GetAxis("Vertical");
            IsJump = Input.GetButtonDown("Jump");
            IsWalk = Mathf.Abs(h) > 0.001 || Mathf.Abs(v) > 0.001;

            if (IsWalk)
            {
                if (IsRun)
                {
                    IsRun = !CnInputManager.GetButtonDown("Run");
                }
                else
                {
                    IsRun = CnInputManager.GetButtonDown("Run");
                }
            }
            else
            {
                IsRun = false;
            }
        }

        void FixedUpdate()
        {
            Move(h, v);

            Rotate(h, v);

            Jump(h, v);
        }

        void Move(float h, float v)
        {
            float speed = IsRun ? RunSpeed : WalkSpeed;


            movement.Set(h, 0.0f, v);

            movement = movement.normalized * speed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movement);

            {
                if (IsRun)
                {
                    animator.SetBool("IsRun", IsRun);
                }
                else
                {
                    animator.SetBool("IsRun", IsRun);
                    animator.SetBool("IsWalk", IsWalk);
                }
            }
        }

        void Jump(float h, float v)
        {
            if (IsJump)
            {
                animator.SetTrigger("Jump");
                playerRigidbody.velocity = new Vector3(0, JumpHeight, 0);
            }
        }

        Vector3 Rotate(float h, float v)
        {
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            Vector3 targetDirection;
            targetDirection = forward * v + right * h;

            if ((IsWalk && targetDirection != Vector3.zero))
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, TurnSmooth * Time.deltaTime);

                newRotation.x = 0f;
                newRotation.z = 0f;
                GetComponent<Rigidbody>().MoveRotation(newRotation);
            }

            return targetDirection;
        }
    }
}