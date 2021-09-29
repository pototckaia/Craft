using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpSpeed;
    public float Gravity;
    public float CameraSensitivity;
    public LayerMask IgnoredLayer;

    private WorldCreater worldCreater;
    private Vector3 moveDir = Vector3.zero;
    private Camera c;
    private float yRotation = 0;


    void Start()
    {
        c = Camera.main;
        worldCreater = GameObject.FindGameObjectWithTag("World").GetComponent<WorldCreater>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        MouseLook();
        MouseClick();
    }

    void Move()
    {
        bool jumped = false;
        var character = GetComponent<CharacterController>();
        if (character.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= MoveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumped = true;
                moveDir.y = JumpSpeed;
            }
        }

        if (!jumped)
        {
            moveDir.y -= Gravity * Time.deltaTime;
        }

        character.Move(moveDir * Time.deltaTime);
    }

    void MouseLook()
    {
        float yRot = -Input.GetAxis("Mouse Y") * CameraSensitivity;
        float xRot = Input.GetAxis("Mouse X") * CameraSensitivity;
        yRotation += yRot;
        yRotation = Mathf.Clamp(yRotation, -80, 80);

        if (xRot != 0)
        {
            transform.eulerAngles += new Vector3(0, xRot, 0);
        }

        if (yRot != 0)
        {
            c.transform.eulerAngles = new Vector3(yRotation, transform.eulerAngles.y, 0);
        }
    }

    void MouseClick()
    {
        var ray = new Ray(c.transform.position, c.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, ~IgnoredLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                worldCreater.DestroyGameObject(Vector3Int.FloorToInt(hit.transform.position));
            }

            if (Input.GetMouseButtonDown(1))
            {
                worldCreater.CreateGameObject(Vector3Int.FloorToInt(hit.transform.position + hit.normal));
            }
        }
    }
}
