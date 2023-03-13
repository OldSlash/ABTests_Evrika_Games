using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core;

namespace AbTests
{
    [DataContract]
    public class AbTest : IAbTest
    {
        [DataMember]
        public int AssignedGroup = -1;
        
        [DataMember]
        public readonly int Id;
        [DataMember]
        public List<AbGroup> Groups;
        
        public bool WasActivated => AssignedGroup > -1;

        [DataMember]
        private readonly ICondition _startConditions;
        [DataMember]
        private readonly ICondition _endConditions;

        public AbTest(int id, ICondition startConditions, ICondition endConditions, List<AbGroup> groups)
        {
            Id = id;
            _startConditions = startConditions;
            _endConditions = endConditions;
            Groups = groups;
        }

        public TestResult Run(PlayerProfile profile, HashSet<Type> restriction)
        {
            if (IsEnded(profile))
                return TestResult.Ended;
            if (WasActivated)
                return TestResult.Active;
            
            if (!IsStarted(profile))
                return TestResult.NotStarted;
            
            
            AssignedGroup = GetGroupId(Groups);
            AbGroup targetAbGroup = Groups[AssignedGroup];
            
            if (restriction.Contains(targetAbGroup.GroupAction.GetType()))
                return TestResult.Blocked;
            targetAbGroup.Apply(profile);

            return TestResult.Activated;
        }
        
        public Type GetActiveActionType()
        {
            if (WasActivated && Groups?.Count > 0)
                return Groups[AssignedGroup].GroupAction.GetType();

            return null;
        }

        private int GetGroupId(List<AbGroup> groups)
        {
            double randomNum = new Random().NextDouble();
            double proportionSum = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                AbGroup abGroup = groups[i];
                proportionSum += abGroup.Weight;
                if (randomNum < proportionSum)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool IsStarted(PlayerProfile profile)
        {
            return _startConditions.Check(profile);
        }

        private bool IsEnded(PlayerProfile profile)
        {
            return _endConditions.Check(profile);
        }
    }
}