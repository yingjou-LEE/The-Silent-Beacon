using UnityEngine;

public class SeaRockGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private Vector2 spawnBoatDistanceRange = new Vector2(3, 5);
    [SerializeField] private Vector2 spawnSizeRange = new Vector2(1, 3);
    [SerializeField] private Vector2 spawnRockForwardRange = new Vector2(2, 5);
    [SerializeField] private Vector2 spawnRockHorizontalRange = new Vector2(-1, 1);
    [SerializeField] private Transform boatTransform;
    [SerializeField] private Vector3 _lastSpawnPosition;

    private float _spawnDistance;
    
    private void Start()
    {
        _lastSpawnPosition = boatTransform.position;
        _spawnDistance = Random.Range(spawnBoatDistanceRange.x, spawnBoatDistanceRange.y);
    }
    
    private void Update()
    {
        if (Vector3.SqrMagnitude(boatTransform.position - _lastSpawnPosition) > _spawnDistance * _spawnDistance)
        {
            SpawnRock();
            _lastSpawnPosition = boatTransform.position;
        }
    }
    
    private void SpawnRock()
    {
        var randomIndex = Random.Range(0, rockPrefabs.Length);
        var randomSize = Random.Range(spawnSizeRange.x, spawnSizeRange.y);
        var randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        var randomPosition = boatTransform.position 
                             + boatTransform.forward * Random.Range(spawnRockForwardRange.x, spawnRockForwardRange.y) 
                             + boatTransform.right * Random.Range(spawnRockHorizontalRange.x, spawnRockHorizontalRange.y);
        
        Instantiate(rockPrefabs[randomIndex], randomPosition, randomRotation, transform)
            .transform.localScale = Vector3.one * randomSize;
        
        _spawnDistance = Random.Range(spawnBoatDistanceRange.x, spawnBoatDistanceRange.y);
    }
}