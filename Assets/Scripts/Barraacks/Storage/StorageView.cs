using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class StorageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Storage _storage;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _text.text = "Storage is empty";
    }
    private void OnEnable()
    {
        _storage.CountChanged += RefreshView;
    }

    private void OnDisable()
    {
        _storage.CountChanged -= RefreshView;
    }

    private void RefreshView(Dictionary<string, int> storage)
    {
        _text.text = "Storage Capacity:\n";
        
        foreach (var kvp in storage)
        {
            _text.text += $"{kvp.Key}: {kvp.Value}\n";
        }
    }
}
