
namespace AddressApi.Schemas
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Address
    {
        [DataMember(Name = AttributeNames.AddressLine)]
        public virtual string AddressLine
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.District)]
        public virtual string District         
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.City)]
        public virtual string City
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Country, IsRequired = true)]
        public virtual string Country
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.NameLast)]
        public virtual string NameLast
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.NameFirst)]
        public virtual string NameFirst
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.PostalCode)]
        public virtual string PostalCode
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Province)]
        public virtual string Province
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Region)]
        public virtual string Region
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Street)]
        public virtual string Street
        {
            get;
            set;
        }

        [DataMember(Name = AttributeNames.Unit)]
        public virtual string Unit
        {
            get;
            set;
        }
    }
}
