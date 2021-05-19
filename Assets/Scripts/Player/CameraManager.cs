using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    private float mouseSensitivity = 100f;
    float xRotation = 0f;

    private float x;
    private float y;
    private Vector3 rotateValue;

    [SerializeField]
    private Transform player;

    private bool cameraFrozen;

    [SerializeField]
    private Transform lastActiveBlock;
    [SerializeField]
    private Transform currentActiveBlock;

    public Transform CurrentActiveBlock
    {
        get { return currentActiveBlock; }
    }


    // Start is called before the first frame update
    void Start()
    {
        AlterCursorLock(true);
    }


    public void AlterCursorLock(bool lockState)
    {
        if(lockState)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }

    public void Freeze(bool frozenState)
    {
        cameraFrozen = frozenState;
    }

    // Update is called once per frame
    void Update()
    { 
        if(cameraFrozen) { return; }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        HighlightActiveBlock();

    }

    void HighlightActiveBlock()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;
        Debug.DrawRay(transform.position, forward, Color.green);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            if(hit.collider.gameObject.tag == "Block")
            {

                if(lastActiveBlock == null)
                {
                    lastActiveBlock = hit.collider.gameObject.transform;
                }

                if(lastActiveBlock != hit.collider.gameObject.transform)
                {
                    lastActiveBlock.GetComponent<Renderer>().material.color = Color.white;
                }
                lastActiveBlock = hit.collider.gameObject.transform;
                lastActiveBlock.GetComponent<Renderer>().material.color = Color.yellow;
                currentActiveBlock = lastActiveBlock;
            } 
        } 
        else {
            if(lastActiveBlock == null)
            {
                return;
            }
            lastActiveBlock.GetComponent<Renderer>().material.color = Color.white;
            currentActiveBlock = null;
        }
    }
}
