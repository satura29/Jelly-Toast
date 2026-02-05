using UnityEngine;
using System;
 
public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;
    public Vector2 currentVelocity = Vector2.zero;
 
    public bool resetCamera = false;
    public float safeZone = 5f;

    //CAMERA BOUNDERIES
    public float minX = 0;
    public float maxX = 9999;
    public float minY = 0;
    public float maxY = 9999;

    void FixedUpdate()
    {
        Vector2 cameraPosition = transform.position;
        Vector2 playerPosition = player.position;
 
        // check our distance
        Vector2 difference = cameraPosition - playerPosition;
        float distance = difference.magnitude;
 
        if (distance > safeZone)
        {
            // set the camera to move
            resetCamera = true;
        }
        else if (distance < 0.1f)
        {
            // the camera has caught up
            resetCamera = false;
        }
         
        // process the movement
        if (resetCamera)
        {
            // perform the smooth damp
            Vector2 targetPos = Vector2.SmoothDamp(cameraPosition, playerPosition, ref currentVelocity, smoothTime);
 
            // move my camera into positoin
            Vector3 newCamPosition = targetPos;
            newCamPosition.z = transform.position.z;

            // clamping time!
            newCamPosition.x = Math.Clamp(newCamPosition.x, minX, maxX);
            newCamPosition.y = Math.Clamp(newCamPosition.y, minY, maxY);

            // move this object to this location
            transform.position = newCamPosition;
        }
    }
}

//using UnityEngine;

//public class CameraScript : MonoBehaviour
//{
//    public Transform player;
//    public float smoothTime = 1f;
//    public float currentVelocity = 0f;

//    void FixedUpdate()
//    {
//        Vector3 cameraPosition = transform.position;
//        // cameraPosition.x = player.position.x;
//        if (player.position.x > cameraPosition.x)
//        {
//            cameraPosition.x = Mathf.SmoothDamp(cameraPosition.x, player.position.x, ref currentVelocity, smoothTime);
//        }
//        else
//        {
//            currentVelocity = 0;
//        }
//        transform.position = cameraPosition;
//    }
//}