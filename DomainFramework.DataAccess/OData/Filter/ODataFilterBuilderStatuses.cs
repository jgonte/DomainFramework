namespace DomainFramework.DataAccess
{
    public enum ODataFilterBuilderStatuses
    {
        Initial,
        BuildingFieldFilter,
        BuildingFunctionCall,
        BuildingFunctionCallParameters,
        BuildingMultiValueOperator,
        BuildingMultiValueOperatorValues
    }
}