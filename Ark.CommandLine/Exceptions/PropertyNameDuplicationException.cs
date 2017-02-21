using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ark.CommandLine.Exceptions
{
    public class PropertyNameDuplicationException : Exception
    {
        public PropertyNameDuplicationException(string propertyName)
            : base(string.Format("[{0}] shouldnt be repeated more than once", propertyName))
        {

        }
    }
}
