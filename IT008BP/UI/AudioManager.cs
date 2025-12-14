using System;
using System.Collections.Generic;
using System.IO;
using WMPLib;

namespace UI
{
    internal static class AudioManager
    {
        // Dictionary lưu tất cả âm thanh
        private static Dictionary<string, WindowsMediaPlayer> sounds = new Dictionary<string, WindowsMediaPlayer>();

        // Âm lượng mặc định theo nhóm
        private static int volumeMenuBGM = 30;
        private static int volumeSFX = 100;

        // Load nhạc Menu và BGM
        public static void LoadSounds()
        {
            LoadFolder(@"Sounds\MENU");
            LoadFolder(@"Sounds\BGM");
            LoadFolder(@"Sounds\SFX");
            LoadFolder(@"Sounds\AllClear");
            LoadFolder(@"Sounds\GameOver");
        }

        // Load folder cụ thể
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
                    player.settings.setMode("loop", false); // mặc định không lặp

                    // Áp dụng âm lượng theo nhóm
                    if (relativePath.ToLower().Contains("sfx"))
                        player.settings.volume = volumeSFX;
                    else
                        player.settings.volume = volumeMenuBGM;
                    
                    sounds[name] = player;
                }
            }
        }

        // Load từ file cụ thể
        public static void LoadFile(string relativePath, string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath, fileName);
            if (File.Exists(path))
            {
                string name = Path.GetFileNameWithoutExtension(fileName).ToLower();
                if (!sounds.ContainsKey(name))
                {
                    var player = new WindowsMediaPlayer();
                    player.URL = path;
                    player.settings.setMode("loop", false);

                    // Áp dụng âm lượng theo nhóm
                    if (relativePath.ToLower().Contains("sfx"))
                        player.settings.volume = volumeSFX;
                    else
                        player.settings.volume = volumeMenuBGM;

                    sounds[name] = player;
                }
            }
        }

        // Phát nhạc 1 lần
        public static void Play(string name)
        {
            name = name.ToLower();
            if (sounds.ContainsKey(name))
            {
                sounds[name].controls.stop();
                sounds[name].controls.play();
            }
        }

        // Phát nhạc lặp (looping)
        public static void PlayLooping(string name)
        {
            name = name.ToLower();
            if (sounds.ContainsKey(name))
            {
                sounds[name].settings.setMode("loop", true);
                sounds[name].controls.stop();
                sounds[name].controls.play();
            }
        }

        // Dừng nhạc
        public static void Stop(string name)
        {
            name = name.ToLower();
            if (sounds.ContainsKey(name))
            {
                sounds[name].controls.stop();
            }
        }

        // Dừng tất cả âm thanh
        public static void StopAll()
        {
            foreach (var player in sounds.Values)
                player.controls.stop();
        }
    }
}
