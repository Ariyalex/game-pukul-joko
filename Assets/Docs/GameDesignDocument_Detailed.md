# Game Design Document: Pukul Joko (Versi Detil & Spesifik)

## 1. Identitas Game
*   **Judul:** Pukul Joko
*   **Genre:** 2D Arcade / Whack-a-Mole
*   **Platform:** Android (Mobile)
*   **Engine:** Unity 6000.4.8f1
*   **Visual Style:** 2D Stylized dengan efek DOTween

---

## 2. Ringkasan Gameplay (High-Level)
"Pukul Joko" adalah game aksi reaksi cepat di mana pemain harus memukul karakter "Joko" yang muncul dari lubang-lubang tanah. Pemain harus mengumpulkan skor setinggi mungkin dalam waktu 30 detik sambil menghindari bom (Penalty). Kesulitan game bersifat dinamis berdasarkan pilihan tingkat kesulitan dan stage yang dipilih oleh pemain di menu utama.

---

## 3. Mekanik Gameplay Inti (Core Mechanics)

### 3.1. Sistem Kendali (Input)
*   **Metode:** Tap (Touch) pada layar mobile atau Klik Kiri pada Mouse.
*   **Deteksi:** `Physics2D.Raycast` pada Layer "Default".
*   **Responsivitas:** Input harus dideteksi secara instan pada frame yang sama saat sentuhan terjadi.

### 3.2. Karakteristik Target (Whackable)
Semua target memiliki siklus hidup: **Hidden -> Rising -> Showing -> Hiding -> Hidden**.

| Tipe Target | Pukulan Dibutuhkan | Skor | Efek Khusus |
| :--- | :---: | :---: | :--- |
| **Normal (Joko)** | 1 | +10 | Animasi hit standar. |
| **Tough (Joko Helm)**| 2 | +25 | Shake pada hit pertama, hancur pada hit kedua. |
| **Penalty (Bom)** | 1 | 0 | +1 Strike (Penalti), Layar bergetar (Shake). |

### 3.3. Sistem Strike & Kondisi Kekalahan
*   Pemain memiliki **3 Jatah Strike** (Nyawa).
*   Memukul **Bom** mengakibatkan 1 Strike.
*   Game Over terjadi seketika jika Strike mencapai 3 sebelum waktu habis.
*   Reward: Menghindari Bom hingga ia masuk kembali ke lubang memberikan bonus +5 skor.

### 3.4. Sistem Spawning (Spawn Logic)
*   **Grid:** 8-9 lubang statis di arena.
*   **Algoritma:** Random selection pada lubang yang berstatus `IsAvailable`.
*   **Interval:** Waktu antar kemunculan target berkurang seiring stage yang lebih tinggi.

---

## 4. Konfigurasi Level & Progresi (Data-Driven)

### 4.1. Tingkat Kesulitan (Difficulty Settings)
Menggunakan pengali (multiplier) pada durasi kemunculan dan interval spawn:
*   **Normal:** Multiplier 1.0x (Kecepatan standar).
*   **Hard:** Multiplier 0.6x (Target muncul dan hilang 40% lebih cepat).

### 4.2. Desain Stage (Stage Design)
| Stage | Tipe Target yang Muncul | Fokus Tantangan |
| :--- | :--- | :--- |
| **Stage 1** | Normal | Pengenalan kontrol dan ritme. |
| **Stage 2** | Normal + Penalty | Ketelitian dalam membedakan target dan bom. |
| **Stage 3** | Normal + Penalty + Tough | Kecepatan reaksi tinggi dan manajemen target kuat. |

---

## 5. Antarmuka Pengguna (UI/UX)

### 5.1. Menu Utama
*   **Stage Selector:** Tombol interaktif untuk memilih Stage 1, 2, atau 3.
*   **Difficulty Toggle:** Pilihan Normal atau Hard yang mempengaruhi kalkulasi internal.
*   **Feedback Visual:** Tombol yang dipilih menggunakan `DOPunchScale` dan `DOMove` untuk memberikan kesan kedalaman.

### 5.2. Gameplay HUD
*   **Score Counter:** Menampilkan skor real-time dengan efek angka bergulir.
*   **Countdown Timer:** 30 detik hitung mundur. Berubah warna menjadi merah saat < 5 detik.
*   **Strike Meter:** 3 ikon nyawa (Hati/Indikator) yang meredup saat terkena pinalti.

---

## 6. Estetika Visual & Audio

### 6.1. Visual & Layering
Untuk menciptakan kedalaman 2D, digunakan sistem sorting layer:
1.  `Background`: Tanah dan latar belakang.
2.  `HoleBack`: Bagian belakang lubang.
3.  `Object (Whackable)`: Karakter Joko atau Bom (berada di antara lapisan lubang).
4.  `HoleFront`: Bagian depan lubang (memberikan efek target "keluar" dari dalam).

### 6.2. Audio Design
*   **Music:**
    *   `bgm_menu`: Santai, ceria.
    *   `bgm_gameplay`: Energetik, bertempo cepat.
*   **Sound Effects (SFX):**
    *   `Hit`: Suara benturan tumpul saat memukul Joko.
    *   `Explosion`: Suara ledakan kecil saat memukul Bom.
    *   `PopUp`: Suara gesekan saat target muncul dari lubang.

---

## 7. Implementasi Teknis (Technical Stack)
*   **State Machine:** `GameManager` mengontrol status `Intro`, `Playing`, dan `GameOver`.
*   **DOTween:** Digunakan untuk semua animasi UI dan pergerakan target (Rising/Hiding).
*   **ScriptableObjects:** Digunakan untuk menyimpan data `WhackableConfig`, `StageConfig`, dan `DifficultyConfig` agar mudah diatur oleh Designer tanpa mengubah kode.
*   **Singleton Pattern:** `GameSession` untuk menyimpan data pilihan pemain dari Menu ke Gameplay scene.
