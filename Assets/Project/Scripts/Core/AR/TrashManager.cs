using System;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterAction;

    private void OnEnable()
    {
        GameUIView.Get.SetTrashManager(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Props"))
        {
            this.OnTriggerEnterAction?.Invoke(other);
        }
    }
}
