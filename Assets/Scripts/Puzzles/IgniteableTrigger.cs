using System.Collections.Generic;
using UnityEngine;

public class IgniteableTrigger : MonoBehaviour
{
    [SerializeField] private List<Igniteable> igniteables;
    [SerializeField]
    private GameObject painting;
    
    private int igniteablesCount;
    private int ignitedCount;

    private void OnEnable()
    {
        foreach (var igniteable in igniteables)
            igniteable.OnIgnited.AddListener(OnIgnited);
    }
    
    private void OnDisable()
    {
        foreach (var igniteable in igniteables)
            igniteable.OnIgnited.RemoveListener(OnIgnited);
    }

    private void Start()
    {
        igniteablesCount = igniteables.Count;
    }

    public void OnIgnited()
    {
        ignitedCount++;
        if (ignitedCount == igniteablesCount)
        {
            painting.SetActive(false);
            GameManager.Instance.CompletedLightPuzzle();
        }
    }
}
