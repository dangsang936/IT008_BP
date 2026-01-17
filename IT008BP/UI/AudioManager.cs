using System;
using System.Collections.Generic;
using System.IO;
using WMPLib;

namespace UI
{
    internal static class AudioManager
    {
        private static Dictionary<string, WindowsMediaPlayer> sounds =
            new Dictionary<string, WindowsMediaPlayer>();

        // ====== VOLUME ======
        private static int bgmVolume = 30;
        private static int sfxVolume = 70;
        private static bool muted = false;

        // ====== LOAD ======
        public static void LoadSounds()
        {
            LoadFolder(@"Sounds\MENU");
            LoadFolder(@"Sounds\BGM");
            LoadFolder(@"Sounds\SFX");
            LoadFolder(@"Sounds\AllClear");
            LoadFolder(@"Sounds\GameOver");
        }

        public static void LoadFolder(string relativePath)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!Directory.Exists(path)) return;

            foreach (var file in Directory.GetFiles(path, "*.mp3"))
            {
                string name = Path.GetFileNameWithoutExtension(file).ToLower();
                if (!sounds.ContainsKey(name))
                {
                    var player = new WindowsMediaPlayer();
                    player.URL = file;
                    player.settings.setMode("loop", false);

                    // set volume mặc định
                    if (file.ToLower().Contains(@"\bgm\") || file.ToLower().Contains(@"\menu\"))
                        player.settings.volume = bgmVolume;
                    else
                        player.settings.volume = sfxVolume;

                    sounds[name] = player;
                }
            }
        }

        // ====== PLAY ======
        public static void Play(string name)
        {
            name = name.ToLower();
            if (!sounds.ContainsKey(name) || muted) return;

            sounds[name].controls.stop();
            sounds[name].controls.play();
        }

        public static void PlayLooping(string name)
        {
            name = name.ToLower();
            if (!sounds.ContainsKey(name) || muted) return;

            sounds[name].settings.setMode("loop", true);
            sounds[name].controls.stop();
            sounds[name].controls.play();
        }

        // ====== MUTE ======
        public static void ToggleMute()
        {
            muted = !muted;

            foreach (var s in sounds.Values)
            {
                s.settings.volume = muted ? 0 :
                    (s.URL.ToLower().Contains(@"\bgm\") || s.URL.ToLower().Contains(@"\menu\"))
                    ? bgmVolume
                    : sfxVolume;
            }
        }

        public static void Stop(string name)
        {
            name = name.ToLower();
            if (sounds.ContainsKey(name))
                sounds[name].controls.stop();
        }

        public static void StopAll()
        {
            foreach (var s in sounds.Values)
                s.controls.stop();
        }

        public static void MuteSFX()
        {
            foreach (var s in sounds.Values)
            {
                if (s.URL.ToLower().Contains(@"\sfx\"))
                    s.controls.stop();
            }
        }
    }
}
