# Tubes1_YaMainLastWarLah
Repository ini dibuat untuk memenuhi Tugas Besar 1 Mata Kuliah Strategi Algoritma IF 2211 Semester 2 Tahun Ajaran 2024/2025.
<b/>
## Implementasi Bot
1. Bot Mayor Teddy
   Bot ini memiliki algoritma greedy dalam pergerakannya karena bot ini selalu bergerak secara random dengan memperhitungkan lokasinya terhadap tiap sisi tembok dan arah bot tersebut menghadap, dengan demikian bot ini dapat menghindari tembok dan menghindari tembakan musuh agar dapat selamat hingga akhir pertandingan. Selain itu bot ini juga mengimplementasikan greedy pada penembakannya, dengan senjatanya yang selalu berputar dan ketika radar mendeteksi musuh maka bot ini akan langsung menembakkan peluru dengan kekuatan 2, sehingga diharapkan bot ini dapat mengenai musuh yang langsung dideteksi olehnya. Dengan demikian bot ini dirancang untuk menghindari tembakan musuh ketika terdapat banyak bot di permainan.
2. Bot AEZAKMI
 Bot ini memiliki algoritma greedy dalam pergerakannya dengan memilih arah pergerakan yang menjauhkannya dari musuh terakhir yang terdeteksi, bot akan memperhitungkan jarak mana yang paling jauh untuk kemudian diambil dan bergerak ke arah tersebut. Selain dari itu bot juga memprediksi gerakan musuh ketika mendeteksinya untuk kemudian bot menembak musuhnya dengan energi yang optimal.
3. Bot Dwifungsi
   Bot ini memiliki algoritma greedy dalam memindai musuh sehingga bot pertama yang discannya akan langsung di-lock dan langsung dikejar untuk ditabrak dan ditembak dengan daya yang lebih kuat. Selain dari itu bot ini juga memperhitungkan jarak musuh yang dipindainya, jika musuh jauh maka akan menembakkan sebesar satu energi, jika lebih dekat sebesar dua energi, jika paling dekat tiga energi. Sehingga bot ini memiliki algoritma yang sangat agresif untuk melock, menabrak, dan menembak musuh untuk memperoleh poin yang maksimal.
4. Bot Aedes Aegypti
   Bot ini memiliki algoritma greedy dalam pemindaiannya, bot ini akan melakukan locking terhadap musuhnya dan menembakkan peluru sesuai dengan energy yang dimilikinya, jika memiliki energy lebih dari 30 maka akan menembakkan sebesar 2,5 energy, sedangkan jika kurang dari sama dengan 30 maka akan menembakkan energy sebesar 1.

## Cara menjalankan program
Clone repository ke folder
```
git clone https://github.com/ahsuunn/Tubes1_YaMainLastWarLah.git
```

Pastikan sudah melakukan instalasi C# atau [.Net](https://dotnet.microsoft.com/en-us/download) dan mengecek kompatibilitas versinya dengan bot di file .csproj dan .json pada tag TargetFramework dan platformnya. Jika terdapat perbedaan versi silahkan ganti versi pada bot sesuai dengan versi .Net yang dimiliki.

Untuk menjalankan program pastikan berada di root folder repository dan jalankan command berikut:
```
java -jar .\robocode-tankroyale-gui-0.30.0.jar
```

Setelah menjalankan program masukkan directory bot dengan menambahkan path parent folder dari pada folder bot. Setelah itu jalankan program dengan menekan start battle dan memulai boot pada bot dan memasukkannya ke dalam pertandingan.


## Author
* 13523049 - Muhammad Fithra Rizki
* 13523055 - Muhammad Timur Kanigara
* 13523074 - Ahsan Malik Al Farisi 
