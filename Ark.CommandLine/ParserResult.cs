namespace Ark.CommandLine
{
    using System;

    public class ParserResult
    {
        private readonly bool _isSucceeded;
        private readonly Exception _exception;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ParserResult(bool isSucceeded, Exception exception)
        {
            _isSucceeded = isSucceeded;
            _exception = exception;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public bool IsSucceeded => _isSucceeded;

        //--------------------------------------------------------------------------------------------------------------------------------------
        public Exception Exception => _exception;
    }

    public class ParserResult<TTargetClass> : ParserResult where TTargetClass : new()
    {
        private readonly TTargetClass _targetClass;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ParserResult(bool isSucceeded, Exception exception, TTargetClass targetClass)
            : base(isSucceeded: isSucceeded, exception: exception)
        {
            _targetClass = targetClass;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public TTargetClass TargetClass => _targetClass;
    }
}