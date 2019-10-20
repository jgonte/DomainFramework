﻿namespace DomainFramework.DataAccess
{
    public enum ODataFilterTokenizerTokenTypes
    {
        OpeningParenthesis,
        ClosingParenthesis,
        StringLiteral,
        NumericValue,
        BooleanValue,
        FunctionName,
        LogicalOperator,
        ComparisonOperator,
        FieldName,
        Comma
    }
}