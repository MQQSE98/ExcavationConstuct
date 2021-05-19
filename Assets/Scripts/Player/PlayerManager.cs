using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //INVENTORY VARIABLES
    [SerializeField]
    private GameObject inventoryPanel;
    public GameObject[] inventory;
    public RawImage[] inventorySlots;

    //!INVENTORY VARIABLES

    //Player fields
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private CharacterController controller;

    private Vector3 movement;
    private Vector3 jumpMovement;
    private float velocity;
    private float gravity = 9.81f;
    private float jumpForce = 2f;
    private CameraManager cameraManager;

    //MINING VARIABLES
    private float blockDurability;
    private Transform currentMiningBlock;
    private float strength = 20;
    //!MINING VARIABLES
    private float attack = 10;
    private float defence = 10;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = this.transform.GetChild(1).GetComponent<CameraManager>();
        inventory = new GameObject[36];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            cameraManager.Freeze(inventoryPanel.activeSelf);
            cameraManager.AlterCursorLock(!inventoryPanel.activeSelf);
        }
        if (inventoryPanel.activeSelf) { return; }
        if (controller.isGrounded)
        {
            velocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = jumpForce;
            }
        }
        else
        {
            velocity -= gravity * Time.deltaTime;
        }

        jumpMovement = new Vector3(0, velocity, 0);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z;
        Vector3 temp = new Vector3(movement.x, velocity, movement.z);

        controller.Move(temp * moveSpeed * Time.deltaTime);
        
        
        MineBlock();
    }


    void MineBlock()
    {
        if(cameraManager.CurrentActiveBlock == null) { return; }

        if(Input.GetMouseButtonDown(0))
        {
            blockDurability = cameraManager.CurrentActiveBlock.gameObject.GetComponent<BlockManager>().BlockData.durability;
            currentMiningBlock = cameraManager.CurrentActiveBlock;
        }
        if(Input.GetMouseButton(0))
        {
            if(currentMiningBlock != cameraManager.CurrentActiveBlock)
            {
                currentMiningBlock = cameraManager.CurrentActiveBlock;
                blockDurability = cameraManager.CurrentActiveBlock.gameObject.GetComponent<BlockManager>().BlockData.durability;
            }
            if (blockDurability <= 0)
            {
                cameraManager.CurrentActiveBlock.gameObject.GetComponent<BlockManager>().Destroyed();
            }

            blockDurability -= strength * Time.deltaTime;

        }
        if(Input.GetMouseButtonUp(0))
        {
            blockDurability = 0;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Item")
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i] == null)
                {
                    Debug.Log(i);
                    inventory[i] = col.gameObject;
                    inventorySlots[i].texture = col.gameObject.transform.GetComponent<ItemManager>().Sprite;
                    inventorySlots[i].color = new Color(inventorySlots[i].color.r, 
                        inventorySlots[i].color.g, inventorySlots[i].color.b, 1f);
                    //set game object inactive
                    col.gameObject.SetActive(false);
                    //move to player inventory in inspector heirarchy
                    col.gameObject.transform.parent = this.transform.GetChild(2);
                    return;
                }
            }
            Debug.Log("Cannot fit in inventory");
            //add to inventory if possible
        }
    }

}
