using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CrashController : MonoBehaviour
{
    [SerializeField]
    private float Acceleration;
    [SerializeField]
    private float Drag;
    [SerializeField]
    private float MaxMovementSpeed;

    [SerializeField]
    private float GravityMultiplier;

    [SerializeField]
    private float JumpForce;

    [SerializeField]
    private float BounceVelocityThreshold;

    [SerializeField]
    private float BounceCheckVelocityMultiplier;

    [SerializeField]
    private float BounceJumpMultiplier;

    [SerializeField]
    private Vector3 Velocity;

    [SerializeField]
    private CrashUI crashUI;

    [SerializeField]
    private GameObject graphics;

    private CharacterController controller;

    private GameManager manager;

    private bool previouslyGrounded;
    private bool JumpNow;
    private bool isJumping;

    private const string CRATETAG = "Crate";

	private void Start ()
    {
        if(GameManager.instance != null)
        {
            manager = GameManager.instance;
        }
        else
        {
            Debug.LogError("No Game Manger Found in Scene");
        }

        if(crashUI == null)
        {
            Debug.LogError("No crashUI assigned, please assign in the inspector");
        }
        else
        {
            crashUI.SetWumpaCountText(manager.CrashWumpas);
            crashUI.SetLifeCountText(manager.CrashLives);
        }

        if(graphics == null)
        {
            Debug.LogError("No Graphics assigned, please assign in the inspector.");
        }

        isJumping = false;
        Velocity = Vector3.zero;
        controller = GetComponent<CharacterController>();
	}
	
	private void Update ()
    {
        JumpNow = Input.GetButton("Jump");

        if(!previouslyGrounded && controller.isGrounded)
        {
            isJumping = false;
            Velocity.y = 0f;
        }

        if(!controller.isGrounded && !isJumping && previouslyGrounded)
        {
            Velocity.y = 0f;
        }

        previouslyGrounded = controller.isGrounded;
	}

    private void FixedUpdate()
    {
        Vector3 Input = GetInput();

        CalculateVelocity(Input);
        CalculateRotation(Input);

        if(isJumping ||
           Velocity.y < BounceVelocityThreshold ||
           Velocity.y > BounceVelocityThreshold) // Player is jumping or falling
        {
            // Check for Crate Collisions
            if(Velocity.y > 0)
            {
                CheckBounceCollision(Vector3.up);
            }
            else if(Velocity.y < 0)
            {
                CheckBounceCollision(Vector3.down);
            }
        }

        CalculateGravity();

        controller.Move(Velocity * Time.fixedDeltaTime);
    }

    private Vector3 GetInput()
    {
        Vector3 InputVec = Vector2.zero;
        InputVec.x = Input.GetAxisRaw("Horizontal");
        InputVec.z = Input.GetAxisRaw("Vertical");
        return InputVec;
    }

    private void CalculateVelocity(Vector3 a_Input)
    {
        // Apply Acceleration or Drag to Horizontal Input
        if (a_Input.x > 0)
        {
            Velocity.x += Acceleration;
        }
        else if (a_Input.x < 0)
        {
            Velocity.x += -Acceleration;
        }
        else
        {
            if (Velocity.x < float.Epsilon && Velocity.x > 0 ||
               Velocity.x < -float.Epsilon && Velocity.x < 0)
            {
                Velocity.x = 0;
            }
            else if (Velocity.x > 0)
            {
                Velocity.x += -Drag;
            }
            else if (Velocity.x < 0)
            {
                Velocity.x += Drag;
            }
        }

        // Apply Acceleration or Drag to Vertical Input
        if (a_Input.z > 0)
        {
            Velocity.z += Acceleration;
        }
        else if (a_Input.z < 0)
        {
            Velocity.z += -Acceleration;
        }
        else
        {
            if (Velocity.z < float.Epsilon && Velocity.z > 0 ||
               Velocity.z < -float.Epsilon && Velocity.z < 0)
            {
                Velocity.z = 0;
            }
            else if (Velocity.z > 0)
            {
                Velocity.z += -Drag;
            }
            else if (Velocity.z < 0)
            {
                Velocity.z += Drag;
            }
        }

        // Cap Velocity with Max Movement Speed
        if (Mathf.Abs(Velocity.x) > MaxMovementSpeed)
        {
            if (Velocity.x > float.Epsilon)
            {
                Velocity.x = MaxMovementSpeed;
            }
            else if (Velocity.x < float.Epsilon)
            {
                Velocity.x = -MaxMovementSpeed;
            }
        }

        if (Mathf.Abs(Velocity.z) > MaxMovementSpeed)
        {
            if (Velocity.z > float.Epsilon)
            {
                Velocity.z = MaxMovementSpeed;
            }
            else if (Velocity.z < float.Epsilon)
            {
                Velocity.z = -MaxMovementSpeed;
            }
        }
    }

    private void CalculateRotation(Vector3 a_Input)
    {
        if (a_Input != Vector3.zero)
        {
            Quaternion newRot = Quaternion.LookRotation(a_Input);
            graphics.transform.rotation = Quaternion.Lerp(graphics.transform.rotation, newRot, 0.25f);
        }
    }

    private void CalculateGravity()
    {
        // Apply Gravity if not Grounded
        if (controller.isGrounded)
        {
            Velocity.y = 0;

            if (JumpNow && !isJumping)
            {
                Velocity.y = JumpForce;
                isJumping = true;
            }
        }
        else
        {
            Velocity += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
        }
    }

    private void CheckBounceCollision(Vector3 a_Direction)
    {
        RaycastHit hitInfo;
        Vector3 crashPos = transform.position;
        float crashYVel = -Velocity.y;

        if(a_Direction == Vector3.up)
        {
            crashPos.y = crashPos.y + controller.height;
            crashYVel = Velocity.y;
        }

        Debug.DrawLine(crashPos, crashPos + a_Direction * BounceCheckVelocityMultiplier * crashYVel);
        // Check Below
        if(Physics.SphereCast(crashPos, controller.radius, a_Direction, out hitInfo, BounceCheckVelocityMultiplier * crashYVel))
        {
            if(hitInfo.collider.tag == CRATETAG)
            {
                int BounceForce = 0;
                bool Wumpa = false;
                bool Life = false;
                // Get Crate Stats
                hitInfo.collider.GetComponent<Crate>().OnBounce(out BounceForce, out Wumpa, out Life);

                if(JumpNow && !controller.isGrounded && !previouslyGrounded) 
                {
                    Velocity.y = (BounceJumpMultiplier * BounceForce * -a_Direction.y);
                    isJumping = true;
                }
                else
                {
                    Velocity.y = BounceForce * -a_Direction.y;
                }

                if(Wumpa)
                {
                    manager.IncreaseWumpaCount();
                    crashUI.SetWumpaCountText(manager.CrashWumpas);
                    crashUI.ShowWumpaCountTimed(crashUI.DefaultCountShowTime);
                }
                else if(Life)
                {
                    manager.IncreaseLifeCount();
                    crashUI.SetLifeCountText(manager.CrashLives);
                    crashUI.ShowLifeCountTimed(crashUI.DefaultCountShowTime);
                }
            }
        }
    }
}
