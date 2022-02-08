using System.Collections.Generic;

namespace No2verse.AzureTable.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAzureTableQuery<T>
    {
        string TableName { get; }
        bool IsAutoGCOnDtor { get; set; }
        AzureTableRole Role { get; }

        IEnumerable<T> All();
        string[] AllPartitionKeys();
        string[] AllRowKeysByPartitionKey(string partitionKey);
        T[] AllDatas();
        List<T> AllDatasList();

        T DataByPRKey(string partitionKey, string rowKey);

        List<T> DatasByPartitionKey(string partitionKey);
        List<T> DatasByRowKey(string rowKey);
        int DataCount();
        int DataCountByPartitionKey(string partitionKey);

        List<T> DataListByKeys(params PRPair[] prpairs);

        bool IsDataExist(string partitionKey, string rowKey);

    }
}
