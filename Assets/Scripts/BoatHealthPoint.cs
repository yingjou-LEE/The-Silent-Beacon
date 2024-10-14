using System;
using UnityEngine;

public class BoatHealthPoint : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int hp = 100;

    [SerializeField] private int damageTutorial;
    [SerializeField] private int damageNormal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoatDanger"))
        {
            hp -= GameManager.Instance.CurrentChapter < Chapter.C2A1 ? damageTutorial : damageNormal;
            Debug.Log($"Boat HP: {hp}");
        }
    }
}