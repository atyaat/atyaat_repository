using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace demo
{
    class MysqlRead
    {
        public MySqlConnection connection_;//申明連接對象
        public DataSet myds;//申明數據集對象
        public MySqlDataAdapter sqlda; //DataAdapter对象在DataSet与数据之间起桥梁作用
        public DataTable mydt;
        public static string Name, Level, username;
        
        public DataSet MysqlRead_fun_status(string _column_name, string _column_val) //无static为非静态
        {
            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];        // 添加 Configuration 引用。
            connection_ = new MySqlConnection(connectionStr);
            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0}= '{1}'", _column_name, _column_val);
                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);
            }
            catch (MySqlException)
            {
                MessageBox.Show("数据库连接失败");
            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }
        string loginPath;
        [DllImport("kernel32.dll")]   
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        
        //读取ini文件内容
        public string GetINI(string Section, string AppName, string lpDefault, string FileName)
        {
            StringBuilder Str = new StringBuilder();

            GetPrivateProfileString(Section, AppName, lpDefault, Str, 2048, FileName);
            if (string.IsNullOrEmpty(Str.ToString()))
            {
                Interaction.MsgBox("无Config.ini文件，请确认", MsgBoxStyle.Exclamation, "提示");
            }

            return Str.ToString();

        }
        //写ini API函数
        public long WriteINI(string Section, string AppName, string lpDefault, string FileName)
        {
            return System.Convert.ToInt64(WritePrivateProfileString(Section, AppName, lpDefault, FileName));
        }

        //private void conn_db()
        //{
        //    string db_host = "";
        //    string db_user = "";
        //    string db_pass = "";
        //    string db_data = "";
        //    string strCN = "";
        //    loginPath = Application.StartupPath + "\\Config.ini";
        //    db_host = GetINI("Host_info", "db_host", "", loginPath);
        //    db_data = GetINI("Data_info", "db_data", "", loginPath);
        //    db_user = "smtdsm";
        //    db_pass = "smtdsm";
        //    //db_data = "agvdb";
        //    strCN = "Server=" + db_host + ";" +
        //        "User ID=" + db_user + ";" +
        //        "Password=" + db_pass + ";" +
        //        "Database=" + db_data + ";";
        //    MySqlConnection mycn = new MySqlConnection();
           
        //    mycn.ConnectionString = strCN;
        //    try
        //    {
        //        mycn.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        Modulelog.loginfo(ref sharedefine.loginfopath);
        //        sharedefine.setlog = "[" + DateTime.Now + "]conn_db:" + ex.Message;
        //        Modulelog.WriterTextFile(sharedefine.loginfopath, sharedefine.setlog);
        //    }
            
        //}



        //public DataTable MysqlRead_flog(string Sersql) //无static为非静态
        //{
        //    //conn_db();
        //    string db_host = "";
        //    string db_user = "";
        //    string db_pass = "";
        //    string db_data = "";
        //    string strCN = "";
        //    loginPath = Application.StartupPath + "\\Config.ini";
        //    db_host = GetINI("Host_info", "db_host", "", loginPath);
        //    db_data = GetINI("Data_info", "db_data", "", loginPath);
        //    db_user = "smtdsm";
        //    db_pass = "smtdsm";
        //    //db_data = "agvdb";
        //    strCN = "Server=" + db_host + ";" +
        //        "User ID=" + db_user + ";" +
        //        "Password=" + db_pass + ";" +
        //        "Database=" + db_data + ";";
        //    MySqlConnection mycn = new MySqlConnection();
        //    //sharedefine.mycn.ConnectionString = strCN;
        //    mycn.ConnectionString = strCN;
        //    //string connectionStr = ConfigurationManager.AppSettings["connectionStr2"];
        //    //connection_ = new MySqlConnection(sharedefine.mycn.ConnectionString);

        //    try
        //    {
        //        //connection_.Open();
        //        //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
        //        //string Sersql = string.Format(@"SELECT * FROM agv_call_sys_db where {0}= '{1}'", _column_name, _column_val);


        //        mycn.Open();
        //        sqlda = new MySqlDataAdapter(Sersql, mycn);
        //        myds = new DataSet();
        //        sqlda.Fill(myds);


        //    }
        //    catch (MySqlException)
        //    {
        //        mylog.WriteLog(DateTime.Now.ToString() + "数据库报错" + Sersql);
        //        MessageBox.Show("数据库连接失败");

        //    }

        //    finally
        //    {
        //        mycn.Close();
        //    }
        //    return myds.Tables[0];


        //}
        //public bool myslqupdata(string Sersql)
        //{
        //    //conn_db();
        //    //string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
        //    //connection_ = new MySqlConnection(connectionStr);

        //    string db_host = "";
        //    string db_user = "";
        //    string db_pass = "";
        //    string db_data = "";
        //    string strCN = "";
        //    loginPath = Application.StartupPath + "\\Config.ini";
        //    db_host = GetINI("Host_info", "db_host", "", loginPath);
        //    db_data = GetINI("Data_info", "db_data", "", loginPath);
        //    db_user = "smtdsm";
        //    db_pass = "smtdsm";
        //    //db_data = "agvdb";
        //    strCN = "Server=" + db_host + ";" +
        //        "User ID=" + db_user + ";" +
        //        "Password=" + db_pass + ";" +
        //        "Database=" + db_data + ";";
        //    MySqlConnection mycn = new MySqlConnection();
        //    //sharedefine.mycn.ConnectionString = strCN;
        //    mycn.ConnectionString = strCN;
          

        //    int i = 0;
        //    try
        //    {
        //        //connection_.Open();
        //        //string Sersql = "update bin_db_log set Status = 'Close' where Date = '2020-04-14 00:14:32'";
        //        //string Sersql = string.Format(@"update  userid_bin_db_log set password='{0}' where ID='{1}'", _newpass, _ID);
        //        mycn.Open();
        //        MySqlCommand sqlup = new MySqlCommand(Sersql, mycn);
        //        //sqlup.ExecuteNonQuery();

        //        i = sqlup.ExecuteNonQuery();//記錄數據更新數量，>0更新成功
        //    }
        //    catch(Exception ex )
        //    {
        //        mylog.WriteLog(DateTime.Now.ToString() + "数据库报错" + Sersql);
        //        MessageBox.Show("修改失败,请重试", "提示");

        //    }
        //    finally
        //    {
        //        mycn.Close();
        //    }
        //    if (i > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    { return false; }


        //}
        public DataSet MysqlRead_flog1(string Sersql) //无static为非静态
        {
            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                sqlda = new MySqlDataAdapter(Sersql, connection_);
                
                    myds = new DataSet();
                    sqlda.Fill(myds);
             
                    sqlda.Fill(myds);
             
                
            }
            catch (MySqlException)
            {
                MessageBox.Show("数据库连接失败");
            }
            finally
            {
                connection_.Close();
            }

            return myds;
        }
   


        public DataSet MysqlRead_fun_status(string _column_name1, string _column_val, string _column_name2) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0}= '{1}'and {2} is null", _column_name1, _column_val,_column_name2);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }
        public DataSet MysqlRead_fun_status(string _column_name) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0} is null and Status='3' ", _column_name);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }
        public DataSet MysqlRead_fun_status(string _column_name, string _column_val, DateTime starttime, DateTime endtime) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0}= '{1}'AND CloseTime BETWEEN '{2}' AND '{3}'order by CloseTime desc", _column_name, _column_val, starttime, endtime);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }


        public DataSet MysqlRead_fun_status(string _column_name, DateTime starttime, DateTime endtime) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0} IS NOT NULL AND CloseTime BETWEEN '{1}' AND '{2}'order by CloseTime desc", _column_name,  starttime, endtime);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }
        public DataSet MysqlRead_fun_status(string _column_name, string _column_val1, string _column_val2, DateTime starttime, DateTime endtime) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0}= '{1}'OR {2}='{3}'AND CloseTime BETWEEN '{4}' AND '{5}'", _column_name, _column_val1, _column_name, _column_val2, starttime, endtime);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }

        public DataSet MysqlRead_fun_pmconfirmok(string _column_name, DateTime starttime, DateTime endtime) //无static为非静态
        {


            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                //string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", Partnumber.Text);
                string Sersql = string.Format(@"SELECT * FROM bin_db_log where {0} is not null AND ConfirmTime BETWEEN '{1}' AND '{2}'", _column_name, starttime, endtime);



                sqlda = new MySqlDataAdapter(Sersql, connection_);
                myds = new DataSet();
                sqlda.Fill(myds);


            }
            catch (MySqlException)
            {

                MessageBox.Show("数据库连接失败");

            }

            finally
            {
                connection_.Close();
            }
            return myds;


        }


        public DataTable MysqlRead_fun(string _partseri) //无static为非静态
        {

            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);
            connection_.Open();
            try
            {

                //查倉庫發料報表獲取工單
                string Sersql = string.Format("SELECT Wo,BarcodeTim FROM wo_send_detail where PartSeri= '{0}'ORDER BY BarcodeTim", _partseri);
                sqlda = new MySqlDataAdapter(Sersql, connection_);
                mydt = new DataTable();
                sqlda.Fill(mydt);
                connection_.Close();

            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());

            }
            if (mydt.Rows.Count > 0)
            { return mydt; }

            else if (mydt.Rows.Count <= 0)
            {
                //查接料報表獲取工單
                connection_.Open();
                string Sersql = string.Format("SELECT Wo,Tims FROM scan_detail where Flg= '{0}'ORDER BY Tims", _partseri);
                sqlda = new MySqlDataAdapter(Sersql, connection_);
                mydt = new DataTable();
                sqlda.Fill(mydt);
                connection_.Close();
                return mydt;

            }
            else
            { return null; }




        }


        public string MysqlRead_bin_fun(string _PartName,string name) //无static为非静态
        {
            string Bin_Nam = "";
            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);
            connection_.Open();
            try
            {

                //查倉庫發料報表獲取工單
                string Sersql = string.Format("SELECT * FROM bin_db where PartName= '{0}'", _PartName);
                MySqlCommand mycom = new MySqlCommand(Sersql, connection_);
                MySqlDataReader mydr;
                mydr = mycom.ExecuteReader();


                if (mydr.Read())
                {

                    Bin_Nam = mydr[name].ToString();

                }
                else { Bin_Nam = "未查到"; }
               
            }
            catch
            {
                Bin_Nam = "未查到";
                //return Bin_Nam;
            }


            return Bin_Nam;

        }

        public bool MysqlRead_password(string _ID, string _password) //无static为非静态
        {
            bool loginok = false;
            string constr = ConfigurationManager.AppSettings["userdb"];
            MySqlConnection conn = new MySqlConnection(constr);//创建数据库连接
            conn.Open();//打开连接
            //SqlDataAdapter sda = new SqlDataAdapter();//SqlDataAdapter是数据适配器，是数据库和调用者之间的桥梁
            MySqlCommand cmd = new MySqlCommand();  //SqlCommand表示对bai数据库要执行的操作命令。
            cmd.CommandText = string.Format("select *from user_db_info where Usernum='{0}'", _ID);//cmd要执行的sql操作语句
            cmd.Connection = conn;//cmd对应的连接
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                username = reader["Usernum"].ToString();
                string password = reader["Userpsd"].ToString();
                Name = reader["Usernames"].ToString();
                Level = reader["Userflg"].ToString();
                //Trim()表示把字符串前后的空格都去除。不然有空格会干扰判断。
                if (_ID.Trim() == username.Trim() && password.Trim() == _password.Trim())
                {
                    //Form1 f = new Form1();
                    //f.Show();   //弹出Form1这个窗体       
                    loginok = true;

                }
                else
                {
                    loginok = false;
                    MessageBox.Show("用户名或密码错误，请重新输入");

                    //this.textBox2.Text = "";
                    //this.textBox1.Text = "";
                }

            }

            return loginok;
        }
        public DataTable MysqlRead_fun(string _partname, string _wo) //无static为非静态
        {

            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);
            connection_.Open();
            try
            {

                //查倉庫發料報表獲取工單  SELECT [PARTNO]as [PARTNO],COUNT(*) as PARTNOCount,sum([Qty]) as sumQty FROM [smt_zd_material] WHERE PARTNO !=''and isTake!=1 and LockLocation IS NOT NULL GROUP BY [PARTNO]
                string Sersql = string.Format("SELECT SUM(PartNum) AS PartNumsum  FROM wo_send_detail where PartName='{0}'and Wo LIKE '%{1}%'GROUP BY PartName", _partname, _wo);//SELECT SUM(PartNum) AS PartNumsum  FROM wo_send_detail where PartName='1542458A00'AND Wo LIKE '%2512005199A%' GROUP BY PartName
                sqlda = new MySqlDataAdapter(Sersql, connection_);
                mydt = new DataTable();
                sqlda.Fill(mydt);
                connection_.Close();

            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.ToString());

            }

            return mydt;


        }


        /*
        返回datatable数据，传入参数1：SQL语句 2：表名
         */
        public void MysqlReadRerurnDatatable(string Sersql, string DataTableName, ref DataTable dara) 
        {
            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                sqlda = new MySqlDataAdapter(Sersql, connection_);

                myds = new DataSet();

                sqlda.Fill(myds, DataTableName);

            }
            catch (MySqlException)
            {
                MessageBox.Show("数据库连接失败");
            }
            finally
            {
                connection_.Close();
            }

            dara = myds.Tables[0];
        }
        /*
         返回dataset数据，传入参数1：SQL语句 2：表名
        */
        public void MysqlReadRerurnDataset(string Sersql, string DataTableName, ref DataSet dara)
        {
            string connectionStr = ConfigurationManager.AppSettings["connectionStr"];
            connection_ = new MySqlConnection(connectionStr);

            try
            {
                connection_.Open();
                sqlda = new MySqlDataAdapter(Sersql, connection_);

                myds = new DataSet();

                sqlda.Fill(myds, DataTableName);

            }
            catch (MySqlException)
            {
                MessageBox.Show("数据库连接失败");
            }
            finally
            {
                connection_.Close();
            }

            dara = myds;
        }




    }


}


