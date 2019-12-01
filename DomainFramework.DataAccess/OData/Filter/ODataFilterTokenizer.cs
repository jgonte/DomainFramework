using System;
using System.Linq;
using Utilities;

namespace DomainFramework.DataAccess
{
    public class ODataFilterTokenizer : Tokenizer
    {
        private static readonly char[] _boleanTypesStartingChar = new char[] { 'f', 'F', 't', 'T' };

        private static readonly string[] _logicalOperators = new string[]
        {
            "and",
            "or",
            "not"
        };

        private static readonly string[] _comparisonOperators = new string[]
        {
            "eq",
            "ne",
            "gt",
            "ge",
            "lt",
            "le",
            "has"
        };

        private static readonly string[] _multiValueOperators = new string[]
        {
            "in"
        };

        public ODataFilterTokenizer(char[] buffer) : base(buffer)
        {
        }

        public (ODataFilterTokenizerTokenTypes, string) GetToken()
        {
            SkipSpaces();

            switch (_char)
            {
                case '(':
                    {
                        ReadChar();

                        return (ODataFilterTokenizerTokenTypes.OpeningParenthesis, "(");
                    }
                case ')':
                    {
                        ReadChar();

                        return (ODataFilterTokenizerTokenTypes.ClosingParenthesis, ")");
                    }
                case ',':
                    {
                        ReadChar();

                        return (ODataFilterTokenizerTokenTypes.Comma, ",");
                    }
                case '\'':
                    {
                        ReadChar();

                        return (ODataFilterTokenizerTokenTypes.StringLiteral, GetStringLiteral());
                    }
                default:
                    {
                        string booleanValue;

                        if (char.IsNumber(_char))
                        {
                            return (ODataFilterTokenizerTokenTypes.NumericValue, GetNumericValue());
                        }
                        else if (GetBooleanValue(out booleanValue))
                        {
                            return (ODataFilterTokenizerTokenTypes.BooleanValue, booleanValue);
                        }
                        else // It can be either a field name, method name, operator name
                        {
                            return GetNameToken();
                        }
                    }
            }
        }

        private string GetStringLiteral()
        {
            var offset = _offset;

            var end = _offset + 1; // Point to next charater after the first one

            for (; end < _buffer.Length; ++end)
            {
                if (_buffer[end] == '\'')
                {
                    break;
                }
            }

            var value = GetValue(end); // Go one before the closing quote

            _offset = end; // Update the offset

            if (End())
            {
                throw new InvalidOperationException($"Unterminated string literal starting at position: {offset}");
            }

            ReadChar();

            return value;
        }

        private string GetNumericValue()
        {
            var end = _offset + 1;

            for (; end < _buffer.Length; ++end)
            {
                var c = _buffer[end];

                if (!char.IsNumber(c)) // || c != '0x' // TODO: Add support for hexadecimal other numeric types
                {
                    break;
                }
            }

            var value = GetValue(end);

            _offset = end - 1;

            ReadChar();

            return value;
        }

        private bool GetBooleanValue(out string booleanValue)
        {
            if (!_boleanTypesStartingChar.Contains(_char))
            {
                booleanValue = null;

                return false;
            }

            // Test for true
            var end = _offset + 4;

            if (end <= _buffer.Length)
            {
                var value = GetValue(end);

                if (value.ToLowerInvariant() == "true")
                {
                    _offset = end - 1; // Update the offset

                    ReadChar();

                    booleanValue = "true";

                    return true;
                }
            }

            // Test for false
            end = _offset + 5;

            if (end <= _buffer.Length)
            {
                var value = GetValue(end);

                if (value.ToLowerInvariant() == "false")
                {
                    _offset = end - 1; // Update the offset

                    ReadChar();

                    booleanValue = "false";

                    return true;
                }
            }

            // Neither
            booleanValue = null;

            return false;
        }

        private (ODataFilterTokenizerTokenTypes, string) GetNameToken()
        {
            var end = _offset + 1;

            for (; end < _buffer.Length; ++end)
            {
                var c = _buffer[end];

                if (!char.IsLetterOrDigit(c))
                {
                    break;
                }
            }

            var value = GetValue(end);

            _offset = end - 1;

            ReadChar();

            // Got the name as a value. Now determine whether it is a function name or a field name or an operator
            if (_logicalOperators.Contains(value.ToLowerInvariant()))
            {
                return (ODataFilterTokenizerTokenTypes.LogicalOperator, value.ToLowerInvariant());
            }
            else if (_comparisonOperators.Contains(value.ToLowerInvariant()))
            {
                return (ODataFilterTokenizerTokenTypes.ComparisonOperator, value.ToLowerInvariant());
            }
            else if (_multiValueOperators.Contains(value.ToLowerInvariant()))
            {
                return (ODataFilterTokenizerTokenTypes.MultiValueOperator, value.ToLowerInvariant());
            }
            else if (FollowsWithOpeningParenthesis())
            {
                return (ODataFilterTokenizerTokenTypes.FunctionName, value.ToLowerInvariant());
            }
            else
            {
                return (ODataFilterTokenizerTokenTypes.FieldName, value);
            }
        }

        private bool FollowsWithOpeningParenthesis()
        {
            var end = _offset;

            for (; end < _buffer.Length; ++end)
            {
                var c = _buffer[end];

                if (char.IsLetterOrDigit(c)) // Not blanks
                {
                    return false;
                }
                else if (c == '(')
                {
                    return true;
                }
            }

            return false;
        }

    }
}
