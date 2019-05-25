using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTranser.QTranseLib
{
    public class GlobalUnhandledException:Application
    {
        private Boolean IsAppend = true;
        public GlobalUnhandledException()
        {
            // UI线程未捕获异常处理事件
            DispatcherUnhandledException += Current_DispatcherUnhandledException;
            // Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            // 非UI线程未捕获异常处理事件(包含UI线程异常）
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出   
                e.Handled = true;    
                Loger.Exception("UI异常：" + e.Exception.ToString(), IsAppend);
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                Loger.Exception("UI致命异常：" + ex.ToString(), IsAppend);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
           
            //设置该异常已察觉（这样处理后就不会引起程序崩溃）
            e.SetObserved();
            Loger.Exception("Task线程异常：" + e.Exception.ToString(), IsAppend);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("非UI线程异常：");
            }
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
                sbEx.Append(((Exception)e.ExceptionObject));
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            Loger.Exception(sbEx.ToString(), IsAppend);
        }
    }
}
