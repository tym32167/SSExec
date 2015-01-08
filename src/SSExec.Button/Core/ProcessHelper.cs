using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Serialization;

namespace SSExec.Button.Core
{
    public class ProcessHelper
    {
        private readonly ILog _log;

        public ProcessHelper(ILog log)
        {
            _log = log;
        }

        public IEnumerable<ProcessInfoView> GetInfoes()
        {
            var actives = new List<string>();

            var ps = Process.GetProcesses();

            foreach (var process in ps)
            {
                try
                {
                    actives.Add(process.MainModule.FileName);
                }
                catch (Exception e)
                {

                }
            }


            var relfname = string.Format("{0}", WebConfigurationManager.AppSettings["processesFile"]);
            var fname = HttpContext.Current.Server.MapPath(relfname);

            var res = new List<ProcessInfoView>();

            using (var sr = new StreamReader(fname))
            {
                var ser = new XmlSerializer(typeof (List<ProcessInfoXml>));
                var list = ser.Deserialize(sr) as List<ProcessInfoXml>;

                if (list != null)
                {
                    res = list.Select(Convert).ToList();
                }
            }

            foreach (var processInfoView in res)
            {
                processInfoView.Active = actives.Contains(processInfoView.FilePath);
            }

            return res;
        }


        private ProcessInfoXml Convert(ProcessInfoView item)
        {
            return new ProcessInfoXml()
            {
                FilePath = item.FilePath,
                LastRestarted = item.LastRestarted,
                LastRestartedBy = item.LastRestartedBy,
                ProcessName = item.ProcessName,
                Arguments = item.Arguments,
                Title = item.Title
            };
        }


        private ProcessInfoView Convert(ProcessInfoXml item)
        {
            return new ProcessInfoView()
            {
                FilePath = item.FilePath,
                LastRestarted = item.LastRestarted,
                LastRestartedBy = item.LastRestartedBy,
                ProcessName = item.ProcessName,
                Arguments = item.Arguments,
                Title = item.Title
            };
        }

        public void Kill(string filename)
        {
            _log.Info("Kill " + filename);
            var infoes = GetInfoes();

            if (infoes.Any(x => String.CompareOrdinal(x.FilePath, filename) == 0))
            {
                var info = infoes.First(x => String.CompareOrdinal(x.FilePath, filename) == 0);
                try
                {

                    var pss = Process.GetProcesses();

                    foreach (var p in pss)
                    {
                        try
                        {
                            if (String.CompareOrdinal(p.MainModule.FileName, info.FilePath) == 0)
                            {
                                p.Kill();
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Error(e);
                    throw;
                }
            }
        }


        public void Start(string filename)
        {
            _log.Info("Start " + filename);
            var infoes = GetInfoes();

            if (infoes.Any(x => String.CompareOrdinal(x.FilePath, filename) == 0))
            {
                var info = infoes.First(x => String.CompareOrdinal(x.FilePath, filename) == 0);

                //var secureString = new SecureString();
                //"Tym#@167".ToCharArray().ToList().ForEach(secureString.AppendChar);
                if (!info.Active)
                {
                    try
                    {
                        Process.Start(info.FilePath, info.Arguments);
                        var uname = HttpContext.Current.User.Identity.Name;
                        var lastDate = DateTime.Now;

                        info.LastRestarted = lastDate;
                        info.LastRestartedBy = uname;

                        Save(infoes.Select(Convert).ToList());
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                        throw;
                    }
                }
                else
                {
                    _log.Info(string.Format("{0} has not been started because it already running. ", filename));
                }
            }
        }


        private void Save(List<ProcessInfoXml> list)
        {
            var relfname = string.Format("{0}", WebConfigurationManager.AppSettings["processesFile"]);
            var fname = HttpContext.Current.Server.MapPath(relfname);


            using (var sw = new StreamWriter(fname))
            {
                var ser = new XmlSerializer(typeof(List<ProcessInfoXml>));
                ser.Serialize(sw, list);
            }

        }



        public void Restart(string filename)
        {
            _log.Info("Restart " + filename);
            Kill(filename);
            Start(filename);
        }
    }




    public class ProcessInfoView
    {
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Arguments { get; set; }
        public string ProcessName { get; set; }
        public DateTime LastRestarted { get; set; }
        public string LastRestartedBy { get; set; }

        public bool Active { get; set; }
    }



    public class ProcessInfoXml
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string FilePath { get; set; }

        [XmlAttribute]
        public string Arguments { get; set; }

        [XmlAttribute]
        public string ProcessName { get; set; }

        [XmlAttribute]
        public DateTime LastRestarted { get; set; }

        [XmlAttribute]
        public string LastRestartedBy { get; set; }
    }

}
