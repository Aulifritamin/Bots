using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class StorageView : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
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

    private void RefreshView(IReadOnlyDictionary<string, int> storage)
    {
        foreach (var kvp in storage)
        {
            _text.text = $"{kvp.Key}: {kvp.Value}";
        }
    }
}
