using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;

namespace No2verse.AzureTable.Core
{
    internal class AzureTableDataReader
    {
        internal static   bool IsDataExisted(string pk, string rk, ref CloudTable cloudTable)
        {

            string pkFilter = new TableQuery<TableEntity>()
              .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                QueryComparisons.Equal, pk)).FilterString;

            string rkFilter = new TableQuery<TableEntity>()
           .Where(TableQuery.GenerateFilterCondition("RowKey",
             QueryComparisons.Equal, rk)).FilterString;


            string combineAgeFilter = TableQuery.CombineFilters(pkFilter, TableOperators.And, rkFilter);
            string combinePKAndAgeFilter = TableQuery.CombineFilters(pkFilter, TableOperators.And, combineAgeFilter);

            TableQuery<TableEntity> query = new TableQuery<TableEntity>().Where(combinePKAndAgeFilter);


            query.SelectColumns = new List<string>();
            query.SelectColumns.Add("ETag");
            var entities = cloudTable.ExecuteQuery<TableEntity>(query);

            return entities.Count() > 0;

        }

    }
}
