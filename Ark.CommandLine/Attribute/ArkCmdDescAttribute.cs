using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ark.CommandLine.Attribute
{
    using System.Collections;


    [AttributeUsage(AttributeTargets.Property)]
    public class ArkCmdDescAttribute : System.Attribute
    {
        private readonly string _fullName;
        private readonly string _shortName;
        private readonly bool _isRequire;
        private readonly List<string> _parameters;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ArkCmdDescAttribute(string fullName, string shortName, bool isRequire)
            : this(fullName: fullName, shortName: shortName, isRequire: isRequire, parameters: new string[] { })
        {

        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public ArkCmdDescAttribute(string fullName, string shortName, bool isRequire, params string[] parameters)
        {
            _fullName = fullName;
            _shortName = shortName ?? string.Empty;
            _isRequire = isRequire;
            _parameters = new List<string>(parameters ?? new string[] { });
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        public string FullName => _fullName;

        //--------------------------------------------------------------------------------------------------------------------------------------

        public string ShortName => _shortName;

        //--------------------------------------------------------------------------------------------------------------------------------------

        public IEnumerable<string> Aliases
        {
            get
            {
                yield return _fullName;

                if (string.IsNullOrEmpty(_shortName) == false)
                {
                    yield return _shortName;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public bool IsRequire
        {
            get { return _isRequire; }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        public List<string> Parameters => _parameters;
    }


}
