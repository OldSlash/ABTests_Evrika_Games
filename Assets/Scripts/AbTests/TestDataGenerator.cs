using System;
using System.Collections.Generic;
using Core;

namespace AbTests
{
    public static class TestDataGenerator
    {
        public static void GenerateRemoteManifest(int version)
        {
            Dictionary<int, IAbTest> dictionary = new Dictionary<int, IAbTest>();

            var test = GetActiveTest();
            dictionary.Add(test.Id, test);
            test = GetInactiveTest();
            dictionary.Add(test.Id, test);
            test = GetEndedTest();
            dictionary.Add(test.Id, test);

            AbTestsData testsData = new AbTestsData()
            {
                AbTests = dictionary,
                Version = version
            };
            FileLoader<AbTestsData> loader = new FileLoader<AbTestsData>();
            loader.Save(AbTestsModule.REMOTE_TESTS, testsData);
        }

        private static AbTest GetActiveTest()
        {
            var startCondition =
                new LastLoginCondition(new DateTime(2022, 4, 11), DateCondition.ComparationType.Bigger);
            var endCondition =
                new CurrenDateCondition(new DateTime(2023, 4, 15), DateCondition.ComparationType.Bigger);
            var Groups = new List<AbGroup>()
            {
                new AbGroup("A", 10, new ApplyFreeMoney(100)),
                new AbGroup("B", 90, new ApplyFreeMoney(300))
            };
            return new AbTest(1, startCondition, endCondition, Groups);
        }

        private static AbTest GetEndedTest()
        {
            var startCondition =
                new LastLoginCondition(new DateTime(2022, 4, 11), DateCondition.ComparationType.Bigger);
            var endCondition =
                new CurrenDateCondition(new DateTime(2022, 4, 15), DateCondition.ComparationType.Bigger);
            var Groups = new List<AbGroup>()
            {
                new AbGroup("A", 10, new ApplyFreeMoney(100)),
                new AbGroup("B", 90, new ApplyFreeMoney(300))
            };
            return new AbTest(2, startCondition, endCondition, Groups);
        }

        private static AbTest GetInactiveTest()
        {
            var startCondition =
                new LastLoginCondition(new DateTime(2024, 4, 11), DateCondition.ComparationType.Bigger);
            var endCondition =
                new CurrenDateCondition(new DateTime(2024, 4, 15), DateCondition.ComparationType.Bigger);
            var Groups = new List<AbGroup>()
            {
                new AbGroup("A", 10, new ApplyFreeMoney(100)),
                new AbGroup("B", 90, new ApplyFreeMoney(300))
            };
            return new AbTest(3, startCondition, endCondition, Groups);
        }
    }
}