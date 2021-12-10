using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tdsCshapu
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //����δ������쳣   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //����UI�߳��쳣   
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //�����UI�߳��쳣   
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = "";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                str = string.Format("UI����Ӧ�ó���δ������쳣\n�쳣���ͣ�{0}\n�쳣��Ϣ��{1}\n�쳣λ�ã�{2}\n",
                     error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("Ӧ�ó����̴߳���:{0}", e);
            }
            try { Log_write(str); } catch { };

          //����
          //  System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.ExitThread();
            Application.Exit();
            System.Environment.Exit(0);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = "";
            Exception error = e.ExceptionObject as Exception;
            if (error != null)
            {
                str = string.Format(",��UI Application UnhandledException:{0};\n��ջ��Ϣ:{1}", error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }
            try { Log_write(str); } catch { };
            
            
            //����;
            //System.Diagnostics.Process.Start(Application.ExecutablePath);

            Application.ExitThread();
            Application.Exit();
            System.Environment.Exit(0);
        }
         static void Log_write(string log)
        {
            string path = Application.StartupPath + "\\" + "Sys.log";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(path, true, System.Text.Encoding.GetEncoding("gb2312")))
            {

                fs.WriteLine(DateTime.Now.ToString() + log);

                fs.Close();
            }
        }
    }
}
