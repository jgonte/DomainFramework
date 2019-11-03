using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.DataAccess
{
    public class ODataOrderByBuilder
    {
        private ODataOrderByTokenizer _tokenizer;

        private ODataOrderByBuilderStatuses _status = ODataOrderByBuilderStatuses.Initial;

        private Queue<SorterNode> _sorters = new Queue<SorterNode>(); // The current sorter being built

        /// <summary>
        /// The current sorter node being built
        /// </summary>
        private SorterNode _sorterNode;

        public Queue<SorterNode> Build(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            _tokenizer = new ODataOrderByTokenizer(query.TrimEnd().ToCharArray());

            while (!_tokenizer.End())
            {
                var (tokenType, token) = _tokenizer.GetToken();

                switch (tokenType)
                {
                    case ODataOrderByTokenizerTokenTypes.FieldName:
                        {
                            if (_status == ODataOrderByBuilderStatuses.Initial)
                            {
                                _status = ODataOrderByBuilderStatuses.BuildingSorter;

                                _sorterNode = new SorterNode
                                {
                                    FieldName = token
                                };
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataOrderByTokenizerTokenTypes.SortOrder:
                        {
                            if (_status == ODataOrderByBuilderStatuses.BuildingSorter)
                            {
                                _sorterNode.SortingOrder = token == "desc" ? 
                                    SorterNode.SortingOrders.Descending :
                                    SorterNode.SortingOrders.Ascending;

                                _sorters.Enqueue(_sorterNode);

                                _status = ODataOrderByBuilderStatuses.Initial;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                        break;
                    case ODataOrderByTokenizerTokenTypes.Comma:
                        {
                            // Ignore
                        }
                        break;
                    default: throw new NotImplementedException();
                }
            }

            return _sorters;
        }
    }
}