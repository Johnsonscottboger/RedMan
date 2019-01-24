using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RedMan.Extensions
{
    /// <summary>
    /// 包含上传文件的一些方法
    /// </summary>
    public class UploadFile
    {
        #region - Private -
        private readonly IHostingEnvironment _env;
        #endregion

        /// <summary>
        /// 初始化<see cref="UploadFile"/>实例
        /// </summary>
        public UploadFile() { }

        /// <summary>
        /// 使用指定的<see cref="IHostingEnvironment"/>实例初始化<see cref="UploadFile"/>实例
        /// </summary>
        /// <param name="env">指定的<see cref="IHostingEnvironment"/>实例</param>
        public UploadFile(IHostingEnvironment env)
        {
            this._env = env;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="image">指定上传的图片</param>
        /// <param name="fileName">指定的文件名</param>
        /// <returns>文件路径</returns>
        public async Task<string> UploadImage(IFormFile image, string fileName)
        {
            var path = GetDefaultFilePath();
            var filePath = path + fileName;
            var fullPath = $"{this._env.WebRootPath}{filePath.Replace("/", @"\")}";
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(fs);
            }
            return filePath;
        }
        
        /// <summary>
        /// 获取文件默认保存路径
        /// </summary>
        /// <returns>文件路径</returns>
        private string GetDefaultFilePath()
        {
            var directory = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder().AddJsonFile($"{directory}/appsettings.json", true, true).Build();
            var filePath = configuration.GetSection("DefaultFilePath");
            var avatarPath = filePath.GetValue(typeof(string), "AvatarPath");
            return (string)avatarPath;
        }
    }
}
