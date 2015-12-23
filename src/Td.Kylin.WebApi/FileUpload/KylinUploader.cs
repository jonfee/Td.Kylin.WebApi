using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Td.AspNet.Upload;

namespace Td.Kylin.WebApi.FileUpload
{
    /// <summary>
    /// Kylin文件上传器
    /// </summary>
    public class KylinUploader
    {
        #region 初始化实例

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
        /// <param name="context">文件上传上下文实例</param>
        /// <param name="startBoot">上传后文件根路径
        /// <para>如：文件路径为 upload/product/2312.gif，根路径为“upload”</para>
        /// </param>
        /// <param name="startReplace">上传后文件根路径替换字符串
        /// <para>如：mall，即将“upload/product/2312.gif”替换成为“mall/product/2312.gif”</para>
        /// </param>
        /// <param name="autoSave">是否自动上传，为false时需要手动调用Save()方法</param>
        public KylinUploader(UploadContext context, string startBoot, string startReplace, bool autoSave) : this(new[] { context }, startBoot, startReplace, autoSave) { }

        /// <summary>
        /// 文件上传器初始化实例并自动上传文件
        /// </summary>
        /// <param name="context">文件上传上下文实例集合</param>
        /// <param name="startBoot">上传后文件根路径
        /// <para>如：文件路径为 upload/product/2312.gif，根路径为“upload”</para>
        /// </param>
        /// <param name="startReplace">上传后文件根路径替换字符串
        /// <para>如：mall，即将“upload/product/2312.gif”替换成为“mall/product/2312.gif”</para>
        /// </param>
        public KylinUploader(IEnumerable<UploadContext> context, string startBoot, string startReplace) : this(context, startBoot, startReplace, true) { }

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
        /// <param name="autoSave">是否自动上传，为false时需要手动调用Save()方法</param>
        public KylinUploader(IEnumerable<UploadContext> context, string startBoot, string startReplace, bool autoSave)
        {
            _uploadContext = context;

            _startBoot = startBoot;

            _startReplace = startReplace;

            if (autoSave)
            {
               Save();
            }
        }

        #endregion

        #region 类私有变量

        /// <summary>
        /// 上传后文件路径开始根路径
        /// </summary>
        private string _startBoot = null;

        /// <summary>
        /// 上传后文件路径开始根路径需替换的路径
        /// </summary>
        private string _startReplace = null;

        /// <summary>
        /// 待上传的文件上下文集合
        /// </summary>
        private IEnumerable<UploadContext> _uploadContext = null;

        #endregion

        #region 文件上传

        /// <summary>
        /// 保存文件
        /// </summary>
        public void Save()
        {
            var results = _uploadContext.Save().Result;

            results.ConvertUploadResult(_startBoot, _startReplace);

            this.Results = results;
        }

        #endregion

        #region 上传结果

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

        #endregion
    }
}
