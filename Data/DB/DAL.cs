using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using MWS.Helper;

namespace MWS.Data
{
    public class DAL
    {
        public readonly DBConnector _connector;

        protected DAL()
        {
            _connector = DBConnector.GetConnector();
        }

        public void Disconnect()
        {
            _connector.Disconnect();
        }

        protected double FetchDouble(string sql)
        {
            try
            {
                return Convert.ToDouble(_connector.ExecuteScalar(sql));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected int FetchInteger(string sql)
        {
            try
            {
                return Convert.ToInt32(_connector.ExecuteScalar(sql));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected string FetchString(string sql)
        {
            try
            {
                return Convert.ToString(_connector.ExecuteScalar(sql));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected object FetchScalar(string sql)
        {
            try
            {
                return _connector.ExecuteScalar(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected Dictionary<string, string>[] FetchData(string sql)
        {
            try
            {
                IDataReader reader = _connector.ExecuteReader(sql);

                List<Dictionary<string, string>> entryList = new List<Dictionary<string, string>>();

                while (reader.Read())
                {
                    Dictionary<string, string> entry = new Dictionary<string, string>();

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        entry.Add(reader.GetName(index), DBHelper.ToString(reader[reader.GetName(index)]));
                    }

                    entryList.Add(entry);
                }

                reader.Close();
                return entryList.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected string[][] FetchArray(string sql)
        {
            try
            {
                IDataReader reader = _connector.ExecuteReader(sql);
                List<string[]> entryList = new List<string[]>();

                while (reader.Read())
                {
                    string[] entry = new string[reader.FieldCount];

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        entry[index] = DBHelper.ToString(reader[index]);
                    }

                    entryList.Add(entry);
                }

                reader.Close();
                return entryList.ToArray();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected ArrayList FetchArrayList(string sql)
        {
            try
            {
                IDataReader reader = _connector.ExecuteReader(sql);
                ArrayList entryList = new ArrayList();

                while (reader.Read())
                {
                    entryList.Add(DBHelper.ToString(reader[0]));
                }

                reader.Close();
                return entryList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected Dictionary<string, string> FetchSingle(string sql)
        {
            try
            {
                IDataReader reader = _connector.ExecuteReader(sql);
                Dictionary<string, string> entry = null;

                if (reader.Read())
                {
                    entry = new Dictionary<string, string>();

                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        entry.Add(reader.GetName(index), DBHelper.ToString(reader[reader.GetName(index)]));
                    }
                }

                reader.Close();
                return entry;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected object Insert(string sql)
        {
            try
            {
                _connector.ExecuteNonQuery(sql);
                return _connector.ExecuteScalar("SELECT @@IDENTITY");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected object Update(string sql)
        {
            try
            {
                return _connector.ExecuteNonQuery(sql);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected void BeginTransaction()
        {
            _connector.BeginTransaction();
        }

        protected void CommitTransaction()
        {
            _connector.CommitTransaction();
        }

        protected void RollbackTransaction()
        {
            _connector.RollbackTransaction();
        }
    }
}
