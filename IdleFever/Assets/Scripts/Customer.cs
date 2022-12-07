using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [field: SerializeField] public int MaxStack { get; private set; } = 5;
    public Stack<Supply> CustomerStack = new Stack<Supply>();

    private void Start()
    {
        StartCoroutine(ExchangeBoxes());
    }
    IEnumerator ExchangeBoxes()
    {
        while (true)
        {
            if (CustomerStack.Count == 0) yield return null;
            if (CustomerStack.Count > 0)
            {
                var box = CustomerStack.Pop();
                Supplier.Pool.Release(box);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
