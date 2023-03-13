using System;
using UnityEngine;
using AbTests;
using Core;
using TMPro;

public class TestModule : MonoBehaviour
{
    [SerializeField] private int version;

    [SerializeField]
    private PlayerProfile profile;

    [SerializeField] 
    private TextMeshProUGUI idLabel;

    [SerializeField]
    private TextMeshProUGUI freeMoneyLabel;

    public void Start()
    {
        profile = new PlayerProfile(355);
        profile.OnProfileUpdate += OnProfileUpdate;
        OnProfileUpdate();
    }

    public void GenerateRemoteManifestV1()
    {
        TestDataGenerator.GenerateRemoteManifest(version);
    }

    public void GenerateRemoteManifestV2()
    {
        TestDataGenerator.GenerateRemoteManifest(version + 1);
    }

    public void StartAbTestModule()
    {
        ILoader<AbTestsData> loader = new FileLoader<AbTestsData>();
        var testModule = new AbTestsModule(loader, loader, profile);
        testModule.RunTests();
    }

    private void OnProfileUpdate()
    {
        idLabel.text = $"ID: {profile.ID}";
        freeMoneyLabel.text = $"Free Money {profile.FreeMoney}";
    }
}