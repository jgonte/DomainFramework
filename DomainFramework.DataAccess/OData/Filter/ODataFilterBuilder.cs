using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.DataAccess
{
    public class ODataFilterBuilder
    {
        private ODataFilterTokenizer _tokenizer;

        private ODataFilterBuilderStatuses _status = ODataFilterBuilderStatuses.Initial;

        private Queue<FilterNode> _filter = new Queue<FilterNode>(); // The current filter being built

        /// <summary>
        /// Stores the current field name to be used when populating the node
        /// </summary>
        private string _fieldName;

        /// <summary>
        /// The current filter node being built
        /// </summary>
        private FilterNode _filterNode;

        private static readonly string[] _singleFieldNodeOperators = new string[]
            {
                "eq",
                "ne"
            };

        public Queue<FilterNode> Build(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            _tokenizer = new ODataFilterTokenizer(query.TrimEnd().ToCharArray());

            while (!_tokenizer.End())
            {
                var (tokenType, token) = _tokenizer.GetToken();

                switch (tokenType)
                {
                    case ODataFilterTokenizerTokenTypes.OpeningParenthesis:
                        {
                            // Function calls have parenthesis so ignore them
                            if (_status == ODataFilterBuilderStatuses.Initial)
                            {
                                _filter.Enqueue(FilterNode.BeginGrouping);
                            }
                            else if (_status == ODataFilterBuilderStatuses.BuildingFunctionCall)
                            {
                                _status = ODataFilterBuilderStatuses.BuildingFunctionCallParameters;
                            }
                            else if (_status == ODataFilterBuilderStatuses.BuildingMultiValueOperator)
                            {
                                _status = ODataFilterBuilderStatuses.BuildingMultiValueOperatorValues;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.ClosingParenthesis:
                        {
                            // Function calls have parenthesis so ignore them
                            if (_status == ODataFilterBuilderStatuses.Initial)
                            {
                                _filter.Enqueue(FilterNode.EndGrouping);
                            }
                            else if (_status == ODataFilterBuilderStatuses.BuildingFunctionCallParameters)
                            {
                                _filter.Enqueue(_filterNode);

                                _status = ODataFilterBuilderStatuses.Initial;
                            }
                            else if (_status == ODataFilterBuilderStatuses.BuildingMultiValueOperatorValues)
                            {
                                _filter.Enqueue(_filterNode);

                                _status = ODataFilterBuilderStatuses.Initial;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.LogicalOperator:
                        {
                            switch (token)
                            {
                                case "not":
                                    {
                                        _filter.Enqueue(FilterNode.Not);
                                    }
                                    break;
                                case "and":
                                    {
                                        _filter.Enqueue(FilterNode.And);
                                    }
                                    break;
                                case "or":
                                    {
                                        _filter.Enqueue(FilterNode.Or);
                                    }
                                    break;
                                default: throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.FieldName:
                        {
                            if (_status == ODataFilterBuilderStatuses.Initial)
                            {
                                _status = ODataFilterBuilderStatuses.BuildingFieldFilter;

                                _fieldName = token;
                            }
                            else if (_status == ODataFilterBuilderStatuses.BuildingFunctionCallParameters)
                            {
                                ((TwoParametersFunctionCallFilterNode)_filterNode).FieldName = token;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.ComparisonOperator:
                        {
                            if (_status == ODataFilterBuilderStatuses.BuildingFieldFilter)
                            {
                                if (IsSingleValueFieldFilterNode(token))
                                {
                                    _filterNode = new SingleValueFieldFilterNode
                                    {
                                        FieldName = _fieldName,
                                        Operator = GetSingleValueFieldFilterOperator(token)
                                    };
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }

                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.FunctionName:
                        {
                            if (_status == ODataFilterBuilderStatuses.Initial)
                            {
                                _status = ODataFilterBuilderStatuses.BuildingFunctionCall;

                                switch (token)
                                {
                                    case "contains":
                                        {
                                            _filterNode = new ContainsFunctionCallFilterNode();
                                        }
                                        break;
                                    default: throw new NotImplementedException();
                                }
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.MultiValueOperator:
                        {
                            if (_status == ODataFilterBuilderStatuses.BuildingFieldFilter)
                            {

                                _status = ODataFilterBuilderStatuses.BuildingMultiValueOperator;

                                switch (token)
                                {
                                    case "in":
                                        {
                                            _filterNode = new InOperatorFilterNode
                                            {
                                                FieldName = _fieldName
                                            };
                                        }
                                        break;
                                    default: throw new NotImplementedException();
                                }
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.StringLiteral:
                        {
                            SetFieldValue(token);
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.NumericValue:
                        {
                            SetFieldValue(int.Parse(token));
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.BooleanValue:
                        {
                            SetFieldValue(bool.Parse(token));
                        }
                        break;
                    case ODataFilterTokenizerTokenTypes.Comma:
                        {
                            // Ignore
                        }
                        break;
                    default: throw new NotImplementedException();
                }
            }

            return _filter;
        }

        private bool IsSingleValueFieldFilterNode(string token) => _singleFieldNodeOperators.Contains(token);

        private FilterNode GetSingleValueFieldFilterOperator(string token)
        {
            switch (token)
            {
                case "eq": return FilterNode.IsEqual;
                case "ne": return FilterNode.IsNotEqual;
                default: throw new NotImplementedException();
            }
        }

        private void SetFieldValue(object value)
        {
            if (_status == ODataFilterBuilderStatuses.BuildingFieldFilter)
            {
                if (_filterNode is ISingleValue)
                {
                    ((ISingleValue)_filterNode).FieldValue = value;
                }
                else
                {
                    throw new NotImplementedException();
                }

                _filter.Enqueue(_filterNode);

                _status = ODataFilterBuilderStatuses.Initial;
            }
            else if (_status == ODataFilterBuilderStatuses.BuildingFunctionCallParameters)
            {
                if (_filterNode is ISingleValue)
                {
                    ((ISingleValue)_filterNode).FieldValue = value;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (_status == ODataFilterBuilderStatuses.BuildingMultiValueOperatorValues)
            {
                if (_filterNode is IMultiValue)
                {
                    ((IMultiValue)_filterNode).FieldValues.Add(value);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}