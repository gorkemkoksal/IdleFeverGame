using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Supplier : MonoBehaviour
{
    [SerializeField] private Supply _supplyPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float supplyRate = 1f;
    [SerializeField] private int _maxCapacity = 5;
    
    private Vector3 boxDistances = new Vector3(0, 0.4f, 0);
    public static ObjectPool<Supply> Pool;
    
    public Stack<Supply> Stock = new Stack<Supply>();
    void Start()
    {
        Pool = new ObjectPool<Supply>(() =>
        {
            return Instantiate(_supplyPrefab);
        }, supply =>
        {
            supply.gameObject.SetActive(true);
            supply.transform.position = _spawnPoint.position + boxDistances * Stock.Count;
        }, supply =>
        {
            supply.gameObject.SetActive(false);
        }, supply =>
        {
            Destroy(supply.gameObject);
        });

        StartCoroutine(Supplying());
    }
    IEnumerator Supplying()
    {
        while (true)
        {
            yield return new WaitUntil(() => Stock.Count < _maxCapacity);
            var stock = Pool.Get();
            Stock.Push(stock);
            yield return new WaitForSeconds(supplyRate);
        }
    }
}
