using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ptilopsis.Utils
{
    public class ZipHelper
    {
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="sourcePath">压缩文件</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public static bool UnZip(string sourcePath,string targetPath)
        {
            if (!File.Exists(sourcePath))
            {
                return false;
            }
            using (ZipArchive archive = ZipFile.OpenRead(sourcePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Path.Combine(targetPath, entry.FullName);

                    if(path.EndsWith(@"/")|| path.EndsWith(@"\"))
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    else
                    {
                        FileInfo finfo = new FileInfo(path);
                        if (!Directory.Exists(finfo.DirectoryName))
                        {
                            Directory.CreateDirectory(finfo.DirectoryName);
                        }
                        finfo=null;
                        if (File.Exists(path))//如果文件存在删除
                        {
                            File.Delete(path);
                        }
                        entry.ExtractToFile(Path.Combine(targetPath, entry.FullName));
                    }
                }
            }
            return true;
        }
    }
    /// <summary>
    /// 一个压缩流程的对象
    /// </summary>
    public class ZipProcess
    {
        string SourcePath { get; set; }
        string TargetPath { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        public bool IsStart { get; set; } = false;
        /// <summary>
        /// 是否运行结束
        /// </summary>
        public bool IsOver { get; set; } = false;
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool? IsSuccess { get; set; } = null;
        private int successCount=0;
        private int AllCount = 1;
        private Task<bool> _task;
        public ZipProcess(string _sourceFile, string _targetPath, ZipType type=ZipType.UNZIP)
        {
            SourcePath = _sourceFile;
            TargetPath = _targetPath;
            _task = new Task<bool>(() => {
                IsStart = true;
                //ZipHelper zh = new ZipHelper();
                //return zh.UnZip(SourcePath, TargetPath,ref successCount,ref AllCount);
                if (type == ZipType.UNZIP)
                {
                    return UnZip(SourcePath, TargetPath);
                }
                else
                {
                    return ZipFiles(SourcePath, TargetPath);
                }
            });
            _task.ContinueWith(i=> {
                IsSuccess = i.Result;
                IsOver = true;
            });
        }
        public void Start()
        {
            _task.Start();
        }
        /// <summary>
        /// 获得解压/压缩进度
        /// </summary>
        /// <returns></returns>
        public double GetPercent()
        {
            double result= Convert.ToDouble(successCount) / Convert.ToDouble(AllCount);
            return result;
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="sourcePath">压缩文件</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        private bool UnZip(string sourcePath, string targetPath)
        {
            if (!File.Exists(sourcePath))
            {
                return false;
            }
            using (ZipArchive archive = ZipFile.OpenRead(sourcePath))
            {
                AllCount = archive.Entries.Count;
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string path = Path.Combine(targetPath, entry.FullName);

                    if (path.EndsWith(@"/") || path.EndsWith(@"\"))
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    else
                    {
                        FileInfo finfo = new FileInfo(path);
                        if (!Directory.Exists(finfo.DirectoryName))
                        {
                            Directory.CreateDirectory(finfo.DirectoryName);
                        }
                        finfo = null;
                        if (File.Exists(path))//如果文件存在删除
                        {
                            File.Delete(path);
                        }
                        entry.ExtractToFile(Path.Combine(targetPath, entry.FullName));
                    }
                    successCount++;
                }
            }
            return true;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourcePath">压缩文件夹</param>
        /// <param name="targetPath">储存的ZIP文件</param>
        /// <returns></returns>
        private bool ZipFiles(string sourcePath,string targetPath)
        {
            if (!Directory.Exists(sourcePath))
            {
                return false;
            }
            try
            {
                using (ZipArchive archive = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    var dinfo = new DirectoryInfo(sourcePath);
                    FileInfo[] files = GetAllFile(dinfo);
                    AllCount = files.Count();
                    foreach (var file in files)
                    {
                        string _path = file.FullName.Remove(0, dinfo.FullName.Count());
                        archive.CreateEntryFromFile(file.FullName, _path);
                        successCount++;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        private FileInfo[] GetAllFile(DirectoryInfo folder)
        {
            List<FileInfo> Result = new List<FileInfo>();
            foreach (var item in folder.GetFiles())
            {
                //Result.Add(item.FullName.Remove(0, folder.FullName.Count()));
                Result.Add(item);
            }
            foreach(var item in folder.GetDirectories())
            {
                Result.AddRange(GetAllFile(item));
            }
            return Result.ToArray();
        }

        public enum ZipType
        {
            ZIP,
            UNZIP
        }
    }
}
