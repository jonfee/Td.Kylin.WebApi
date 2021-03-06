﻿namespace Td.Kylin.WebApi
{
    /// <summary>
    /// API返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 业务状态为true则程序出错
        /// </summary>
        public bool IsError
        {
            get
            {
                return Code != 0;
            }
        }

        /// <summary>
        /// 期望结果
        /// </summary>
        public T Content { get; set; }
    }
}
