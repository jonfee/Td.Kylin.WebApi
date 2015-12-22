using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Td.AspNet.Upload;

namespace Td.Kylin.WebApi.FileUpload
{
    /// <summary>
    /// 上传扩展类
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 获取最终文件上传的文件夹路径
        /// </summary>
        /// <param name="uploadBoot">文件上传的根（如：upload）</param>
        /// <param name="uploadFolder">从上传根后指定的文件夹路径部分（如：“mall/product/”，不存在时自动存储在other文件夹）</param>
        /// <param name="needDate">是否需要年月日作为路径的一部分（如：15/12/22）</param>
        /// <returns></returns>
        public static string GetUploadFolder(this string uploadFolder,string uploadBoot,  bool needDate = false)
        {
            string path = string.Empty;

            if (string.IsNullOrWhiteSpace(uploadFolder))
            {
                uploadFolder = "other";
            }

            path = Path.Combine(uploadBoot, uploadFolder);

            if (needDate)
            {
                path = Path.Combine(path, DateTime.Now.ToString("yy/MM/dd"));
            }

            return path;
        }

        /// <summary>
        /// 转换上传后的文件结果
        /// </summary>
        /// <param name="result">上传后的结果集</param>
        /// /// <param name="startBoot">文件路径起始字符</param>
        /// <param name="startReplace">文件路径起始字符要替换的字符</param>
        public static void ConvertUploadResult(this IEnumerable<UploadResult> result, string startBoot, string startReplace)
        {
            if (string.IsNullOrWhiteSpace(startBoot)) return;

            if (string.IsNullOrWhiteSpace(startReplace)) return;

            string boot = startBoot;
            if (!boot.EndsWith("/")) boot = boot + "/";

            string replace = startReplace;
            if (!replace.EndsWith("/")) replace = replace + "/";

            if (null != result && result.Count() > 0)
            {
                foreach (var rst in result)
                {
                    if (rst.FilePath.TrimStart('/').StartsWith(boot, StringComparison.OrdinalIgnoreCase))
                    {
                        rst.FilePath = replace + rst.FilePath.Remove(0, boot.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 转换上传后文件结果
        /// </summary>
        /// <param name="result">上传结果</param>
        /// <param name="startBoot">文件路径起始字符</param>
        /// <param name="startReplace">文件路径起始字符要替换的字符</param>
        public static void ConvertUploadResult(this UploadResult result, string startBoot, string startReplace)
        {
            if (null != result)
            {
                string boot = startBoot;
                if (!boot.EndsWith("/")) boot = boot + "/";

                string replace = startReplace;
                if (!replace.EndsWith("/")) replace = replace + "/";

                if (result.FilePath.TrimStart('/').StartsWith(boot, StringComparison.OrdinalIgnoreCase))
                {
                    result.FilePath = replace + result.FilePath.Remove(0, boot.Length);
                }
            }
        }
    }
}
