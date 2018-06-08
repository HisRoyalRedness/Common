using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace HisRoyalRedness.com
{
    public class CommandLineParser
    {
        // https://stackoverflow.com/questions/156767/whats-the-difference-between-an-argument-and-a-parameter
        // A parameter is a variable in a method definition.
        // When a method is called, the arguments are the data 
        // you pass into the method's parameters.

        public CommandLineParser()
            : this(false, Environment.GetCommandLineArgs(), DEFAULT_SWITCH_PREFIXES)
        { }

        public CommandLineParser(bool caseSensitive)
            : this(caseSensitive, Environment.GetCommandLineArgs(), DEFAULT_SWITCH_PREFIXES)
        { }

        public CommandLineParser(params string[] switchPrefixes)
            : this(false, Environment.GetCommandLineArgs(), switchPrefixes)
        { }

        public CommandLineParser(bool caseSensitive, string[] args, params string[] switchPrefixes)
        {
            _args = args;
            _switchPrefixes = switchPrefixes;
            _comparer = caseSensitive
                ? StringComparer.CurrentCulture
                : StringComparer.CurrentCultureIgnoreCase;

            _switches = new Dictionary<string, bool>(_comparer);
            _namedArgs = new Dictionary<string, Tuple<bool, Stack<string>>>(_comparer);
            _unnamedArgs = new List<string>();
            Parse();
        }

        #region Switches
        public IEnumerable<string> Switches => _switches.Keys;
        public IEnumerable<string> UnusedSwitches => _switches.Where(s => !s.Value).Select(s => s.Key);
        public bool HasSwitch(string swtch) => _switches.ContainsKey(swtch);
        public CommandLineParser HasSwitch(string swtch, Action<string> hasSwitchAction)
        {
            if (HasSwitch(swtch))
            {
                hasSwitchAction(swtch);
                _switches[swtch] = true;
            }
            return this;
        }
        public CommandLineParser MustHaveSwitch(string swtch, Action<string> hasSwitchAction)
        {
            if (!HasSwitch(swtch))
                throw ParameterException.MissingParameter(swtch, ParameterException.ParameterTypes.Switch);
            hasSwitchAction(swtch);
            _switches[swtch] = true;
            return this;
        }

        public CommandLineParser ThrowOnUnusedSwitches()
        {
            if (UnusedSwitches.Any())
            {
                var unused = string.Join(", ", UnusedSwitches);
                throw new UnusedParameterException($"One or more switches weren't used: {unused}", unused);
            }
            return this;
        }
        #endregion Switches

        #region Named arguments
        public IEnumerable<string> NamedArg => _namedArgs.Keys;
        public IEnumerable<string> UnusedNamedArgs => _namedArgs.Where(s => !s.Value.Item1).Select(s => s.Key);
        public bool HasNamedArg(string name) => _namedArgs.ContainsKey(name);
        public CommandLineParser HasNamedArg(string name, Action<string> hasNamedArgAction)
        {
            if (HasNamedArg(name))
            {
                hasNamedArgAction(name);
                if (!_namedArgs[name].Item1)
                    _namedArgs[name] = new Tuple<bool, Stack<string>>(true, _namedArgs[name].Item2);
            }
            return this;
        }
        public CommandLineParser MustHaveNamedArg(string name, Action<string> hasNamedArgAction)
        {
            if (!HasNamedArg(name))
                throw ParameterException.MissingParameter(name, ParameterException.ParameterTypes.NamedParameter);
            hasNamedArgAction(name);
            if (!_namedArgs[name].Item1)
                _namedArgs[name] = new Tuple<bool, Stack<string>>(true, _namedArgs[name].Item2);
            return this;
        }

        public CommandLineParser ThrowOnUnusedNamedParameters()
        {
            if (UnusedNamedArgs.Any())
            {
                var unused = string.Join(", ", UnusedNamedArgs);
                throw new UnusedParameterException($"One or more named arguments weren't used: {unused}", unused);
            }
            return this;
        }
        #endregion Named arguments



        void Parse()
        {
            foreach (var arg in _args)
            {
                // Switches
                var nextArg = false;
                foreach (var prefix in _switchPrefixes)
                {
                    if (arg.StartsWith(prefix))
                    {
                        var swtch = arg.Substring(prefix.Length).Trim();
                        if (!_switches.ContainsKey(swtch))
                            _switches.Add(swtch, false);
                        nextArg = true;
                    }
                }
                if (nextArg)
                    continue;

                // Named parameters
                if (arg.Contains("="))
                {
                    var ind = arg.IndexOf("=");
                    var name = ind > 0
                        ? arg.Substring(0, ind).Trim()
                        : "";
                    var value = arg.Length > ind + 1 ? arg.Substring(ind + 1).Trim() : "";
                    if (!_namedArgs.ContainsKey(name))
                        _namedArgs.Add(name, new Tuple<bool, Stack<string>>(false, new Stack<string>()));
                    _namedArgs[name].Item2.Push(value);
                    continue;
                }

                // Unnamed parameters
                _unnamedArgs.Add(arg);
            }
        }

        internal static readonly string[] DEFAULT_SWITCH_PREFIXES = new[] { "-", "/", "\\" };

        readonly string[] _switchPrefixes;
        readonly string[] _args;
        readonly Dictionary<string, bool> _switches;
        readonly Dictionary<string, Tuple<bool, Stack<string>>> _namedArgs;
        readonly List<string> _unnamedArgs;
        readonly StringComparer _comparer;
    }

    public class UnusedParameterException : ApplicationException
    {
        public UnusedParameterException(string message, string unusedParameters)
            : base(message)
        {
            UnusedParameters = unusedParameters;
        }

        public string UnusedParameters { get; private set; }
    }

    public class ParameterException : ApplicationException
    {
        public ParameterException(string parameterName, ParameterTypes parameterType, string message)
            : base(message)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
        }

        public static ParameterException MissingParameter(string parameterName, ParameterTypes parameterType)
            => new ParameterException(parameterName, parameterType, $"{parameterType} {parameterName} is missing.");

        public string ParameterName { get; private set; }
        public ParameterTypes ParameterType { get; private set; }

        public enum ParameterTypes
        {
            Switch,
            NamedParameter,
            UnnamedParameter
        }
    }
}
