using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Arcane
{
    enum SoundType { Effect, Song }

    class Sound
    {
        private SoundEffect effect;
        private Song song;
        private SoundType type;

        public Sound(SoundType type, string soundName)
        {
            this.type = type;
            if (type == SoundType.Effect)
            {
                effect = Common.Content.Load<SoundEffect>(soundName);
                song = null;
            }
            else if (type == SoundType.Song)
            {
                song = Common.Content.Load<Song>(soundName);
                effect = null;
            }
        }

        public void Play()
        {
            if (type == SoundType.Effect)
            {
                effect.Play();
            }
            else if (type == SoundType.Song)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(song);
            }
        }

        public void Stop()
        {
            if (type == SoundType.Song)
            {
                MediaPlayer.Stop();
            }
        }


    }
}
