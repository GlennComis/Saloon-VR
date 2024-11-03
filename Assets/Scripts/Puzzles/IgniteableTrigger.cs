using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IgniteableTrigger : MonoBehaviour
{
    public UnityEvent OnIgnitedAll;
    
    [SerializeField] private List<Igniteable> igniteables;
    
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
            Debug.Log("All igniteables have been ignited!");
            OnIgnitedAll?.Invoke();
        }
    }
}
