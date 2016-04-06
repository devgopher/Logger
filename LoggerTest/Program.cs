/*
 * User: igor.evdokimov
 * Date: 20.02.2016
 * Time: 12:01
 */
using System;
using System.Threading;

namespace LoggerTest
{
	class Program
	{
		static Logger.Logger logger = null;
		private static void ThreadProc( object state ) {
			int thread_number = (int)state;
			for ( int k =0 ; k < 100 ; ++k) {
				logger.WriteEntry("THREAD #"+thread_number.ToString());
				logger.WriteError("THREAD #"+thread_number.ToString());
				logger.WriteWarning("THREAD #"+thread_number.ToString());
				logger.WriteDebug("THREAD #"+thread_number.ToString());
			}
		}
		
		public static void Main(string[] args)
		{
			logger = Logger.Logger.GetInstance(".", "out.log");
			int threads_cnt = 200;
			Logger.Logger.IsSingle = true;
			Console.WriteLine("A multithread test for Logger!");
			for ( int i = 0 ; i < threads_cnt; ++i ) {
				var ts = new ParameterizedThreadStart(ThreadProc);
				var new_thr = new Thread( ts );
				
				new_thr.Start( i );
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}