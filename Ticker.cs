using System;
using System.Threading;
using Org.Kevoree.Annotation;
using Org.Kevoree.Core.Api;
using Org.Kevoree.Log.Api;
using System.ComponentModel.Composition;

namespace Org.Kevoree.Library
{
    [ComponentType]
    [Serializable]
    [Export(typeof(DeployUnit))]
    internal class Ticker : MarshalByRefObject, DeployUnit
    {
        [Param(Optional = true, DefaultValue = "3000")]
        private long period = 3000;

        [Output]
        private Port tick;

        [Param(Optional = true, DefaultValue = "false")]
        private bool random = false;

        [KevoreeInject]
        private ILogger logger;

        private readonly Random rnd = new Random();

        private bool stopMe;

        [Start]
        public void Start()
        {
            logger.Debug("Start");
            stopMe = false;
            new Thread(this.Run).Start();
        }

        [Stop]
        public void Stop()
        {
            logger.Debug("Stop");
            stopMe = true;
        }

        private void Run()
        {
            while (!stopMe)
            {
                long value;
                if (random)
                {
                    value = rnd.Next();
                }
                else
                {
                    value = DateTime.UtcNow.Ticks;
                }

                logger.Debug("Value : " + value);

                tick.send(value.ToString(), null);
                Thread.Sleep((int)this.period);
            }
        }
    }
}