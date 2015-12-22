using System.Collections.Generic;
using System.Linq;
using Td.AspNet.Upload;

namespace Td.Kylin.WebApi.FileUpload
{
    /// <summary>
    /// Kylin文件上传器
    /// </summary>
    public class KylinUploader
    {
        /// <summary>
        /// 文件上传器初始化实例
        /// </summary>
        /// <param name="context">文件上传上下文实例</param>
        /// <param name="startBoot">上传后文件根路径
        /// <para>如：文件路径为 upload/product/2312.gif，根路径为“upload”</para>
        /// </param>
        /// <param name="startReplace">上传后文件根路径替换字符串
        /// <para>如：mall，即将“upload/product/2312.gif”替换成为“mall/product/2312.gif”</para>
        /// </param>
        public KylinUploader(UploadContext context, string startBoot, string startReplace) : this(new[] { context }, startBoot, startReplace) { }

        /// <summary>
        /// 文件上传器初始化实例
        /// </summary>
        /// <param name="context">文件上传上下文实例集合</param>
        /// <param name="startBoot">上传后文件根路径
        /// <para>如：文件路径为 upload/product/2312.gif，根路径为“upload”</para>
        /// </param>
        /// <param name="startReplace">上传后文件根路径替换字符串
        /// <para>如：mall，即将“upload/product/2312.gif”替换成为“mall/product/2312.gif”</para>
        /// </param>
        public KylinUploader(IEnumerable<UploadContext> context, string startBoot, string startReplace)
        {
            _uploadContext = context;

            Save();
        }

        /// <summary>
        /// 上传后文件路径开始根路径
        /// </summary>
        private string _startBoot = null;

        /// <summary>
        /// 上传后文件路径开始根路径需替换的路径
        /// </summary>
        private string _startReplace = null;

        /// <summary>
        /// 保存文件
        /// </summary>
        private async void Save()
        {
            var results = await _uploadContext.Save();

            results.ConvertUploadResult(_startBoot, _startReplace);

            this.Results = results;
        }

        /// <summary>
        /// 待上传的文件上下文集合
        /// </summary>
        private IEnumerable<UploadContext> _uploadContext = null;

        /// <summary>
        /// 第一个上传结果，如果不存在时，返回默认值
        /// </summary>
        public UploadResult Result
        {
            get
            {
                return null != Results ? Results.FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// 返回上传后的结果
        /// </summary>
        public List<UploadResult> Results { get; private set; }
    }
}
