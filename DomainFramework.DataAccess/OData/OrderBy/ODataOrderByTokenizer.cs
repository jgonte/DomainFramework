using System;
using System.Linq;
using Utilities;

namespace DomainFramework.DataAccess
{
    public class ODataOrderByTokenizer : Tokenizer
    {
        private static readonly string[] _sortOrders = new string[]
        {
            "asc",
            "desc"
        };

        public ODataOrderByTokenizer(char[] buffer) : base(buffer)
        {
        }

        public (ODataOrderByTokenizerTokenTypes, string) GetToken()
        {
            SkipSpaces();

            switch (_char)
            {
                case ',':
                    {
                        ReadChar();

                        return (ODataOrderByTokenizerTokenTypes.Comma, ",");
                    }
                default:
                    {
                        return GetNameToken();
                    }
            }
        }

        private (ODataOrderByTokenizerTokenTypes, string) GetNameToken()
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
            if (_sortOrders.Contains(value.ToLowerInvariant()))
            {
                return (ODataOrderByTokenizerTokenTypes.SortOrder, value.ToLowerInvariant());
            }
            else
            {
                return (ODataOrderByTokenizerTokenTypes.FieldName, value);
            }
        }
    }
}
