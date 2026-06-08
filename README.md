# Pukul Joko - 2D Whack-a-Mole Game

**Pukul Joko** adalah sebuah proyek game arcade sederhana berbasis Unity yang mengadaptasi mekanik permainan klasik "Whack-a-Mole". Pemain ditantang untuk memukul karakter "Joko" yang muncul dari lubang-lubang tanah secepat mungkin dalam batas waktu tertentu.

## 🚀 Fitur Utama
- **Sistem Stage**: Terdapat 3 pilihan Stage dengan komposisi target yang berbeda (Normal, Bom, dan Tough).
- **Tingkat Kesulitan**: Pilihan mode **Normal** dan **Hard** yang mempengaruhi kecepatan interval spawn dan durasi kemunculan target.
- **Tipe Target**:
  - **Normal**: Target standar (1 hit).
  - **Tough**: Target yang membutuhkan 2 hit untuk dikalahkan.
  - **Penalty (Bom)**: Objek yang jika dipukul akan mengurangi nyawa (Strike).
- **Visual & Audio**: Menggunakan animasi halus dari **DOTween** dan sistem audio yang dinamis.

## 🛠️ Detail Teknis
- **Unity Version**: 6000.4.8f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Input System**: New Input System
- **Dependencies**: 
  - DOTween (Demigiant) - Untuk animasi UI dan pergerakan objek.

## 🎮 Cara Menjalankan Project
1. Pastikan Anda menggunakan versi Unity yang sesuai (**6000.4.8f1**).
2. Clone atau buka folder project ini di Unity Hub.
3. Di dalam Unity Editor, navigasikan ke folder `Assets/Scenes/`.
4. Buka scene **MenuScene.unity**.
5. Tekan tombol **Play** di bagian atas Editor.
6. Gunakan mouse (Klik Kiri) atau layar sentuh untuk memilih Stage, Difficulty, dan mulai memukul target.

## 📁 Struktur Folder Penting
- `Assets/Scripts/Core`: Berisi logika dasar seperti GameManager dan Session.
- `Assets/Scripts/Gameplay`: Berisi logika mekanik memukul, spawning, dan lubang.
- `Assets/Scripts/Configs`: ScriptableObjects untuk pengaturan Stage dan Difficulty.
- `Assets/Docs`: Berisi dokumen desain game (GDD) yang lebih rinci.

## 📝 Catatan
Game ini dirancang untuk platform Android (Mobile), namun dapat dijalankan dengan baik di Editor menggunakan simulasi mouse.
