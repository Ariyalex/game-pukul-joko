# Game Design Document: Pukul Joko

## 1. Ringkasan Eksekutif
**Nama Game:** Pukul Joko  
**Genre:** Whack-a-Mole (Arcade)  
**Platform:** Android (Mobile)  
**Target Pemain:** Pemain kasual yang menyukai tantangan reaksi cepat.

## 2. Deskripsi Game
"Pukul Joko" adalah permainan arkade di mana pemain harus memukul target yang muncul dari lubang-lubang tertentu dalam batas waktu yang ditentukan. Pemain harus menghindari memukul jebakan (bom) untuk bertahan hidup hingga waktu habis.

## 3. Mekanik Gameplay
### 3.1. Inti Permainan (Core Loop)
Muncul Target -> Pemain Memukul (Tap) -> Skor Bertambah -> Kesulitan Meningkat -> Sesi Berakhir.

### 3.2. Kontrol
- **Tap/Klik:** Pemain menyentuh layar pada target yang muncul untuk memukulnya.

### 3.3. Jenis Target (Whackable)
| Jenis | Deskripsi | Efek |
| :--- | :--- | :--- |
| **Normal** | Target standar yang muncul satu per satu. | +10 Skor |
| **Penalty (Bom)** | Jebakan yang tidak boleh dipukul. | +1 Strike (Penalti) |
| **Tough** | Target yang lebih kuat. | Membutuhkan 2 kali pukulan |

### 3.4. Aturan Permainan
- **Waktu:** Setiap sesi permainan berlangsung selama 30 detik.
- **Sistem Strike (Nyawa):** Pemain memiliki batas maksimal 3 strike. Memukul bom akan menambah 1 strike.
- **Kondisi Menang:** Bertahan hingga waktu habis dengan strike kurang dari 3.
- **Kondisi Kalah:** Terkena 3 strike (memukul bom 3 kali).

## 4. Sistem Progresi dan Kesulitan
### 4.1. Tingkat Kesulitan (Difficulty)
- **Normal:** Kecepatan kemunculan target standar.
- **Hard:** Kecepatan kemunculan target 1.6x lebih cepat (Multiplier 0.6x pada interval).

### 4.2. Tahapan (Stages)
- **Stage 1:** Hanya target Normal yang muncul.
- **Stage 2:** Target Normal dan Bom mulai bermunculan.
- **Stage 3:** Kombinasi target Normal, Bom, dan Tough.

## 5. Antarmuka Pengguna (UI)
- **Main Menu:** Pilihan untuk memulai permainan, memilih Stage (1-3), dan memilih tingkat kesulitan.
- **Gameplay HUD:** Menampilkan sisa waktu, skor saat ini, indikator stage, dan jumlah strike (ikon nyawa).
- **Game Over Screen:** Menampilkan hasil akhir (Menang/Kalah), skor total, tombol *Retry*, dan tombol kembali ke *Menu*.

## 6. Audio dan Visual
### 6.1. Visual
- Menggunakan perspektif 2D dengan sistem *layering* (HoleBack, Object, HoleFront) untuk mensimulasikan target keluar dari lubang.
- Efek partikel muncul saat target berhasil dipukul.

### 6.2. Audio
- **BGM:** Musik latar yang ceria di menu dan musik yang lebih intens saat gameplay.
- **SFX:** Suara pukulan (Hit), suara target muncul (Appear), dan suara tanda stage dimulai.

## 7. Struktur Teknis
- **GameManager:** Mengatur state permainan (Intro, Playing, Game Over).
- **SpawnManager:** Mengatur logika kemunculan objek secara acak pada grid lubang.
- **GameSession:** Menyimpan data antar scene menggunakan pola *Singleton*.
- **ScriptableObjects:** Digunakan untuk konfigurasi Stage, Difficulty, dan properti target.
