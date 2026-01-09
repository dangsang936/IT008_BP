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

        // Chỉnh âm lượng 1 sound (0–100)
        public static void SetVolume(string name, int volume)
        {
            name = name.ToLower();
            volume = Math.Max(0, Math.Min(100, volume));

            if (sounds.ContainsKey(name))
            {
                sounds[name].settings.volume = volume;
            }
        }

        public static void StopAll()
        {
            foreach (var player in sounds.Values)
            {
                player.controls.stop();
            }
        }


        // Dừng tất cả SFX (không dừng BGM / Menu)
        public static void MuteSFX()
        {
            foreach (var player in sounds.Values)
            {
                if (player.URL.ToLower().Contains(@"\sfx\"))
                {
                    player.controls.stop();
                }
            }
        }
        public static void Stop(string name) { name = name.ToLower(); if (sounds.ContainsKey(name)) { sounds[name].controls.stop(); } }

    }
}
