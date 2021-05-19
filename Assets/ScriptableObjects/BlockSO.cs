using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Block Data")]
public class BlockSO : ScriptableObject
{
    public Material material;

    //Amount of damage an item can take before destructing fully
    [Range(0, 100)]
    public float durability;

    public bool gravity;

    public Texture sprite;

    [Range(0, 64)]
    public int stackSize;

}
