using System;
using MWS.Data;

namespace MWS.Helper
{
    public static class DBHelper
    {
        public static string ToString(object dbResult)
        {
            if (dbResult is DBNull)
            {
                return "";
            }
            if (dbResult is DateTime)
            {
                return DateTimeHelper.FormatDate((DateTime)dbResult);
            }
                return dbResult.ToString();
            }

        public static object ToNull(object dbResult) => dbResult is DBNull ? null : dbResult;

        public static string FormatTableName(string tableName)
        {
            DBConnector c = DBConnector.GetConnector();
            if ("EXCEL".Equals(c.GetDataSource()))
            {
                return tableName + "$";
            }

            return tableName;
        }
    }
}
