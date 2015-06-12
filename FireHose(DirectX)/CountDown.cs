using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireHose_DirectX_
{
    class CountDown
    {

        public int Timer;
        public bool CountDownIsDone = false;

        public CountDown(int timer)
        {
            Timer = timer;
            CountDownIsDone = false;
        }

        public void Update()
        {
            Timer--;
            if (Timer < 0)
            {
                CountDownIsDone = true;
            }
        }
    }
}
