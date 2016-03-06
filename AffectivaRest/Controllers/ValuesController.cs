using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Affdex;
using System.Text;
using Newtonsoft.Json;

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

        private class ProcessOutput
        {
            internal bool processed;
            public bool detectSmile;
            public bool anger;
            public bool attention;
            public bool browFurrow;
            public bool browRaise;
            public bool lipPress;
            public bool contempt;
            public bool disgust;
            public bool chinRaise;
            public bool engagement;
            public bool eyeClosure;
            public bool fear;
            public bool gender;
            public bool glasses;
            public bool innerBrowRaise;
            public bool joy;
            public bool lipCornerDepressor;
            public bool lipPucker;
            public bool lipSuck;
            public bool mouthOpen;
            public bool noseWrinkle;
            public bool sadness;
            public bool smile;
            public bool smirk;
            public bool surprise;
            public bool upperLipRaise;
            public bool valence;

            public ProcessOutput(bool detectSmile) {
                this.processed = false;
                this.detectSmile = detectSmile;
            }
        }

        private class ProcessListener : Affdex.ProcessStatusListener
        {
            private PhotoDetector pd;
            private ProcessOutput po;

            public ProcessListener(PhotoDetector pd, ProcessOutput po)
            {
                this.pd = pd;
                this.po = po;
                pd.setProcessStatusListener(this);
            }

            public void onProcessingException(AffdexException ex)
            {
                throw new NotImplementedException();
            }

            public void onProcessingFinished()
            {
                po.anger = pd.getDetectAnger();
                po.attention = pd.getDetectAttention();
                po.browFurrow = pd.getDetectBrowFurrow();
                po.browRaise = pd.getDetectBrowRaise();
                po.chinRaise = pd.getDetectChinRaise();
                po.contempt = pd.getDetectContempt();
                po.disgust = pd.getDetectDisgust();
                po.engagement = pd.getDetectEngagement();
                po.eyeClosure = pd.getDetectEyeClosure();
                po.fear = pd.getDetectFear();
                po.gender = pd.getDetectGender();
                po.glasses = pd.getDetectGlasses();
                po.innerBrowRaise = pd.getDetectInnerBrowRaise();
                po.joy = pd.getDetectJoy();
                po.lipCornerDepressor = pd.getDetectLipCornerDepressor();
                po.lipPress = pd.getDetectLipPress();
                po.lipPucker = pd.getDetectLipPucker();
                po.lipSuck = pd.getDetectLipSuck();
                po.mouthOpen = pd.getDetectMouthOpen();
                po.noseWrinkle = pd.getDetectNoseWrinkle();
                po.sadness = pd.getDetectSadness();
                po.smile = pd.getDetectSmile();
                po.smirk = pd.getDetectSmirk();
                po.surprise = pd.getDetectSurprise();
                po.upperLipRaise = pd.getDetectUpperLipRaise();
                po.valence = pd.getDetectValence();
                po.processed = true;
            }
        };

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
            
            var po = new ProcessOutput(false);
            var pl = new ProcessListener(pd, po);
            
            pd.setDetectAllEmojis(true);
            pd.setDetectAllEmotions(true);
            pd.setDetectAllExpressions(true);

            pd.start();

            pd.process(frame);

            //pd.stop();

            while (true)
            {
                if (po.processed) {

                    return JsonConvert.SerializeObject(po);
                }
            }
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
