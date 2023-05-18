using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

public static class FakeEfCoreSqlExtensions
{
    /// <summary>
    /// Sql查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="facade">Database</param>
    /// <param name="sql">sql语句</param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IEnumerable<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters)
        where T : class, new()
    {
        DataTable dt = SqlQuery(facade, sql, parameters);
        return dt.ToEnumerable<T>();
    }

    private static IEnumerable<T> ToEnumerable<T>(this DataTable dt) where T : class, new()
    {
        PropertyInfo[] propertyInfos = typeof(T).GetProperties();
        T[] ts = new T[dt.Rows.Count];
        int i = 0;
        foreach (DataRow row in dt.Rows)
        {
            T t = new T();
            foreach (PropertyInfo p in propertyInfos)
            {
                if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                    p.SetValue(t, row[p.Name], null);
            }

            ts[i] = t;
            i++;
        }

        return ts;
    }

    public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
    {
        DbCommand cmd = CreateCommand(facade, sql, out DbConnection conn, parameters);
        DbDataReader reader = cmd.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        reader.Close();
        conn.Close();
        return dt;
    }

    private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection dbConn,
        params object[] parameters)
    {
        DbConnection conn = facade.GetDbConnection();
        dbConn = conn;
        conn.Open();
        DbCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        foreach (var parameter in parameters)
        {
            cmd.Parameters.Add(parameter);
        }

        return cmd;
    }
}