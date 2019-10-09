using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemHandler : MonoBehaviour
{
    private new Rigidbody rigidbody;

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float force = 100;

    [SerializeField]
    private float forceZ = 100;

    private Vector3 oldPosition;

    private void Awake()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        this.rigidbody.useGravity = false;
    }

    public void Drag(Vector3 position)
    {
        this.oldPosition = this.rigidbody.position;
        this.rigidbody.MovePosition(position);
    }

    public void Release()
    {
        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce((this.rigidbody.position - this.oldPosition) * this.force, ForceMode.Impulse);
        this.rigidbody.AddForce((this.transform.forward + this.transform.up / 2) * this.forceZ, ForceMode.Impulse);

        Destroy(this.gameObject, 3);
    }
}
