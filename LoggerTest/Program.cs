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
		static Logger.Logger logger1 = null;
		static int random_num = 0;
		
		private static void ThreadProc( object state ) {
			int thread_number = (int)state;
			for ( int k =0 ; k < 100 ; ++k) {
				logger.WriteEntry(random_num + "-THREAD #"+thread_number.ToString());
				logger.WriteError(random_num + "-THREAD #"+thread_number.ToString());
				logger.WriteFatal(random_num + "-THREAD #"+thread_number.ToString());
				logger.WriteWarning(random_num + "-THREAD #"+thread_number.ToString());
				logger.WriteDebug(random_num + "-THREAD #"+thread_number.ToString());
				logger.WriteDebug(random_num + "-THREAD #"+thread_number.ToString()+" k= "+k.ToString());
				Thread.Sleep(50);
				logger1.WriteEntry(random_num + "-1THREAD #"+thread_number.ToString());
				logger1.WriteError(random_num + "-1THREAD #"+thread_number.ToString());
				logger1.WriteFatal(random_num + "-1THREAD #"+thread_number.ToString());
				logger1.WriteWarning(random_num + "-1THREAD #"+thread_number.ToString());
				logger1.WriteDebug(random_num + "-1THREAD #"+thread_number.ToString());
				logger1.WriteDebug(random_num + "-1THREAD #"+thread_number.ToString()+" k= "+k.ToString());
			}
		}
		
		public static void Main(string[] args)
		{
			random_num = (new Random((DateTime.Now.Millisecond))).Next();
			Logger.Logger.IsSingle = false;
			logger = Logger.Logger.GetInstance(@".\logs", "out.log");
			logger1 = Logger.Logger.GetInstance(@".\logs", "out.log");

			const int threads_cnt = 100;

			
			Thread[] threads = new Thread[threads_cnt];
			ThreadPool.SetMaxThreads( 100 * Environment.ProcessorCount, 20 );
			Logger.Logger.IsSingle = true;
			Console.WriteLine("A multithread test for Logger!");
			for ( int i = 0 ; i < threads_cnt; ++i ) {
				var ts = new ParameterizedThreadStart(ThreadProc);
				var new_thr = new Thread( ts );
				threads[i] = new_thr;
				new_thr.Start( i );
			}
			
			// Waiting for all of our threads...
			foreach ( var thr in threads ) {
				while ( thr.ThreadState != ThreadState.Stopped ) {
					Thread.Sleep(50);
				}
			}
			
			logger.WriteEntry("Press any key to continue . . . ");
			logger.Dispose();
			Console.ReadKey(true);
		}
	}
}