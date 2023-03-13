using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Core;
using UnityEngine;

namespace AbTests
{
    [DataContract]
    public class AbTestsData
    {
        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public Dictionary<int,IAbTest> AbTests { get; set; }

        public void MergeActivated(Dictionary<int, IAbTest> inputDictionary)
        {
            foreach (var testKvp in inputDictionary)
            {
                //We don't care about tests which weren't activated
                //If they are still actual, then AbTests will contain it.
                if(!testKvp.Value.WasActivated)
                   continue;

                if (AbTests.ContainsKey(testKvp.Key))
                    AbTests[testKvp.Key] = testKvp.Value;
                else
                    AbTests.Add(testKvp.Key, testKvp.Value);
            }
        }
    }
    
    public class AbTestsModule
    {
        public const string STORED_TESTS = "ABTests.json";
        public const string REMOTE_TESTS = "ABTestsRemote.json";

        private readonly PlayerProfile _profile;

        private readonly ILoader<AbTestsData> _localLoader;
        private readonly ILoader<AbTestsData> _remoteLoader;

        private AbTestsData _tests;

        //We need this to prevent conflicts
        private HashSet<Type> _restrictions;

        public AbTestsModule(ILoader<AbTestsData> localLoader, ILoader<AbTestsData> remoteLoader, PlayerProfile profile)
        {
            _localLoader = localLoader;
            _remoteLoader = remoteLoader;
            _profile = profile;

            _tests = LoadLocalAbTests();
            GetRestrictions(ref _restrictions, _tests);
            UpdateAbTests(ref _tests);
        }

        private AbTestsData LoadLocalAbTests()
        {
            return LoadTests(STORED_TESTS, _localLoader);
        }

        private void GetRestrictions(ref HashSet<Type> restrictions, AbTestsData tests)
        {
            restrictions ??= new HashSet<Type>();
            
            if(restrictions.Count > 0)
                restrictions.Clear();

            if (tests?.AbTests != null)
            {
                foreach (var kvp in tests.AbTests)
                {
                    if (kvp.Value.WasActivated)
                    {
                        Type type = kvp.Value.GetActiveActionType();
                        if(type != null)
                            restrictions.Add(type);
                    }
                }
            }
        }

        private void UpdateAbTests(ref AbTestsData tests)
        {
            var remoteTests = LoadTests(REMOTE_TESTS, _remoteLoader);
            
            if (remoteTests == null)
                return;

            if (tests == null)
                tests = remoteTests;
            else if (tests.Version < remoteTests.Version)
            {
                remoteTests.MergeActivated(tests.AbTests);
                tests = remoteTests;
            }

            SaveLocalTests();
        }

        private bool SaveLocalTests()
        {
           return _localLoader.Save(STORED_TESTS, _tests);
        }
        
        private T LoadTests<T>(string fileName, ILoader<T> loader)
        {
            return loader.Load(fileName);
        }

        public void RunTests()
        {
            if (_tests?.AbTests == null || _tests.AbTests.Count <= 0)
            {
                //No tests to Run
                return;
            }

            List<int> itemsToRemove = new List<int>();
            foreach (var kvp in _tests.AbTests)
            {
                var testResult = kvp.Value.Run(_profile, _restrictions);
                
                Debug.Log($"TestModule: Run test {kvp.Key} : {kvp.Value.GetType()} with result {testResult}");

                switch (testResult)
                {
                    case TestResult.Ended:
                        //Remove inactive tests
                        //Additional actions could required
                        itemsToRemove.Add(kvp.Key);
                        break;
                    case TestResult.Activated:
                        //If test was activated we should avoid any test with same action type
                        _restrictions.Add(kvp.Value.GetActiveActionType());
                        //We definitely want to send info to our backend
                        break;
                }
            }

            foreach (var item in itemsToRemove)
            {
                _tests.AbTests.Remove(item);
            }

            SaveLocalTests();
        }
    }
}