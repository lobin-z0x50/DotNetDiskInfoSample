using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample1
{
    class Program
    {
        static void Main(string[] args)
        {
            var collector = new DiskInfoCollector();

            collector.CollectDiskDrive();        // Win32_DiskDrive
            collector.CollectPhysicalMedia();    // Win32_PhysicalMedia

            // 結果を出力
            foreach (var deviceId in collector.Result.Keys)
            {
                System.Console.WriteLine($"--- {deviceId} -----");

                var hdi = collector.Result[deviceId];
                foreach (var key in hdi.Keys)
                {
                    System.Console.WriteLine($"{key}: {hdi[key]}");
                }

            }

            Console.WriteLine();


            // 物理ディスクのみ抽出
            var physicalDrives = collector.Result.Keys.Where(key => key.Contains("PHYSICALDRIVE"));

            // 1つ目の物理ディスクドライブのシリアル番号
            if (physicalDrives.Any())
            {
                Console.WriteLine("First physical drive = " + physicalDrives.First());
                var dic = collector.Result[physicalDrives.First()];
                Console.WriteLine("Serial Number = " + dic["SerialNumber"]);
            }
            else
            {
                Console.WriteLine("No physical drive found!");
            }

            // wait
            Console.ReadLine();
        }
    }
}
