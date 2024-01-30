using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;             
    [SerializeField]
    float minDistance = 10;  
    [SerializeField]
    float maxDistance = 30; 
    [SerializeField]
    float wheelSpeed = 500;
    [SerializeField]
    float xMoveSpeed = 500;
    [SerializeField]
    float yMoveSpeed = 250; 
    float yMinLimit = 5;        
    float yMaxLimit = 80;       
    float x = 0, y = 0;            
    float distance = 0;
    Vector3 cameraPos = Vector3.zero;    
    bool mouseActive = false;
    private void Awake()
    {
        cameraPos = transform.position;       
        distance = Vector3.Distance(cameraPos, target.position);       
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Cursor.visible = false;
    }
  
    private void Update()
    {
        if (target == null)
        { return; }
        if (PlayerManager.Instance.S_Stat.HP <= 0)
        { return; }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (mouseActive == false)
            { mouseActive = true; }
            else if (mouseActive == true)
            { mouseActive = false; }

            Cursor.visible = mouseActive;
        }
        if (Cursor.visible == false)
        {
            x += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
            y += Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            transform.rotation = Quaternion.Euler(y, x, 0);
            target.rotation = Quaternion.Euler(0, x, 0);
            distance -= Input.GetAxis("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;   
        cameraPos.z = -distance;
        transform.position = transform.rotation * cameraPos + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
