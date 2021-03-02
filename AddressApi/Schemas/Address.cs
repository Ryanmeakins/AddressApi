
namespace AddressApi.Schemas
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Address
    {
        [DataMember(Name = AttributeNames.City)]
        public virtual string City
        {
            get;
            set;
        }
    }
}
