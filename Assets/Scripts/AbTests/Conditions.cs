using System;
using System.Runtime.Serialization;
using Core;

namespace AbTests
{
    public interface ICondition
    {
        bool Check(PlayerProfile profile);
    }

    [DataContract]
    public abstract class DateCondition : ICondition
    {
        public enum ComparationType
        {
            Bigger,
            Smaller,
            Equal
        }

        [DataMember]
        protected readonly DateTime TargetDate;
        [DataMember]
        protected readonly ComparationType Type;

        protected DateCondition(DateTime dateTime, ComparationType type)
        {
            TargetDate = dateTime;
            Type = type;
        }
        public virtual bool Check(PlayerProfile profile)
        {
            return false;
        }
    }

    public class LastLoginCondition : DateCondition
    {
        public LastLoginCondition(DateTime dateTime, ComparationType type) : base(dateTime, type)
        {
        }

        public override bool Check(PlayerProfile profile)
        {
            switch (Type)
            {
                case ComparationType.Bigger:
                    return profile.LastLoginDate > TargetDate ;
                case ComparationType.Smaller:
                    return profile.LastLoginDate < TargetDate;
                case ComparationType.Equal:
                    return profile.LastLoginDate == TargetDate;
            }

            return false;
        }
    }

    public class CurrenDateCondition : DateCondition
    {
        public CurrenDateCondition(DateTime dateTime, ComparationType type) : base(dateTime, type)
        {
        }

        public override bool Check(PlayerProfile profile)
        {
            switch (Type)
            {
                case ComparationType.Bigger:
                    return DateTime.Now  > TargetDate;
                case ComparationType.Smaller:
                    return DateTime.Now  < TargetDate;
                case ComparationType.Equal:
                    return DateTime.Now  == TargetDate;
            }
            return false;
        }
    }
}