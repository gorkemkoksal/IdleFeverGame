using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private int _maxStack = 5;
    [SerializeField] Supplier _supplier;
    [SerializeField] float deployTime = 0.5f;
    [SerializeField] Transform _carryPos;

    private Vector3 boxDistances = new Vector3(0, 0.4f, 0);

    public Stack<Supply> CarryingStack = new Stack<Supply>();
 
    private void OnTriggerStay(Collider other)
    {
       // Debug.Log("Entered"); buna goz at
        if (other.tag == "Supplier" && CarryingStack.Count < _maxStack && _supplier.Stock.Count > 0)
        {
            StartCoroutine(PlacingBoxes());
        }
    }
    IEnumerator PlacingBoxes()
    {
        var box = _supplier.Stock.Pop();
        CarryingStack.Push(box);
        box.transform.SetParent(_carryPos.transform);
        SetBoxToPlayer(box);
        yield return new WaitForSeconds(deployTime);
    }
    public void SetBoxToPlayer(Supply cube)
    {
        Debug.Log(CarryingStack.Count + " a");
        cube.transform.DOLocalMove(boxDistances * (CarryingStack.Count-1), deployTime).SetEase(Ease.InOutSine);
        cube.transform.rotation = transform.rotation;
    }
}
