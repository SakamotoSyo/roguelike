using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    [Header("Destroy‚·‚é‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField] float _destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
}
