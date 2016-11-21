using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public class ShopifyCall
    {
        const int MaxCallPerSecond = 2;
        const int MaxBucketSize = 120;   

        private volatile static HashSet<ShopifyCall> Calls = new HashSet<ShopifyCall>();

        public ShopifyCall(Guid id)
        {
            Id = id;
            StartDate = DateTime.UtcNow;
        }

        public Guid Id { get; set; }

        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public void End()
        {
            EndDate = DateTime.UtcNow;
        }

        public static ShopifyCall StartCall()
        {
            var maximumAchieved = false;
            var maximumBucket = false;
            var call = default(ShopifyCall);

            do
            {
                lock (Calls)
                {
                    var lastMinute = DateTime.UtcNow;
                    var lastSecond = DateTime.UtcNow;

                    lastMinute = lastMinute.AddMinutes(-1);
                    lastSecond = lastSecond.AddSeconds(-1);

                    Calls.RemoveWhere(i => i.StartDate < lastMinute);

                    maximumAchieved = Calls.Count(i => i.StartDate >= lastSecond) >= MaxCallPerSecond;
                    maximumBucket = Calls.Count(i => i.StartDate >= lastMinute) >= MaxBucketSize;

                    if (!maximumAchieved && !maximumBucket)
                    {
                        call = new ShopifyCall(Guid.NewGuid());

                        Calls.Add(call);
                    }
                }

                if (maximumAchieved || maximumBucket)
                {
                    Thread.Sleep(250);
                }

            } while (maximumAchieved || maximumBucket);

            return call;
        }

        public static void ExecuteCall(Action doAction)
        {
            var call = StartCall();

            try
            {
                doAction();

                call.End();
            }
            catch (Exception e)
            {
                call.End();

                throw e;
            }
        }
    }
}
