using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Classes
{
    public class OjException : System.Exception
    {
        public OjException(string msg) : base(msg)
        { }
    }

    public class OjMissingCastingException : OjException
    {
        public OjMissingCastingException(string msg) : base(msg)
        { }
    }

    public class OjInvalidStoneComboException : OjException
    {
        public OjInvalidStoneComboException(string msg) : base(msg)
        { }
    }

    public class OjMissingStoneException : OjException
    {
        public OjMissingStoneException(string msg) : base(msg)
        { }
    }

    public class OjMissingFindingException : OjException
    {
        public OjMissingFindingException(string msg) : base(msg)
        { }
    }

    public class OjMissingLaborException : OjException
    {
        public OjMissingLaborException(string msg) : base(msg)
        { }
    }

    public class OjMissingMiscException : OjException
    {
        public OjMissingMiscException(string msg) : base(msg)
        { }
    }

}