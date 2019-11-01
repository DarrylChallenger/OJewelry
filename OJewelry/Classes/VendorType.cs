using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OJewelry.Classes
{
    [Flags] public enum vendorTypeEnum { General = 0x0, Stone = 0x2, Finding = 0x4 };

    public class VendorTypeEnumObj
    {
        public String Value { get; set; }
        public int Id { get; set; }
    }

    public class VendorType
    {
        vendorTypeEnum _type;
        public VendorType()
        {
            _type = vendorTypeEnum.General;
        }
        public vendorTypeEnum Type
        {
            get { return _type; }
            private set { _type = value; }
        }

        /*
        [NotMapped]
        public vendorTypeEnum bGeneral
        {
            get => _type & vendorTypeEnum.General;
            set { _type = (_type & (vendorTypeEnum.General)) | value; }
        }
        */
        [NotMapped]
        public vendorTypeEnum bStone
        {
            get => _type & vendorTypeEnum.Stone;
            set { _type = (_type & (~vendorTypeEnum.Stone)) | value; }
        }

        [NotMapped]
        public vendorTypeEnum bFinding
        {
            get => _type & vendorTypeEnum.Finding;
            set { _type = (_type & (~vendorTypeEnum.Finding)) | value; }
        }

        public vendorTypeEnum Clear()
        {
            _type = vendorTypeEnum.General;
            return vendorTypeEnum.General;
        }

        public IEnumerable<VendorTypeEnumObj> GetEnumOjbs()
        {
            List<VendorTypeEnumObj> vte = new List<VendorTypeEnumObj>();
            Array names = Enum.GetNames(typeof(vendorTypeEnum));
            int i = 0;
            foreach (int v in Enum.GetValues(typeof(vendorTypeEnum)))
            {
                vte.Add(new VendorTypeEnumObj { Value = names.GetValue(i).ToString(), Id = v });
                i++;
            }
            return vte;
        }
    }
}