using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera crashCam;

    [SerializeField]
    private Transform[] anchors;

    [SerializeField]
    private CrashController crashController;

    [SerializeField]
    private float RotationSmooth;

    private int PointIter1;
    private int PointIter2;

    private float CrashRelStartDist;
	private void Start ()
    {
        CrashRelStartDist = Vector3.Distance(transform.position, crashController.transform.position);
        crashCam = GetComponent<Camera>();
        if(anchors.Length > 2) // More than 2 Waypoints - Valid
        {
            PointIter1 = 0;
            PointIter2 = 1;
        }
        else
        {
            Debug.Log("Camera requires at least 2 points, a start and an end point");
        }
	}
	

	private void FixedUpdate ()
    {
        LookAtCrash();

        // Get Crash's Velocity
        Vector3 crashVel = crashController.GetComponent<CharacterController>().velocity;
        if (crashVel != Vector3.zero)
        {
            
            // Get Normalised Relative Position Between Anchors
            Vector3 anchorRelPos = (anchors[PointIter2].position - anchors[PointIter1].position).normalized;

            // Get Relative Velocity based on Relative Position between Anchors
            Vector2 relVelocity = new Vector2(crashVel.x * anchorRelPos.x, crashVel.z * anchorRelPos.z);

            // Ensure Distance between Crash and Camera is always the same
            float diff = 0f;
            float currentDist = Vector3.Distance(transform.position, crashController.transform.position);

            if (currentDist != CrashRelStartDist) // Camera isn't same distance
            {
                diff = (currentDist - CrashRelStartDist); // Get Difference
            }

            if (crashVel.z > 0) // Moving Forwards
            {
                MoveCamForwards(relVelocity, diff);
            }
            else if(crashVel.z < 0) // Moving Backwards
            {
                MoveCamBackwards(relVelocity, diff);
            }
            else if (crashVel.x != 0) // Moving Sideways
            {
                float relVelocityX = relVelocity.x;
                relVelocity.x = relVelocity.y;
                relVelocity.y = relVelocityX;
                float Anchor1RelX = crashController.transform.position.x - anchors[PointIter1].transform.position.x;
                float Anchor2RelX = crashController.transform.position.x - anchors[PointIter2].transform.position.x;

                if (Anchor1RelX == Anchor2RelX)
                {
                    return;
                }

                if (crashVel.x > 0) // Moving Right
                {
                    if(Anchor1RelX > 0 || Anchor2RelX > 0) // At least One Anchor is in the Direction we're moving
                    {
                        if(Anchor1RelX < Anchor2RelX) // Anchor1 Closer
                        {
                            MoveCamBackwards(relVelocity, diff);
                        }
                        else if(Anchor2RelX < Anchor1RelX) // Anchor2 Closer
                        {
                            MoveCamForwards(relVelocity, diff);
                        }
                    }
                }
                else if (crashVel.x < 0) // Moving Left
                {
                    if(Anchor1RelX < 0 || Anchor2RelX < 0) // At least one anchor is in the direction we're moving
                    {
                        if(Anchor1RelX > Anchor2RelX) // Anchor1 closer
                        {
                            MoveCamBackwards(relVelocity, diff);
                        }
                        else if (Anchor2RelX > Anchor1RelX) // Anchor2 closer
                        {
                            MoveCamForwards(relVelocity, diff);
                        }
                    }
                }
            }
        }
    }

    private void MoveCamForwards(Vector3 a_relVelocity, float a_diff)
    {
        float step = Mathf.Abs(a_relVelocity.magnitude + a_diff) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, anchors[PointIter2].position, step);
        if (PointIter2 + 1 != anchors.Length)
        {
            if (Vector3.Distance(transform.position, anchors[PointIter2].position) < float.Epsilon)
            {
                ++PointIter1;
                ++PointIter2;
            }
        }
    }
    
    private void MoveCamBackwards(Vector3 a_relVelocity, float a_diff)
    {
        float step = Mathf.Abs(a_relVelocity.magnitude - a_diff) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, anchors[PointIter1].transform.position, step);
        if (PointIter1 - 1 != -1)
        {
            if (Vector3.Distance(transform.position, anchors[PointIter1].transform.position) < float.Epsilon)
            {
                --PointIter1;
                --PointIter2;
            }
        }
    }

    private void LookAtCrash()
    {
        Vector3 CrashTop = crashController.transform.position;
        CrashTop.y += 0.5f * crashController.GetComponent<CharacterController>().height;
        Quaternion targetRotation = Quaternion.LookRotation(CrashTop - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSmooth * Time.deltaTime);
    }
}
