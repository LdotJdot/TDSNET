using DocumentFormat.OpenXml.Office2021.Excel.NamedSheetViews;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDSNET;
using TDSNET.UI;

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
            bool flag = false;
            System.Threading.Mutex hMutex = new System.Threading.Mutex(true, Application.ProductName, out flag);
            
            //waitOne����ɾ������Ϊ���迼����Դ�ȴ������ж��Ƿ��½�����
            bool b = hMutex.WaitOne(0, false);

            if(!flag)
            {
                //��ȡ������������
                string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                ////��ȡ�汾�� 
                //CommonData.VersionNumber = Application.ProductVersion; 
                //�������Ƿ��Ѿ��������Ѿ���������ʾ������Ϣ�˳����� 
                var p = System.Diagnostics.Process.GetProcessesByName(strProcessName);
                if (p?.Length > 1)
                {
                    PostThreadMessage(p[0].Threads[0].Id, 0x010, IntPtr.Zero, IntPtr.Zero);
                    Application.Exit();
                    Environment.Exit(0);
                    return;
                }
            }

    

            //����δ������쳣   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //����UI�߳��쳣   
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //�����UI�߳��쳣   
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form1();
            Application.AddMessageFilter(new MsgRecev(() => form.autoshoworhide()));

            Application.Run(form);
        }
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
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

                
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(int threadId, uint msg, IntPtr wParam, IntPtr lParam);

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
