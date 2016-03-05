using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Affdex;
using System.Text;

namespace AffectivaRest.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            var a = DateTime.Now.ToString();
            return new string[] { "value1", "value2", a };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        private class ProcessListener : Affdex.ProcessStatusListener
        {
            private PhotoDetector pd;
            private AsyncCallback callback;

            public ProcessListener(PhotoDetector pd)
            {
                this.pd = pd;
                //this.callback = callback;
                pd.setProcessStatusListener(this);
            }

            public void onProcessingException(AffdexException ex)
            {
                throw new NotImplementedException();
            }

            public void onProcessingFinished()
            {
                pd.getDetectSmile();   
                throw new NotImplementedException();
            }
        };

        public delegate bool CallBack(int hwnd, int lParam);

        // POST api/values
        public string Post([FromBody]dynamic value)
        {

            String image = value.image.Value;
            byte[] pixels = Convert.FromBase64String(image);
            int width = Int32.Parse(value.width.Value);
            int height = Int32.Parse(value.height.Value);
            
            var frame = new Frame(width, height, pixels, Frame.COLOR_FORMAT.RGB);
            var pd = new PhotoDetector();

            String licensePath = "C:\\Program Files\\Affectiva\\Affdex SDK\\affdex.license";
            pd.setLicensePath(licensePath);

            String classifierPath = "C:\\Program Files\\Affectiva\\Affdex SDK\\data";
            pd.setClassifierPath(classifierPath);

            //public static string Report(int hwnd, int lParam)
            //{
            //    return "true";
            //}

            //CallBack myCallBack = new CallBack(EnumReportApp.Report);
            //EnumWindows(myCallBack, 0);

            new ProcessListener(pd);

            pd.setDetectSmile(true);

            pd.start();

            pd.process(frame);

            //pd.stop();

            return "NON";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
