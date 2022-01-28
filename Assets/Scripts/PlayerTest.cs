using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoleState
{
    Idle,
    Run,
    None,
}

public class PlayerTest : MonoBehaviour
{
    // Start is called before the first frame update
    float x;
    float MouseX;
    float MouseY;

    float z;

    public float speed = 5;
    public float rotSpeed = 25;

    Vector3 temp;
    Vector3 tempV3;
    Quaternion targetQua;
    Transform cam;

    Transform shoulderTrans;

    Animator ani;

    RoleState state;

    void Start()
    {
        temp = new Vector3();
        tempV3 = new Vector3();
        cam = Camera.main.transform;
        shoulderTrans = transform.FindChildWithName("shoulderTarget");
        Cursor.lockState = CursorLockMode.Locked;
        ani = GetComponentInChildren<Animator>();
        state = RoleState.Idle;
        //CursorLockMode.Locked;
    }

    // void Rotate()
    // {
    //     temp.Set(0, cam.localEulerAngles.y - Vector2.SignedAngle(Vector2.up, temp), 0);
    //     targetQua = Quaternion.Euler(temp);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, targetQua, Time.deltaTime * rotSpeed);
    // }

    void Rotate()
    {
        shoulderTrans.transform.rotation *= Quaternion.AngleAxis(MouseX * rotSpeed, Vector3.up);
        shoulderTrans.transform.rotation *= Quaternion.AngleAxis(MouseY * rotSpeed, -Vector3.right);

        var angles = shoulderTrans.transform.localEulerAngles;
        angles.z = 0;

        var angle = shoulderTrans.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        shoulderTrans.transform.localEulerAngles = angles;
        //targetQua = shoulderTrans.rotation;

    }

    int sign;
    void Move()
    {
        // Debug.Log(Mathf.Cos((transform.localEulerAngles.y - cam.transform.eulerAngles.y) * Mathf.Deg2Rad));
        // z = Mathf.Cos((transform.localEulerAngles.y - cam.transform.eulerAngles.y) * Mathf.Deg2Rad) * Mathf.Sign(transform.localEulerAngles.y - cam.transform.eulerAngles.y);
        // x = z >0? 1;
        // sign = (int)Mathf.Sign(x);
        // temp.Set(x,0,z);
        // Debug.Log(temp);
        // transform.Translate(temp * Time.deltaTime * speed);
        RoleTurn();

        transform.Translate(transform.forward * Time.deltaTime * speed,Space.Self);
    }

    float angle;


    void RoleTurn()
    {
        //Vector3.Cross()
        tempV3 = shoulderTrans.eulerAngles;
        angle = Vector3.SignedAngle(transform.forward, shoulderTrans.forward, Vector3.up);
        // Debug.Log(Quaternion.Angle(transform.rotation,shoulderTrans.rotation));
        temp.Set(0, transform.localEulerAngles.y + angle, 0);
        targetQua = Quaternion.Euler(temp);

        if (Mathf.Abs(Quaternion.Dot(transform.rotation,targetQua)) >=0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetQua, Time.deltaTime * rotSpeed);
        }
        else
        {
            transform.rotation = targetQua;
        }
        shoulderTrans.eulerAngles = tempV3;

    }


    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        Rotate();

        if (x != 0 || z != 0)
        {
            temp.Set(x, 0, z);
            // Rotate();
            Move();
            SetAni(RoleState.Run);
        }
        else
        {
            SetAni(RoleState.Idle);
        }
    }

    void SetAni(RoleState aniState)
    {
        if (state != RoleState.Idle && aniState == RoleState.Idle)
        {
            state = RoleState.Idle;
            ani.SetBool("isRun", false);
            Debug.Log("1");
        }
        else if (state != RoleState.Run && aniState == RoleState.Run)
        {
            state = RoleState.Run;
            ani.SetBool("isRun", true);
            Debug.Log("2");
        }
    }
}
