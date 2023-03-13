using System;
using System.Collections.Generic;
using Core;

namespace AbTests
{
    public enum TestResult
    {
        NotStarted,
        Active,
        Activated,
        Blocked,
        Ended
    }

    public interface IAbTest
    { 
        public bool WasActivated { get; }
        public TestResult Run(PlayerProfile profile, HashSet<Type> restriction);
        public Type GetActiveActionType();
    }
}