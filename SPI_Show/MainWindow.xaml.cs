using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Media;

namespace SPI_Show
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        MySqlConnection conn;
        string connectionStr = "server = 10.148.208.25; port = 3306; database = agvdb; user = smtdsm; password = smtdsm; Sslmode = none;";
        int flag_sound = 0;

        // 实例化 定时器
        private static System.Windows.Threading.DispatcherTimer readDataTimer = new System.Windows.Threading.DispatcherTimer();
        //界面启动运行事件
        private void Load_form(object sender, RoutedEventArgs e)
        {
            //butt_1.Background = System.Windows.Media.Brushes.ForestGreen;
           
            conn = new MySqlConnection(connectionStr);          //创建数据库连接对象
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            Show_text();
            conn.Close();
            //配置定时器，时间为一秒
            readDataTimer.Tick += new EventHandler(timeCycle); //到时间执行 timeCycle 程序
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 30);
            readDataTimer.Start();
        }
        //定时运行的程序
        public  void timeCycle(object sender, EventArgs e)
        {
            if(a!=1)
           Show_text();
        }
        //显示数据程序
        private void Show_text()
        {
            find_sql();
            To_show();
        }
        string day;
        int H = 0;
        int a = 0;
        float[] M = new float[12];
        float[] T = new float[12];
        float[] G = new float[12];
        float[] P = new float[12];
        float[] F = new float[12];
        string[] diaox = new string[12];
        int[] ts = new int[12];
        string[] spi_lock = new string[12];
       

        DataSet dM;
        DataTable dT, dG, dF, dP, diaoxian, sp_lock;

        Brush red = new SolidColorBrush(Color.FromRgb(233, 150, 122));//鲜肉色 233,150,122  240, 128, 128
        Brush green = new SolidColorBrush(Color.FromRgb(50, 205, 50));// 酸橙色               0, 255, 127
        Brush ghuis = new SolidColorBrush(Color.FromRgb(211, 211, 211));// 灰色
        Brush rred = new SolidColorBrush(Color.FromRgb(255, 69, 0));    //橘红色
        //查询数据程序 将数据放入dataset 5个表中 ，并将表中的数据分配到对应的数组中
        private void find_sql()
        {
          
            string strSQL_Mflg = "";
            string strSQL_T = "";
            string strSQL_G = "";
            string strSQL_P = "";
            string strSQL_F = "";
            string sql_diaoxian = "";
            string sql_lock = "";

            MySqlDataAdapter da;
            string day2, banc;
           
            if (a==0)            // 实时显示模式
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");

                times.Text = day;
                H = DateTime.Now.Hour;

            }
            //TEST REPEAT!
             if (H > 7 && H < 20)
            {
                banc = "白班";
                time.Text = banc;
                strSQL_Mflg = "SELECT LINENAME,count(*) Mflg FROM spisnlinkdb WHERE M_FLG = '0'AND TIMS between '" + day + " 08 'AND '" + day + " 20' GROUP BY LINENAME";     
                strSQL_T = "SELECT LINENAME,count(*) T FROM spisnlinkdb WHERE  TIMS between '" + day + " 08 'AND '" + day + " 20' GROUP BY LINENAME";
                strSQL_G = "SELECT LINENAME,count(*) G FROM spisnlinkdb WHERE SPIRESULT = 'GOOD'AND TIMS between '" + day + " 08 'AND '" + day + " 20' GROUP BY LINENAME";
                strSQL_P = "SELECT LINENAME,count(*) P FROM spisnlinkdb WHERE SPIRESULT = 'PASS'AND TIMS between '" + day + " 08 'AND '" + day + " 20' GROUP BY LINENAME";
                strSQL_F = "SELECT LINENAME,count(*) F FROM spisnlinkdb WHERE SPIRESULT = 'NG'AND TIMS between '" + day + " 08 'AND '" + day + " 20' GROUP BY LINENAME";
               
            }
            else if (H < 8)
            {
                banc = "夜班";
                day = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                day2 = DateTime.Now.ToString("yyyy-MM-dd");
                time.Text = banc;
                if (a == 0)
                
                strSQL_Mflg = "SELECT LINENAME,count(*) Mflg FROM spisnlinkdb WHERE M_FLG = '0'AND TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                strSQL_T = "SELECT LINENAME,count(*) T FROM spisnlinkdb WHERE  TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                strSQL_G = "SELECT LINENAME,count(*) G FROM spisnlinkdb WHERE SPIRESULT = 'GOOD'AND TIMS  between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                strSQL_P = "SELECT LINENAME,count(*) P FROM spisnlinkdb WHERE SPIRESULT = 'PASS'AND TIMS  between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                strSQL_F = "SELECT LINENAME,count(*) F FROM spisnlinkdb WHERE SPIRESULT = 'NG'AND TIMS  between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
              
            }
            else if (H > 19)
            {
                banc = "夜班";
                time.Text = banc;

                if (a == 0)
                {
                    day2 = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                    day = DateTime.Now.ToString("yyyy-MM-dd");
                    times.Text = day;
                }
                else        // 将日期加一天，31号？？
                {
                    string d = times.Text.ToString();
                    string year = d.Substring(0, 4);
                    string mon = d.Substring(5, 2);
                    int days = Convert.ToInt32(d.Substring(8, 2)) + 1;
                    day2 = year + "-" + mon + "-" + days.ToString();
                }

                strSQL_Mflg = "SELECT LINENAME,count(*) Mflg FROM spisnlinkdb WHERE M_FLG = '0'AND TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";            
                strSQL_T = "SELECT LINENAME,count(*) T FROM spisnlinkdb WHERE  TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                strSQL_G = "SELECT LINENAME,count(*) G FROM spisnlinkdb WHERE SPIRESULT = 'GOOD'AND TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";             
                strSQL_P = "SELECT LINENAME,count(*) P FROM spisnlinkdb WHERE SPIRESULT = 'PASS'AND TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";        
                strSQL_F = "SELECT LINENAME,count(*) F FROM spisnlinkdb WHERE SPIRESULT = 'NG'AND TIMS between '" + day + " 20 'AND '" + day2 + " 08' GROUP BY LINENAME";
                
            }
            sql_diaoxian = "SELECT LINENAME,TIMS FROM linecheckdb ";
            sql_lock = "SELECT LINENAME,BLOCKFLG FROM linebasedb WHERE MACHINETYPE = 'LOADER'";
            

               da = new MySqlDataAdapter(strSQL_Mflg, conn); //参数1：SQL语句；参数2：数据库连接对象
               dM = new DataSet();         //M_FLG 数量
               da.Fill(dM, "Mflg");

               da = new MySqlDataAdapter(strSQL_T, conn); //参数1：SQL语句；参数2：数据库连接对象
               dT = new DataTable();    // 总数
               da.Fill(dM, "T");

               da = new MySqlDataAdapter(strSQL_G, conn); //参数1：SQL语句；参数2：数据库连接对象
               dG = new DataTable();    // good数量
               da.Fill(dM, "G");

               da = new MySqlDataAdapter(strSQL_P, conn); //参数1：SQL语句；参数2：数据库连接对象
               dP = new DataTable();    // pass 数量
               da.Fill(dM, "P");

               da = new MySqlDataAdapter(strSQL_F, conn); //参数1：SQL语句；参数2：数据库连接对象
               dF = new DataTable();    // mes_fail 数量
               da.Fill(dM, "F");

               da = new MySqlDataAdapter(sql_diaoxian, conn); //参数1：SQL语句；参数2：数据库连接对象
               diaoxian = new DataTable();    // 是否掉线
               da.Fill(dM, "diaoxian");

               da = new MySqlDataAdapter(sql_lock, conn); //参数1：SQL语句；参数2：数据库连接对象
               sp_lock = new DataTable();    // 是否锁线
               da.Fill(dM, "lock");


            // 将表中的数据整理到数组中
            value_null("Mflg", "Mflg", M);
            value_null("T", "T", T);
            value_null("G", "G", G);
            value_null("P", "P", P);
            value_null("F", "F", F);
            
            value_str("diaoxian", "TIMS", diaox);
            value_str("lock", "BLOCKFLG", spi_lock);

            int h_s = DateTime.Now.Hour;
            int min_s = DateTime.Now.Minute;

            for (int i = 0; i < 12; i++)
            {
                //string H = diaox[i].Substring(11, 2);
                //string min = diaox[i].Substring(14, 2);

                TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(diaox[i]);

                int gg = timeSpan.Days * 1440 + timeSpan.Hours * 60 + timeSpan.Minutes; ;
                //int f = Convert.ToInt32(H) * 60 + Convert.ToInt32(min);
                //int s = h_s * 60 + min_s;
                //ts[i] = s - f;
                ts[i] = gg;
                if (ts[i] > 999)
                {
                    ts[i] = 999;
                }
                if (ts[i] < 0)
                    ts[i] = 0;
            }
        }
      
      //将表中的数据分配到对应的数组中（float）
        private void value_null(string a,string b,float[] d )   // a:表名 b:列名 d: 数组名
        {
          int c = Convert.ToInt32(dM.Tables[a].Rows.Count);
            for (int i = 0; i < c; i++)
            {
                string C = dM.Tables[a].Rows[i][0].ToString();
                switch (C)
                {
                    case "SMT_H01":
                        d[0] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H02":
                        d[1] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H03":
                        d[2] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H04":
                        d[3] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H05":
                        d[4] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H06":
                        d[5] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H07":
                        d[6] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H08":
                        d[7] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H09":
                        d[8] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H10":
                        d[9] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H11":
                        d[10] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                    case "SMT_H12":
                        d[11] = Convert.ToInt32(dM.Tables[a].Rows[i][b]);
                        break;
                }

            }

        }

        // 将表中的数据分配到对应的数组中（string）
        private void value_str(string a, string b, string[] d)   //a:表名 b:列名 d: 数组名
        {
            int c = Convert.ToInt32(dM.Tables[a].Rows.Count);
            for (int i = 0; i < c; i++)
            {
                string C = dM.Tables[a].Rows[i][0].ToString();
                switch (C)
                {
                    case "SMT_H01":
                        d[0] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H02":
                        d[1] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H03":
                        d[2] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H04":
                        d[3] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H05":
                        d[4] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H06":
                        d[5] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H07":
                        d[6] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H08":
                        d[7] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H09":
                        d[8] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H10":
                        d[9] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H11":
                        d[10] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                    case "SMT_H12":
                        d[11] = dM.Tables[a].Rows[i][b].ToString();
                        break;
                }

            }
        }
        string lianglv, wuche, mes_fail, m,t,l;

      



        int suo = 0; // 标记锁线的
        //循环遍历数组，计算后給对应的textblock 赋值，并将数据清零，并设置颜色，灰色-红色-绿色
        private void To_show()
        {
            for (int i = 0; i < 12; i++)
            {
                lianglv = string.Format("{0:f1}", 100 * (G[i] + P[i]) / T[i]);   // 良率
                wuche = string.Format("{0:f1}", 100 * G[i] / T[i]);         // 直通率
                mes_fail = string.Format("{0:f0}", 1000000 * F[i] / T[i]);      // mes_fail
                m = M[i].ToString();
                t = ts[i].ToString();
                l = spi_lock[i];

                // 将数据清零
                M[i] = 0;
                T[i] = 0;
                G[i] = 0;
                P[i] = 0;
                F[i] = 0;
                ts[i] = 0;
                spi_lock[i] = "";
                switch (i)
                {
                    case 0:
                        text_show(text1_1, text1_2, text1_3, text1_4, text1_5, text1_6, butt1, Show_1);
                        break;
                    case 1:
                        text_show(text2_1, text2_2, text2_3, text2_4, text2_5, text2_6, butt2, Show_2);

                        break;
                    case 2:
                        text_show(text3_1, text3_2, text3_3, text3_4, text3_5, text3_6, butt3, Show_3);

                        break;
                    case 3:
                        text_show(text4_1, text4_2, text4_3, text4_4, text4_5, text4_6, butt4, Show_4);

                        break;
                    case 4:
                        text_show(text5_1, text5_2, text5_3, text5_4, text5_5, text5_6, butt5, Show_5);

                        break;
                    case 5:
                        text_show(text6_1, text6_2, text6_3, text6_4, text6_5, text6_6, butt6, Show_6);

                        break;
                    case 6:
                        text_show(text7_1, text7_2, text7_3, text7_4, text7_5, text7_6, butt7, Show_7);

                        break;
                    case 7:
                        text_show(text8_1, text8_2, text8_3, text8_4, text8_5, text8_6, butt8, Show_8);

                        break;
                    case 8:
                        text_show(text9_1, text9_2, text9_3, text9_4, text9_5, text9_6, butt9, Show_9);

                        break;
                    case 9:
                        text_show(text10_1, text10_2, text10_3, text10_4, text10_5, text10_6, butt10, Show_10);

                        break;
                    case 10:
                        text_show(text11_1, text11_2, text11_3, text11_4, text11_5, text11_6, butt11, Show_11);

                        break;
                    case 11:
                        text_show(text12_1, text12_2, text12_3, text12_4, text12_5, text12_6, butt12, Show_12);

                        break;
                }
            }
                if (suo == 1)
                {

                if (flag_sound != 2)
                    flag_sound = 1;

                    suo = 0;
                }
                else
                {
                    flag_sound = 0;
                }
           
                Sound_Ctrl();
         }
          

            // 赋值 并改变颜色
         void text_show(TextBlock text1, TextBlock text2, TextBlock text3, TextBlock text4, TextBlock text5, TextBlock text6, Button button, Border Show)
        {
                text1.Text = lianglv + "%";
                text2.Text = wuche + "%";
                text3.Text = mes_fail ;
                text4.Text = m;
                text5.Text = t+"min";

                
                if (Convert.ToUInt32(t) > 2)
                {
             
                    Show.Background = ghuis;
                    Show.BorderBrush = ghuis;
                    button.Background = ghuis;
                    button.BorderBrush = ghuis;
                    text6.Text = "掉线";
                    
                }
                else if (Convert.ToInt32(m) > 0)
                {
                    Show.Background = red;
                    Show.BorderBrush = red;
                    button.Background = red;
                    button.BorderBrush = red;
                    text6.Text = "超标";
                   
                }
                else
                {
                    Show.Background = green;
                    Show.BorderBrush = green;
                    button.Background = green;
                    button.BorderBrush = green;
                    text6.Text = "正常";
                    
                }
                if (l == "1")
                {
                    text6.Text = "锁线";
                    suo = 1;
                   
                    button.Background = rred;
                    button.BorderBrush = rred;
                }

               
            }

        
       
        // 按钮 事件
        private void Butt_1_Click(object sender, RoutedEventArgs e)
        {
            //butt_1.Background = System.Windows.Media.Brushes.ForestGreen;
            //butt_1.BorderBrush = System.Windows.Media.Brushes.ForestGreen;

            ////MyGrid.Children.Add(new Page1());

            ////界面切换
            //Details window1 = new Details();
            //window1.Show();
            //this.Close();
        }


        //实时显示按钮
        private void xinas_mo(object sender, MouseButtonEventArgs e)
        {

            if ("实时显示".Equals(mode.Text.ToString()))
            {
                mode.Text = "历史数据";
                a = 1;
            }
            else if ("历史数据".Equals(mode.Text.ToString()))
            {
                mode.Text = "实时显示";
                times.Text = DateTime.Now.ToString("yyyy/MM/dd");
                a = 0;
                Show_text();
            }
        }
        //白班按钮
        private void banc_m(object sender, MouseButtonEventArgs e)
        {
            if (a == 1)
            {
                if ("白班".Equals(time.Text.ToString()))
                {
                    time.Text = "夜班";
                }
                else if ("夜班".Equals(time.Text.ToString()))
                {
                    time.Text = "白班";
                }      
            }
        }
        //SPI_Show按钮
        private void show_mo(object sender, MouseButtonEventArgs e)
        {
            if (a == 1)
            {
                string b;
                b = times.Text.ToString();
                string year = b.Substring(0, 4);
                string mon = b.Substring(5, 2);
                string d = b.Substring(8, 2);
                day = year + "-" + mon + "-" + d;

                if ("白班".Equals(time.Text.ToString()))
                {
                    H = 19;
                }
                else if ("夜班".Equals(time.Text.ToString()))
                {
                    H = 21;
                }   
                Show_text();
            }
            
        }

        
        public void Sound_Ctrl()
        {
            switch (flag_sound)
            {
                case 0:  // 关声音 不显示图标
                    sound_off.Visibility = Visibility.Collapsed; // 隐藏图标
                    sound_o.Visibility = Visibility.Collapsed; // 隐藏图标
                    player.Stop();
                    break;
                case 1:  // 开声音 显示图标
                    sound_off.Visibility = Visibility.Collapsed; // 隐藏图标
                    sound_o.Visibility = Visibility.Visible;
                    Sound();
                    break;
                case 2:  //关声音 显示图标
                    sound_o.Visibility = Visibility.Collapsed; // 隐藏图标
                    sound_off.Visibility = Visibility.Visible;
                    player.Stop();
                    break;
            }
        }
        SoundPlayer player = new SoundPlayer();
        public void Sound()
        {
            string currentPath = Directory.GetCurrentDirectory();//在Debug文件夾下新建

             
            player.SoundLocation = currentPath + @"\test.wav";
            player.Load(); //同步加载声音
            player.Play(); //启用新线程播放
            player.PlayLooping(); //循环播放模式
            //player.PlaySync(); //UI线程同步播放     
        }
        private void sound_up(object sender, MouseButtonEventArgs e)
        {
            flag_sound = 2;
            Sound_Ctrl();
        }
        private void sound_dow(object sender, MouseButtonEventArgs e)
        {
            flag_sound = 1;
            Sound_Ctrl();
        }

    }
}
