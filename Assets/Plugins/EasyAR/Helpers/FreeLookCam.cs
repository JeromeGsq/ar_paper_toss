using UnityEngine;

public class FreeLookCam : MonoBehaviour
{
    [SerializeField]
    private float lookSpeedH = 2f;
    [SerializeField]
    private float lookSpeedV = 2f;
    [SerializeField]
    private float zoomSpeed = 2f;
    [SerializeField]
    private float dragSpeed = 6f;
    [SerializeField]
    private float movementSpeed = 0.02f;

    private float yaw;
    private float pitch;

    void Update()
    {
        //Look around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            yaw += this.lookSpeedH * Input.GetAxis("Mouse X");
            pitch -= this.lookSpeedV * Input.GetAxis("Mouse Y");

            this.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
        }

        //drag camera around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            this.transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * this.dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * this.dragSpeed, 0);
        }

        // ZQSD Move
        if (Input.GetKey(KeyCode.Z))
        {
            this.transform.Translate(0, 0, 1 * this.movementSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, 0, -1 * this.movementSpeed, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(1 * this.movementSpeed, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.Translate(-1 * this.movementSpeed, 0, 0, Space.Self);
        }

        //Zoom in and out with Mouse Wheel
        this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed, Space.Self);
    }
}