namespace DomainFramework.Core
{
    public static class ISingleValueExtensions
    {
        public static string GetFieldValue(this ISingleValue holder) => holder.FieldValue is string ?
            $"'{holder.FieldValue.ToString()}'" :
            holder.FieldValue.ToString();
    }
}
