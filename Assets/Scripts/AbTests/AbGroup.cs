using System.Runtime.Serialization;
using Core;

namespace AbTests
{
    [DataContract]
    public class AbGroup
    {
        [DataMember]
        public string Name { get; }
        [DataMember]
        public double Weight { get; }

        [DataMember]
        public IGroupAction GroupAction;
        
        public AbGroup(string name, double weight, IGroupAction groupAction)
        {
            Name = name;
            Weight = weight;
            GroupAction = groupAction;
        }

        public void Apply(PlayerProfile profile)
        {
            GroupAction.Apply(profile);
        }
    }
}