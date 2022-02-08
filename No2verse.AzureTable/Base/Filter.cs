namespace No2verse.AzureTable.Base
{
    /// <summary>
    /// 
    /// </summary>
    public enum CompareFilter
    {
        /// <summary>
        /// &lt;
        /// </summary>
        Less,
        /// <summary>
        /// &lt;=
        /// </summary>
        LessEqual,
        /// <summary>
        /// =
        /// </summary>
        Eqaul,
        /// <summary>
        /// &gt;
        /// </summary>
        Greater,
        /// <summary>
        /// &lt;=
        /// </summary>
        GreaterEqual


    }

    /// <summary>
    /// 
    /// </summary>
    public static class FilteHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFilter"></param>
        /// <returns></returns>
        public static string ConvertCompareFilterString(this CompareFilter dateFilter)
        {
            if (dateFilter == CompareFilter.Eqaul) return "=";
            if (dateFilter == CompareFilter.Greater) return ">";
            if (dateFilter == CompareFilter.GreaterEqual) return ">=";
            if (dateFilter == CompareFilter.Less) return "<";
            if (dateFilter == CompareFilter.LessEqual) return "<=";

            return "";
        }



    }

}
