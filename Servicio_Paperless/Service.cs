using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Leer_Stock;

namespace Servicio_Paperless
{
    public partial class Service : ServiceBase
    {
        Timer tmservicio = null;

        public Service()
        {
            InitializeComponent();
            tmservicio = new Timer(10000);
            tmservicio.Elapsed += new ElapsedEventHandler(tmpServicio_Elapsed);
        }

        void tmpServicio_Elapsed(object sender, ElapsedEventArgs e)
        {
            //string path = @"C:\log.txt";
            //TextWriter tw = new StreamWriter(path, true);
            //tw.WriteLine("A fecha de : " + DateTime.Now.ToString() + ", Intervalo: " + tmservicio.Interval.ToString());
            try
            {
                Leer_Stock.Program.Procesa();
                //tw.WriteLine("ok " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                //tw.WriteLine("error "+ex.Message);
                //tw.Close();
            }
            //tw.Close();
        }

        protected override void OnStart(string[] args)
        {
            tmservicio.Start();
        }

        protected override void OnStop()
        {
            tmservicio.Stop();
        }
    }
}
