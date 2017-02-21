namespace Ark.CommandLine.Attribute
{
    using System;

    [AttributeUsage(AttributeTargets.Property,AllowMultiple =true)]
    public class ArkCmdArgumentsDescAttribute : System.Attribute
    {
        private readonly string[] _parameters;


        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ArkCmdArgumentsDescAttribute(params string[] parameters)
            //: base(fullName: "", shortName: "", isRequire: false)
        {
            _parameters = parameters;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        public string[] Parameters => _parameters;
    }
}