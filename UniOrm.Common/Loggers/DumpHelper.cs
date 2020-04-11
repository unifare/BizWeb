/*
 * ************
 * file:	    DumpHelper.cs
 * creator:	    Cai Huan(huan.cai@philips.com)
 * date:	    2014-07
 * description:	Implement the functionality of creating a mini dump file
 * ************
 */

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices; 

namespace UniOrm
{
    public static class DumpHelper
    {
        private const string LOGGER_NAME = "DumpHelper";

        static class MinidumpType
        {
            public const int MiniDumpNormal = 0x00000000;
            public const int MiniDumpWithDataSegs = 0x00000001;
            public const int MiniDumpWithFullMemory = 0x00000002;
            public const int MiniDumpWithHandleData = 0x00000004;
            public const int MiniDumpFilterMemory = 0x00000008;
            public const int MiniDumpScanMemory = 0x00000010;
            public const int MiniDumpWithUnloadedModules = 0x00000020;
            public const int MiniDumpWithIndirectlyReferencedMemory = 0x00000040;
            public const int MiniDumpFilterModulePaths = 0x00000080;
            public const int MiniDumpWithProcessThreadData = 0x00000100;
            public const int MiniDumpWithPrivateReadWriteMemory = 0x00000200;
            public const int MiniDumpWithoutOptionalData = 0x00000400;
            public const int MiniDumpWithFullMemoryInfo = 0x00000800;
            public const int MiniDumpWithThreadInfo = 0x00001000;
            public const int MiniDumpWithCodeSegs = 0x00002000;
        }

        [DllImport("dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 processId, IntPtr hFile, int dumpType, IntPtr exceptionParam, IntPtr userStreamParam, IntPtr callackParam);

        /// <summary>
        /// Create a dump file for specified process with specified path
        /// </summary>
        /// <param name="process">process need to create dump file for </param>
        /// <param name="path">dump file path, it will be append '.dmp' automatically</param>
        public static void CreateMiniDump(System.Diagnostics.Process process, string path)
        {
            using (var fs = new FileStream(path + ".dmp", FileMode.Create))
            {
                if (fs.SafeFileHandle == null)
                {
                    throw new NullReferenceException("fs.SafeFileHandle");
                }
                    
                MiniDumpWriteDump(process.Handle, process.Id,
                                  fs.SafeFileHandle.DangerousGetHandle(),
                                  MinidumpType.MiniDumpNormal,
                                  IntPtr.Zero,
                                  IntPtr.Zero,
                                  IntPtr.Zero);
            }
        }

        /// <summary>
        /// Create a dump file for current process with specified path
        /// </summary>
        /// <param name="path">dump file path, it will be append '.dmp' automatically</param>
        public static void CreateMiniDump(string path)
        {
            using (var process = System.Diagnostics.Process.GetCurrentProcess())
            {
                CreateMiniDump(process, path);
            }
        }

        public static void CreateOutOfMemoryDump()
        {
            var currentFile = new FileInfo(Assembly.GetCallingAssembly().Location);
            if (currentFile.Directory == null)
            {
                Logger.LogError(LOGGER_NAME, "We want to create a dump file, but we can't get the related path info.");
                return;
            }
            var dir = currentFile.Directory.FullName + Path.DirectorySeparatorChar.ToString() + "Dumps";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var shortName = "OutMemory" + DateTime.Now.ToString("_yyyyMMddHHmmss");
            var fullPath = Path.Combine(dir, shortName);

            CreateFullDump(fullPath);
            Logger.LogError(LOGGER_NAME, "Create a full dump file: {0}", fullPath);
        }

        public static void CreateMemoryPeakDump(int iFlag)
        {
            var currentFile = new FileInfo(Assembly.GetCallingAssembly().Location);
            if (currentFile.Directory == null)
            {
                Logger.LogError(LOGGER_NAME, "We want to create a dump file, but we can't get the related path info.");
                return;
            }
            var dir = currentFile.Directory.FullName + Path.DirectorySeparatorChar.ToString() + "Dumps";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var shortName = "_90";
            if (iFlag == 2)
            {
                shortName = "_94";
            }
            shortName = "PeakMemory" + shortName;
            var fullPath = Path.Combine(dir, shortName);

            CreateFullDump(fullPath);
            Logger.LogError(LOGGER_NAME, "Create a full dump file: {0}", fullPath);
        }

        public static void CreateCMSPeakDump(int iFlag)
        {
            var currentFile = new FileInfo(Assembly.GetCallingAssembly().Location);
            if (currentFile.Directory == null)
            {
                Logger.LogError(LOGGER_NAME, "We want to create a dump file, but we can't get the related path info.");
                return;
            }
            var dir = currentFile.Directory.FullName + Path.DirectorySeparatorChar.ToString() + "Dumps";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var shortName = "_800";
            if (iFlag == 2)
            {
                shortName = "_1100";
            }
            shortName = "CMSMemory" + shortName;
            var fullPath = Path.Combine(dir, shortName);

            CreateFullDump(fullPath);
            Logger.LogError(LOGGER_NAME, "Create a full dump file: {0}", fullPath);
        }

        public static void CreateFullDump(string path)
        {
            using (var process = System.Diagnostics.Process.GetCurrentProcess())
            {
                using (var fs = new FileStream(path + ".dmp", FileMode.Create))
                {
                    if (fs.SafeFileHandle == null)
                    {
                        throw new NullReferenceException("fs.SafeFileHandle");
                    }

                    MiniDumpWriteDump(process.Handle, process.Id,
                                      fs.SafeFileHandle.DangerousGetHandle(),
                                      MinidumpType.MiniDumpWithFullMemory,
                                      IntPtr.Zero,
                                      IntPtr.Zero,
                                      IntPtr.Zero);
                }
            }
        }

        public static void CreateFullDump()
        {
            var currentFile = new FileInfo(Assembly.GetCallingAssembly().Location);
            if (currentFile.Directory == null)
            {
                Logger.LogError(LOGGER_NAME, "We want to create a dump file, but we can't get the related path info.");
                return;
            }
            var dir = currentFile.Directory.FullName + Path.DirectorySeparatorChar.ToString() + "Dumps";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var shortName = currentFile.Name + DateTime.Now.ToString("_yyyyMMddHHmmss");
            var fullPath = Path.Combine(dir, shortName);

            CreateFullDump(fullPath);
            Logger.LogError(LOGGER_NAME, "Create a full dump file: {0}", fullPath);
        }

        /// <summary>
        /// Create a dump file for current process
        /// </summary>
        public static void CreateMiniDump()
        {
            var currentFile = new FileInfo(Assembly.GetCallingAssembly().Location);
            if (currentFile.Directory == null)
            {
                Logger.LogError(LOGGER_NAME, "We want to create a dump file, but we can't get the related path info.");
                return;
            }
            var dir = currentFile.Directory.FullName + Path.DirectorySeparatorChar.ToString() + "Dumps";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var shortName = currentFile.Name + DateTime.Now.ToString("_yyyyMMddHHmmss");
            var fullPath = Path.Combine(dir, shortName);

            CreateMiniDump(fullPath);
            Logger.LogError(LOGGER_NAME, "Create a mini dump file: {0}", fullPath);
        }
    }
}
