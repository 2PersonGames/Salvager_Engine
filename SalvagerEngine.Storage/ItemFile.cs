using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalvagerEngine.Storage
{
    public abstract class ItemFile : BaseFile
    {
        // Variables

        public static string Folder { get; set; }

        private LockFile mLock;

        // Constructors

        static ItemFile()
        {
            Folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public ItemFile(string localFilePath)
            : base(Path.Combine(Folder, localFilePath))
        {
            MaxTries = 20;
            MaxWaitTime = 1000;
            mLock = new LockFile(Fullpath);
        }

        // Save

        public Task<Result> BeginSave()
        {
            // Create the save as a new task
            var task = new Task<Result>(delegate
            {
                try
                {
                    if (mLock.Lock() == Result.Success)
                    {
                        using (StreamWriter writer = new StreamWriter(Fullpath, false, Encoding.Unicode))
                        {
                            return TryAction(delegate { return Save(writer); });
                        }
                    }
                    else
                    {
                        return Result.TimeOut;
                    }
                }
                finally
                {
                    mLock.Unlock();
                }
            });

            // Start the task
            task.Start();

            // Return the task
            return task;
        }

        protected abstract bool Save(StreamWriter writer);

        // Load

        public Task<Result> BeginLoad()
        {
            // Create the load as a new task
            var task = new Task<Result>(delegate
            {
                try
                {
                    if (File.Exists(Fullpath))
                    {
                        if (mLock.Lock() == Result.Success)
                        {
                            using (StreamReader reader = new StreamReader(Fullpath))
                            {
                                return TryAction(delegate { return Load(reader); });
                            }
                        }
                        else
                        {
                            return Result.TimeOut;
                        }
                    }
                    else
                    {
                        return Result.FileDoesNotExist;
                    }
                }
                finally
                {
                    mLock.Unlock();
                }
            });

            // Start the task
            task.Start();

            // Return the task
            return task;
        }

        protected abstract bool Load(StreamReader reader);

        // Overrides

        public override void Dispose()
        {
            base.Dispose();
            mLock.Dispose();
        }
    }
}
