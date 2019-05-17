using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace ABO.Core
{
    /// <summary>
    /// Contains helper methods only.
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Gets absolute path of given path. The given path might be a relative or an absolute path.
        /// </summary>
        /// <param name="folderPath">Relative or absolute path.</param>
        /// <returns>Absolute path.</returns>
        public static string GetAbsolutePath(string folderPath)
        {
            string path = string.Empty;
            if (Path.IsPathRooted(folderPath))// absolute path
            {
                path = folderPath;
            }
            else // relative path
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderPath);
            }
            return path;
        }

        /// <summary>
        /// Gets string containing address combined from adddress 1, 2, 3, etc.
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        public static string GetAddressString(params string[] addresses)
        {
            if (addresses == null)
                throw new ArgumentNullException("addresses");

            StringBuilder sb = new StringBuilder();
            foreach (string s in addresses)
            {
                if (!string.IsNullOrEmpty(s))
                    sb.Append(", " + s.Trim());
            }

            if (sb.Length > 0)
                sb.Remove(0, 2);

            return sb.ToString();
        }

        /// <summary>
        /// Gets full details of exception (including inner exceptions)
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetFullExceptionDetails(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Message: {0}\r\n{1}", ex.Message, ex.StackTrace);
            int index = 0;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                sb.AppendFormat("\r\n### InnerException #{0}:\r\n", ++index);
                sb.AppendFormat("Message: {0}\r\n{1}", ex.Message, ex.StackTrace);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the end of date by setting time to 23:59:59.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndOfDate(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        /// <summary>
        /// Gets the start of date by setting time to 00:00:00.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetStartOfDate(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }

        /// <summary>
        /// Checks if current user has specific permission on given folder.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="desiredRight"></param>
        /// <returns></returns>
        public static bool HasPermissionOnFolder(string folderPath, FileSystemRights desiredRight)
        {
            bool hasPermission = false;
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

            try
            {
                DirectorySecurity acl = Directory.GetAccessControl(folderPath);
                AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
                foreach (AuthorizationRule rule in rules)
                {
                    if (currentUser.User == rule.IdentityReference || currentUser.Groups.Contains(rule.IdentityReference)) // if rule's identity is user or any user's group
                    {
                        FileSystemAccessRule currentRule = (FileSystemAccessRule)rule;

                        if (currentRule.AccessControlType == AccessControlType.Deny && (currentRule.FileSystemRights & desiredRight) == desiredRight) // if Deny, return immediately
                            return false;

                        if (currentRule.AccessControlType == AccessControlType.Allow && (currentRule.FileSystemRights & desiredRight) == desiredRight) // if Allow, set flag
                            hasPermission = true;
                    }
                }
            }
            catch (UnauthorizedAccessException) { }

            return hasPermission;
        }
    }

    public static class UnicodeCharacterReplacer
    {
        private static string[] _unicodeChars = new string[]
        {
            "a|áàạảãâấầậẩẫăắằặẳẵ",
            "d|đ",
            "e|éèẹẻẽêếềệểễ",
            "i|íìịỉĩ",
            "o|óòọỏõôốồộổỗơớờợởỡ",
            "u|úùụủũưứừựửữ",
            "y|ýỳỵỷỹ"
        };

        private static Dictionary<char, char> _replacementDict;
        internal static Dictionary<char, char> ReplacementDict
        {
            get
            {
                if (_replacementDict == null)
                {
                    _replacementDict = new Dictionary<char, char>();
                    foreach (string chars in _unicodeChars)
                    {
                        string[] arr = chars.Split(new char[] { '|' });
                        foreach (char ch in arr[1])
                        {
                            _replacementDict[ch] = arr[0][0];
                        }
                    }
                }
                return _replacementDict;
            }
        }

        public static string ReplaceString(string input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, length = input.Length;i<length; i++)
            {
                char ch = input[i];
                char ch2 = ch;
                bool isUpper = char.IsUpper(ch);
                if (isUpper) ch2 = char.ToLower(ch);

                if (ReplacementDict.ContainsKey(ch2))
                    sb.Append(isUpper ? char.ToUpper(ReplacementDict[ch2]) : ReplacementDict[ch2]);
                else
                    sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
