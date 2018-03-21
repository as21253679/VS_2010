using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices; //使用外部動態函式庫，需要參考這個物件

namespace 模擬鍵盤C_Sharp
{
    class Program
    {
        //調用user32.dll內的keybd_event函式
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, IntPtr dwExtraInfo);

        //定義數值
        const byte A_key = 0x41;                    //鍵盤A的虛擬掃描代碼
        const byte KEYEVENTF_EXTENDEDKEY = 0x01;
        const byte KEYEVENTF_KEYUP = 0x02;
        static void Main(string[] args)
        {
            //連續執行
            while (true)
            {
                //按下鍵盤A鍵
                keybd_event(A_key, 0, KEYEVENTF_EXTENDEDKEY, (IntPtr)0);
                //放開鍵盤A鍵
                keybd_event(A_key, 0, KEYEVENTF_KEYUP, (IntPtr)0);
                //程序暫停1000ms
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
