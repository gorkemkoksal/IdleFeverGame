using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private int _maxStack = 5;
    [SerializeField] Supplier _supplier;
    [SerializeField] Customer _customer;
    [SerializeField] Transform _carryPos;
    [SerializeField] Transform _deployPos;

    [SerializeField] float deployTime = 0.5f;

    private Vector3 boxDistances = new Vector3(0, 0.4f, 0);

    public Stack<Supply> CarryingStack = new Stack<Supply>();
   // private bool IsGathering;

    private void OnTriggerStay(Collider other)
    {
        //if (IsGathering) return;
        // Debug.Log("Entered"); buna goz at
        if (other.tag == "Supplier" && CarryingStack.Count < _maxStack && _supplier.Stock.Count > 0)
        {
            StartCoroutine(DeployingBoxes(_supplier.Stock, CarryingStack, _carryPos));
        }
        if (other.tag == "Customer" && _customer.CustomerStack.Count < _customer.MaxStack && CarryingStack.Count > 0)
        {
            StartCoroutine(DeployingBoxes(CarryingStack, _customer.CustomerStack, _deployPos));
        }
        //IsGathering = true;
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Supplier" && other.tag == "Customer")
    //    {
    //        IsGathering = false;
    //    }
    //}
    IEnumerator DeployingBoxes(Stack<Supply> giver, Stack<Supply> taker, Transform parent)
    {
        var box = giver.Pop();
        taker.Push(box);
        box.transform.SetParent(parent);
        box.transform.DOLocalMove(boxDistances * (taker.Count - 1), deployTime).SetEase(Ease.InOutSine);
        box.transform.rotation = parent.rotation;

        yield return new WaitForSeconds(deployTime);
    }
}
