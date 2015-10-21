using System;
using System.Threading;
using Org.Kevoree.Annotation;
using Org.Kevoree.Core.Api;

namespace Org.Kevoree.Library
{
    [ComponentType]
    [Serializable]
    internal class Ticker : MarshalByRefObject, DeployUnit
    {
        [Param(Optional = true, DefaultValue = "3000")] private long period;

        [Output] private Port tick;

        [Param(Optional = true, DefaultValue = "false")] private bool random;

        private readonly Random rnd = new Random();

        private bool stopMe;

        [Start]
        public void Start()
        {
            stopMe = false;
            new Thread(new Ticker().Run);
        }

        [Stop]
        public void Stop()
        {
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

                tick.send(value.ToString(), null);
            }
        }
    }
}