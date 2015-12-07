using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Sample1
{
    /// <summary>
    /// ディスク情報を収集するクラス
    /// </summary>
    public class DiskInfoCollector
    {
        /// <summary>
        /// 収集結果を格納しているDictionary
        /// キーはデバイスパス
        /// </summary>
        public IDictionary<string, IDictionary<string, string>> Result = new Dictionary<string, IDictionary<string, string>>();


        /// <summary>
        /// hdCollection に指定のキーがあればその値を返し、なければ新規Dictionaryを登録して返す
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IDictionary<string, string> _GetOrCreateDictionary(string key)
        {
            if (Result.ContainsKey(key))
            {
                return Result[key];
            }
            var dic = new Dictionary<string, string>();
            Result[key] = dic;
            return dic;
        }

        private void _Collect(string tableName, string keyName)
        {
            var searcher = new ManagementObjectSearcher($"SELECT * FROM {tableName}");

            foreach (ManagementObject obj in searcher.Get())
            {
                string key = obj[keyName].ToString();
                var dic = _GetOrCreateDictionary(key);
                foreach (var kv in obj.Properties) { dic[kv.Name] = kv.Value?.ToString(); }
            }
        }

        /// <summary>
        /// Win32_DiskDriveより情報収集
        /// </summary>
        public void CollectDiskDrive() { _Collect("Win32_DiskDrive", "DeviceId"); }

        /// <summary>
        /// Win32_PhysicalMediaより情報収集
        /// </summary>
        public void CollectPhysicalMedia() { _Collect("Win32_PhysicalMedia", "Tag"); }
    }
}
