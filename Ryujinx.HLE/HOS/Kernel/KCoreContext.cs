namespace Ryujinx.HLE.HOS.Kernel
{
    class KCoreContext
    {
        private KScheduler Scheduler;

        private HleCoreManager CoreManager;

        public bool ContextSwitchNeeded { get; private set; }

        public KThread CurrentThread  { get; private set; }
        public KThread SelectedThread { get; private set; }

        public KCoreContext(KScheduler Scheduler, HleCoreManager CoreManager)
        {
            this.Scheduler   = Scheduler;
            this.CoreManager = CoreManager;
        }

        public void SelectThread(KThread Thread)
        {
            SelectedThread = Thread;

            ContextSwitchNeeded = true;
        }

        public void UpdateCurrentThread()
        {
            ContextSwitchNeeded = false;

            CurrentThread = SelectedThread;
        }

        public void ContextSwitch()
        {
            ContextSwitchNeeded = false;

            if (CurrentThread != null)
            {
                CoreManager.GetThread(CurrentThread.Thread.Work).Pause();
            }

            CurrentThread = SelectedThread;

            if (CurrentThread != null)
            {
                CurrentThread.ClearExclusive();

                CoreManager.GetThread(CurrentThread.Thread.Work).Unpause();

                CurrentThread.Thread.Execute();
            }
        }

        public void RemoveThread(KThread Thread)
        {
            //TODO.
        }
    }
}