using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ark.CommandLine
{
    using System.Collections;
    using System.Reflection;
    using Ark.CommandLine.Attribute;
    using Ark.CommandLine.Exceptions;

    public class ArkParser<TTargetClass> where TTargetClass : new()
    {
        private class FullPropertyDescription
        {
            private readonly PropertyInfo _propertyInfo;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            private readonly string _fullName;
            private readonly string _shortName;
            private readonly bool _isRequire;

            private readonly Dictionary<int, List<string>> _parameters;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            internal FullPropertyDescription(ArkCmdDescAttribute cmdDescription, PropertyInfo propertyInfo)
                : this(fullName: cmdDescription.FullName, shortName: cmdDescription.ShortName, isRequire: cmdDescription.IsRequire, propertyInfo: propertyInfo)
            {

            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            private FullPropertyDescription(string fullName, string shortName, bool isRequire, PropertyInfo propertyInfo, List<List<string>> parameters = null)
            {
                _fullName = fullName;
                _shortName = shortName;
                _isRequire = isRequire;
                _propertyInfo = propertyInfo;

                _parameters = new Dictionary<int, List<string>>();
                if (parameters != null)
                {
                    var index = 0;
                    foreach (var prms in parameters)
                    {
                        _parameters.Add(++index, prms);
                    }
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public static FullPropertyDescription operator +(FullPropertyDescription fullPropertyDescription, ArkCmdArgumentsDescAttribute cmdArgs)
            {
                var args = fullPropertyDescription.Parameters.ToList();
                args.Add(cmdArgs.Parameters.ToList());

                var report = new FullPropertyDescription(
                    fullName: fullPropertyDescription.FullName,
                    shortName: fullPropertyDescription.ShortName,
                    isRequire: fullPropertyDescription.IsRequire,
                    propertyInfo: fullPropertyDescription.PropertyInfo,
                    parameters: args);



                return report;
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            internal bool IsRequire => _isRequire;

            //--------------------------------------------------------------------------------------------------------------------------------------

            internal string ShortName => _shortName;

            //--------------------------------------------------------------------------------------------------------------------------------------

            internal string FullName => _fullName;

            //--------------------------------------------------------------------------------------------------------------------------------------

            public PropertyInfo PropertyInfo => _propertyInfo;

            //--------------------------------------------------------------------------------------------------------------------------------------

            internal IEnumerable<List<string>> Parameters
            {
                get
                {
                    foreach (var list in _parameters.Values)
                    {
                        yield return list;
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private class TableContent<TTarget> : IDictionary<string, FullPropertyDescription> where TTarget : new()
        {
            private readonly TTarget _target;
            private readonly Dictionary<string, FullPropertyDescription> _tableContent;

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

            public TableContent()
            {
                _target = new TTarget();
                _tableContent = new Dictionary<string, FullPropertyDescription>();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------


            public FullPropertyDescription this[string key]
            {
                get
                {
                    return _tableContent[key];
                }

                set
                {
                    _tableContent[key] = value;
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public void Add(string key, FullPropertyDescription value)
            {
                _tableContent.Add(key, value);
            }

            public void SetPropertyWithValue(string propName, string value)
            {
                if (_tableContent[propName].PropertyInfo.PropertyType == typeof(char))
                {
                    _tableContent[propName].PropertyInfo.SetValue(_target, value.ElementAt(0));
                }

                if (_tableContent[propName].PropertyInfo.PropertyType == typeof (int))
                {
                    _tableContent[propName].PropertyInfo.SetValue(_target, int.Parse(value));
                }

                if (_tableContent[propName].PropertyInfo.PropertyType == typeof(string))
                {
                    _tableContent[propName].PropertyInfo.SetValue(_target, value);
                }


            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public int Count
            {
                get
                {
                    return _tableContent.Count();
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public ICollection<string> Keys
            {
                get
                {
                    return _tableContent.Keys;
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public ICollection<FullPropertyDescription> Values
            {
                get
                {
                    return _tableContent.Values;
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public void Add(KeyValuePair<string, FullPropertyDescription> item)
            {
                throw new NotImplementedException();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            //--------------------------------------------------------------------------------------------------------------------------------------

            public void Clear()
            {
                _tableContent.Clear();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool Contains(KeyValuePair<string, FullPropertyDescription> item)
            {
                return _tableContent.Contains(item);
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool ContainsKey(string key)
            {
                return _tableContent.ContainsKey(key);
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public void CopyTo(KeyValuePair<string, FullPropertyDescription>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public IEnumerator<KeyValuePair<string, FullPropertyDescription>> GetEnumerator()
            {
                return _tableContent.GetEnumerator();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool Remove(KeyValuePair<string, FullPropertyDescription> item)
            {
                throw new NotImplementedException();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            public bool TryGetValue(string key, out FullPropertyDescription value)
            {
                return _tableContent.TryGetValue(key, out value);
            }

            //--------------------------------------------------------------------------------------------------------------------------------------

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _tableContent.GetEnumerator();
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
            //--------------------------------------------------------------------------------------------------------------------------------------

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private List<string> _errors;

        private TableContent<TTargetClass> _tableContent;

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        public ArkParser()
        {
            _errors = new List<string>();

        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        public ParserResult Parse(string[] arguments, string delimiter)
        {

            _tableContent = new TableContent<TTargetClass>();



            var result = CreateTableContent(arguments);

            if (result.IsSucceeded == false)
            {
                return result;
            }

            result = CreateInstance(arguments, delimiter);

            return result;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        private ParserResult CreateTableContent(string[] arguments)
        {
            //TODO multi exception not solely single.
            foreach (var property in typeof(TTargetClass).GetProperties())
            {
                var cmdDescription = property.GetCustomAttributes<ArkCmdDescAttribute>().Single();
                //TODO

                try
                {
                    var propertyDescription = new FullPropertyDescription(cmdDescription, propertyInfo: property);
                    _tableContent.Add(cmdDescription.FullName, propertyDescription);
                    if (string.IsNullOrEmpty(cmdDescription.ShortName) == false)
                    {
                        _tableContent.Add(cmdDescription.ShortName, propertyDescription);
                    }
                }
                catch (ArgumentException argumentException)
                {
                    return new ParserResult(isSucceeded: false, exception: new PropertyNameDuplicationException("stam"));
                }
                catch (Exception exceprtion)
                {
                    //TODO
                    int x = 0;
                }

                foreach (var cmdArgs in property.GetCustomAttributes<ArkCmdArgumentsDescAttribute>())
                {
                    _tableContent[cmdDescription.FullName] += cmdArgs;
                }


            }

            return new ParserResult(isSucceeded: true, exception: null);
        }

        //--------------------------------------------------------------------------------------------------------------------------------------

        private ParserResult CreateInstance(IReadOnlyList<string> mainArguments, string delimiter)
        {
            var arguments = mainArguments.Select(x => x.Trim()).Select(x => x).ToList();
            var anticipating = 0; // 0 - init 1-cmd  2-regular without params 3-with params
            var nameProperty = string.Empty;

            for (int i = 0; i < arguments.Count;)
            {
                if (arguments[i].Equals(delimiter))
                {
                    i++;
                    anticipating = 1;
                    continue;
                }

                if (arguments[i][0].ToString() == delimiter)
                {
                    arguments[i] = arguments[i].Remove(0, 1);
                    anticipating = 1;
                    continue;
                }

                if (anticipating == 1)
                {
                    nameProperty = arguments[i];
                    anticipating = _tableContent[arguments[i++]].Parameters.Any() == false ? 2 : 3;
                    continue;
                }

                if (anticipating == 2)
                {
                    _tableContent.SetPropertyWithValue(nameProperty, arguments[i++]);
                    anticipating = 1;
                    continue;
                }

                if (anticipating == 3)
                {
                    //TODO
                    throw new NotImplementedException();
                }

            }




            return new ParserResult(isSucceeded: true, exception: null);
        }
    }
}
