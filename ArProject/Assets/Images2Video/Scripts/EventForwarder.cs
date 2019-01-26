/// <summary>
/// Ref : http://jacksondunstan.com/articles/2922
/// Utility component to add to game objects whose events you want forwarded from Unity's message
/// system to standard C# events. Handles all events as of Unity 4.6.1.
/// <summary>
/// <author>
/// Jackson Dunstan
/// </author>

using UnityEngine;

namespace tw.com.championtek
{
    public class EventForwarder : MonoBehaviour
    {
		public delegate void EventHandler2<TParam1, TParam2>(TParam1 param1, TParam2 param2);

        public event EventHandler2<float[], int> OnAudioFilterReadEvent = (data, channels) => { };

        public void OnAudioFilterRead(float[] data, int channels)
        {
            OnAudioFilterReadEvent(data, channels);
        }
    }
}