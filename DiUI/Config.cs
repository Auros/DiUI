using System;

namespace DiUI
{
    public class Config
    {
        internal event Action<Config> Reloaded;

        public virtual void Changed()
        {
            Reloaded?.Invoke(this);
        }
    }
}