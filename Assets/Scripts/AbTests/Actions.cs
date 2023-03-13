using System.Runtime.Serialization;
using Core;

namespace AbTests
{
    public interface IGroupAction
    {
        void Apply(PlayerProfile profile);
    }

    [DataContract]
    public class ApplyFreeMoney : IGroupAction
    {
        [DataMember]
        private int _freeMoney;

        public ApplyFreeMoney(int freeMoney)
        {
            _freeMoney = freeMoney;
        }

        public void Apply(PlayerProfile profile)
        {
            profile.FreeMoney = _freeMoney;
        }
    }
}