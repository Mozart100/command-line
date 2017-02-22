namespace Ark.CommandLine
{
    using System;

    public class ParserResult
    {
        private readonly bool _isSucceeded;
        private readonly Exception [] _exceptions;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ParserResult(bool isSucceeded,params  Exception [] exceptions)
        {
            _isSucceeded = isSucceeded;
            _exceptions = exceptions;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public bool IsSucceeded => _isSucceeded;

        //--------------------------------------------------------------------------------------------------------------------------------------
        public Exception [] Exceptions => _exceptions;
    }

    public class ParserResult<TTargetClass> : ParserResult where TTargetClass : new()
    {
        private readonly TTargetClass _targetClass;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ParserResult(bool isSucceeded, Exception [] exceptions, TTargetClass targetClass)
            : base(isSucceeded: isSucceeded, exceptions: exceptions)
        {
            _targetClass = targetClass;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TTargetClass TargetClass => _targetClass;
    }
}