using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ABO.Core
{
    public class AppSettings : IAppSettings
    {
        #region IAppSettings Members

        public string ProfileBoxFolder
        {
            get
            {
                //var setting = ConfigurationManager.AppSettings["ProfileBoxFolder"];
                //if (!string.IsNullOrEmpty(setting))
                //    return CommonHelper.GetAbsolutePath(setting);
                //return @"Data\ProfileBox"; // default value

                return GetAppSettingString("ProfileBoxFolder", false, @"Data\ProfileBox", (setting) => CommonHelper.GetAbsolutePath(setting));
            }
        }

        public string AS400DistributorDateFormat
        {
            get
            {
                return GetAppSettingString("AS400DistributorDateFormat", false, "yyMMdd");
            }
        }

        public string ProfileExportDateFormat
        {
            get
            {
                return GetAppSettingString("ProfileExportDateFormat", false, "ddMMyyyy");
            }
        }

        public bool DistributorUpdateWorkerEnabled
        {
            get { return GetAppSettingsBoolean("DistributorUpdateWorkerEnabled"); }
        }

        public bool ProfileScanWorkerEnabled
        {
            get { return GetAppSettingsBoolean("ProfileScanWorkerEnabled"); }
        }

        public bool DataPurgeWorkerEnabled
        {
            get { return GetAppSettingsBoolean("DataPurgeWorkerEnabled"); }
        }

        public int ExpiredDistributorDeletionPeriod
        {
            get { return GetAppSettingInt32("ExpiredDistributorDeletionPeriod", 1); }
        }

        public IList<string> TestUsers
        {
            get
            {
                var users = GetAppSettingString("TestUsers", defaultValue: "Tri Nguyen,Chi Cuong");
                if (!string.IsNullOrEmpty(users))
                    return new List<string>(users.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

                return new List<string>();
            }
        }

        public string TestUserReplacement
        {
            get { return GetAppSettingString("TestUserReplacement", defaultValue: "VNM00000"); }
        }

        public string ProfileBoxRootPath
        {
            get { return GetAppSettingString("ProfileBoxRootPath"); }
        }

        public int DefaultCacheDuration { get { return 30; } }

        public int DistributorUpdateBatchSize
        {
            get { return GetAppSettingInt32("DistributorUpdateBatchSize", 5000); }
        }

        public char[] DistributorDelimiterInProfileFile
        {
            get
            {
                var delimiter = GetAppSettingString("DistributorDelimiterInProfileFile");
                return string.IsNullOrEmpty(delimiter) ? new[] { '-' } : delimiter.ToArray();
            }
        }

        public int UploadAvatarWidth
        {
            get { return GetAppSettingInt32("UploadAvatarWidth", 400); }
        }

        public int UploadAvatarHeight
        {
            get { return GetAppSettingInt32("UploadAvatarHeight", 533); }
        }
        #endregion

        #region Utility
        private string GetAppSettingString(string key, bool isRequired = false, string defaultValue = null, Func<string, string> settingProcessor = null)
        {
            var setting = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(setting))
                return (settingProcessor == null) ? setting : settingProcessor(setting);

            if (isRequired)
                throw new ABOException(string.Format("The key '{0}' does not exist in configuration file OR the retrieved values is empty", key));

            if (!string.IsNullOrEmpty(defaultValue))
                return (settingProcessor == null) ? defaultValue : settingProcessor(defaultValue);

            return string.Empty;
        }

        private bool GetAppSettingsBoolean(string key, bool isRequired = false)
        {
            var setting = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(setting))
                return Convert.ToBoolean(setting);

            if (isRequired)
                throw new ABOException(string.Format("The key '{0}' does not exist in configuration file OR the retrieved values is empty", key));

            return false;
        }

        private int GetAppSettingInt32(string key, int defaultValue)
        {
            int value;
            if (int.TryParse(ConfigurationManager.AppSettings[key], out value))
                return value;
            return defaultValue;
        }

        private float GetAppSettingFloat(string key, float defaultValue)
        {
            float value;
            if (float.TryParse(ConfigurationManager.AppSettings[key], out value))
                return value;
            return defaultValue;
        }
        #endregion
    }
}
