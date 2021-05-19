using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : ItemManager
{
    [SerializeField]
    private BlockSO blockData;

    public BlockSO BlockData
    {
        get { return blockData; }
        set { blockData = value; }
    }

    private bool destroyed;

    float tempValue;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = blockData.gravity;
        this.GetComponent<Renderer>().material = blockData.material;
        stackSize = blockData.stackSize;
        sprite = blockData.sprite;
        if(!blockData.gravity)
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | 
                RigidbodyConstraints.FreezeRotation;
        } else
        {
            this.GetComponent<Rigidbody>().constraints
                = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
        }
        //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Destroyed()
    {
        transform.localScale = new Vector3(.3f, .3f, .3f);
        this.GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.tag = "Item";
        this.GetComponent<Collider>().isTrigger = true;
        //update all blocks using gravity to use gravity? (very slow)
        destroyed = true;
    }

    void Update()
    {
        if(destroyed)
        {
            float y = Mathf.PingPong(Time.time * .5f, .3f) + .2f;
            transform.position = new Vector3(transform.position.x, y, this.transform.position.z);
        }

    }

    void FixedUpdate()
    {
        //TEMPORARY SOLUTION
        //CURRENTLY NEEDED TO UPDATE GRAVITY WHEN BLOCKS UNDER A GRAVITY BLOCK ARE DETROYED
        if(blockData.gravity)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
