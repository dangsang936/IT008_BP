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

        private static int bgmVolume = 30;
        private static int sfxVolume = 70;

        private static bool bgmMuted = false;
        private static bool sfxMuted = false;

        public static void LoadSounds()
        {
            LoadFolder(@"Sounds\MENU");
            LoadFolder(@"Sounds\BGM");
            LoadFolder(@"Sounds\SFX");
            LoadFolder(@"Sounds\AllClear");
            LoadFolder(@"Sounds\GameOver");
        }

        private static void LoadFolder(string relativePath)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!Directory.Exists(path)) return;

            foreach (var file in Directory.GetFiles(path, "*.mp3"))
            {
                string name = Path.GetFileNameWithoutExtension(file).ToLower();
                if (sounds.ContainsKey(name)) continue;

                var player = new WindowsMediaPlayer();
                player.URL = file;
                player.settings.setMode("loop", false);

                if (file.ToLower().Contains(@"\bgm\") || file.ToLower().Contains(@"\menu\"))
                    player.settings.volume = bgmVolume;
                else
                    player.settings.volume = sfxVolume;

                sounds[name] = player;
            }
        }

        public static void Play(string name)
        {
            name = name.ToLower();
            if (!sounds.ContainsKey(name)) return;

            var s = sounds[name];

            if (sfxMuted && s.URL.ToLower().Contains(@"\sfx\")) return;
            if (bgmMuted && (s.URL.ToLower().Contains(@"\bgm\") || s.URL.ToLower().Contains(@"\menu\"))) return;

            s.controls.stop();
            s.controls.play();
        }

        public static void PlayLooping(string name)
        {
            name = name.ToLower();
            if (!sounds.ContainsKey(name)) return;

            var s = sounds[name];

            if (sfxMuted && s.URL.ToLower().Contains(@"\sfx\")) return;
            if (bgmMuted && (s.URL.ToLower().Contains(@"\bgm\") || s.URL.ToLower().Contains(@"\menu\"))) return;

            s.settings.setMode("loop", true);
            s.controls.stop();
            s.controls.play();
        }

        public static void ToggleBGM()
        {
            bgmMuted = !bgmMuted;

            foreach (var s in sounds.Values)
            {
                if (s.URL.ToLower().Contains(@"\bgm\") ||
                    s.URL.ToLower().Contains(@"\menu\"))
                {
                    s.settings.volume = bgmMuted ? 0 : bgmVolume;
                }
            }
        }

        public static void ToggleSFX()
        {
            sfxMuted = !sfxMuted;

            foreach (var s in sounds.Values)
            {
                if (s.URL.ToLower().Contains(@"\sfx\"))
                {
                    s.settings.volume = sfxMuted ? 0 : sfxVolume;
                }
            }
        }

        public static bool IsBGMMuted() => bgmMuted;
        public static bool IsSFXMuted() => sfxMuted;

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

        public static string currentBGM;

        public static void PlayBGM(string name)
        {
            if (currentBGM == name) return;

            StopAll();
            PlayLooping(name);
            currentBGM = name;
        }
    }
}
