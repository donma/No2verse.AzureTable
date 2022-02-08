using No2verse.AzureTable.Collections;

namespace No2verse.AzureTable.Base
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAzureTableOperator
    {
        string TableName { get; }
        bool IsAutoGCOnDtor { get; set; }
        AzureTableRole Role { get; }

        bool Delete(string partitionKey, string rowKey);

        bool DeleteByPK(string partitionKey);

        bool DeleteByRK(string rowKey);
        string Insert(DTableEntity data);
        bool IsDataExist(string partitionKey, string rowKey);
        Operator Update(DTableEntity data);
        Operator UpdateWithEtag(DTableEntity data);
        bool DeleteWithEtag(string partitionKey, string rowKey, string eTag);


    }

}
