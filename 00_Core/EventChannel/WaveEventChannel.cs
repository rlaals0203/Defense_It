namespace _01_Script._00_Core.EventChannel
{
    public class WaveEvntChannel
    {
        public static readonly WaveChangeEvent WaveChangeEvent = new WaveChangeEvent();
        public static readonly GameCompleteEvent GameCompleteEvent = new GameCompleteEvent();
        public static readonly CanWaveSkipEvent CanWaveSkipEvent = new CanWaveSkipEvent();
        public static readonly WaveSkipEvent WaveSkipEvent = new WaveSkipEvent();
    }
    
    public class WaveChangeEvent : GameEvent
    {
        public int currentWave;
    
        public WaveChangeEvent Initializer(int wave)
        {
            currentWave = wave;
            return this;
        }
    }
    
    public class CanWaveSkipEvent : GameEvent { }

    public class WaveSkipEvent : GameEvent
    {
        public int leftTime;
        public WaveSkipEvent Initializer(int time)
        {
            leftTime = time;
            return this;
        }
    }
    public class GameCompleteEvent : GameEvent { }
}