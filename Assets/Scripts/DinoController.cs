using System;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

[RequireComponent(typeof(Rigidbody))]
public class DinoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 60f;
    public float DriftAngle = 3f;
    public float Decelerate = 5f;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Animator anim;
    public PhotonView view;


    public static int start = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Disable physics-driven rotation
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        
          if (view.IsMine)
          {
              Camera.main.GetComponent<CameraController>().target = transform;
              if (start == 1)
              {
                  // 움직임 관리
                  horizontalInput = Input.GetAxis("Horizontal");
                  verticalInput = Input.GetAxis("Vertical");

                  if (Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)))
                  {
                      // 좌우를 반전시킴
                      rotationSpeed = -rotationSpeed;
                  }

                  if (Input.GetKeyUp(KeyCode.S) || (Input.GetKeyUp(KeyCode.DownArrow)))
                  {
                      // 좌우를 반전시킨것 되돌림
                      rotationSpeed = -rotationSpeed;
                  }

                  if (Input.GetKeyDown(KeyCode.LeftShift))
                  {
                      // 드리프트 각도 조정
                      rotationSpeed = DriftAngle * rotationSpeed;
                      // 드리프트 감속
                      moveSpeed = moveSpeed - Decelerate;
                  }

                  if (Input.GetKeyUp(KeyCode.LeftShift))
                  {
                      // 드리프트 각도 복구
                      rotationSpeed = 1 / DriftAngle * rotationSpeed;
                      // 드리프트 감속 복구
                      moveSpeed = moveSpeed + Decelerate;
                  }

                  //키 입력시 애니매이션 재생
                  if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                  {
                      anim.SetBool("isRunning", true);
                  }
                  else
                  {
                      anim.SetBool("isRunning", false);
                  }
              }


          }  
        
        
    }

    private void FixedUpdate()
    {

        // 이동거리 계산
        Vector3 movementDirection = transform.forward * verticalInput;
        rb.velocity = movementDirection * moveSpeed;

        // 회전각도 계산
        Quaternion rotation = Quaternion.Euler(0, horizontalInput * rotationSpeed * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rotation);
    }
}