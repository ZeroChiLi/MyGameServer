using System;
using System.Data.SqlClient;
using System.Text;
using MyGameServer.Model;
using System.Collections.Generic;

namespace MyGameServer.Cache
{
    static class SqlCache
    {
        //数据库连接
        static public SqlConnection SqlConnection;

        //静态构造初始化
        static SqlCache() { ConnectSql(); }

        //连接数据库
        static private void ConnectSql()
        {
            //视图->服务器资源管理器->数据连接->数据库右键->属性->连接->连接字符串
            const string constr = "Data Source=PC-LCL\\SQLEXPRESS;Initial Catalog=TestDb;Integrated Security=True;Pooling=False";
            SqlConnection = new SqlConnection(constr);
            MyApplication.Log("连接数据库");
        }

        //打开连接数据库
        static public bool OpenSql()
        {
            try
            {
                //关闭的时候才开，要不然会抛异常
                if (SqlConnection.State == System.Data.ConnectionState.Closed)
                    SqlConnection.Open();
                MyApplication.Log("成功打开数据库");
                return true;
            }
            catch (Exception e) { MyApplication.Log("打开数据库 失败" + e.ToString()); }
            return false;
        }

        //关闭连接数据库
        static public void CloseSql()
        {
            try
            {
                if (SqlConnection.State == System.Data.ConnectionState.Open)
                    SqlConnection.Close();
                MyApplication.Log("成功关闭数据库");
            }
            catch (Exception e) { MyApplication.Log("关闭数据库 失败" + e.ToString()); }
        }

        //是否存在帐号或帐号密码匹配，写在同一个函数因为其实就差多个检查密码
        static public bool IsMatchAccount(string accountName, string password = null)
        {
            if (!OpenSql())
                return false;

            bool isMatch = false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append(" Select * From Account");
            commandStr.Append(" Where AccountName = '" + accountName + "'");

            if (!string.IsNullOrEmpty(password))                        //加了密码就是检测帐号密码同时匹配
                commandStr.Append(" And Password = '" + password + "'");

            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandStr.ToString(), SqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                    isMatch = true;
                MyApplication.Log("查询用户存在、或账户密码匹配成功");
            }
            catch (Exception e) { MyApplication.Log("查询用户存在 失败了啊 " + e.ToString()); }
            finally { CloseSql(); }

            return isMatch;
        }

        //添加用户
        static public bool AddAccount(string accountName, string password)
        {
            if (IsMatchAccount(accountName) || !OpenSql())          //已存在该帐号
                return false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append(" Insert Into Account(AccountName, Password) Values('" + accountName + "','" + password + "')");

            SqlCommand sqlCommand = new SqlCommand(commandStr.ToString(), SqlConnection);
            try
            {
                sqlCommand.ExecuteScalar();
                MyApplication.Log(string.Format("成功添加用户 {0} ,密码 {1}", accountName, password));
            }
            catch (Exception e) { MyApplication.Log("添加用户 失败了啊" + e.ToString()); }
            finally { CloseSql(); }

            return true;
        }

        //删除用户
        static public bool DeleteAccount(string accountName)
        {
            if (!IsMatchAccount(accountName) || !OpenSql())         //不存在该账户
                return false;

            StringBuilder commandStr = new StringBuilder();
            commandStr.Append("DELETE FROM Account WHERE AccountName = '" + accountName + "'");

            SqlCommand sqlCommand = new SqlCommand(commandStr.ToString(), SqlConnection);
            try
            {
                sqlCommand.ExecuteScalar();
                MyApplication.Log(string.Format("删除用户 {0}", accountName));
            }
            catch (Exception e) { MyApplication.Log("删除用户 失败了啊" + e.ToString()); }
            finally { CloseSql(); }

            return true;
        }

        //获取用户列表
        static public List<AccountModel> GetAccountList()
        {
            List<AccountModel> accountList = new List<AccountModel>();

            if (!OpenSql())                 //打开数据库失败返回空的用户列表。
                return accountList;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("Select * from Account", SqlConnection);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                    accountList.Add(new AccountModel(reader["AccountName"].ToString(), reader["Password"].ToString()));
                MyApplication.Log("成功查询获取整个用户表");
            }
            catch (Exception e) { MyApplication.Log("查询获取整个用户表 失败了啊 " + e.ToString()); }
            finally { CloseSql(); }

            return accountList;
        }

    }
}
