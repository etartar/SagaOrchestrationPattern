.NET Core ile Microservice Choreography-Based Saga Pattern kodlamas� yap�lm��t�r.

- Distributed Transaction nedir ?

Distributed Transaction senaryolar�nda, microservice'ler aras�nda data consistency(veri tutarl���) olay�n� y�netmeyi imkan veren bir pattern'd�r. Her bir microservice baz� durumlarda shared bir veritaban�na ba�lanabilir. Ama bizim genellikle istedi�imiz her microservicein kendisine ait bir veritaban� olmas�. Birden fazla microservice i�eren sistemde �rne�in sipari�in olu�turulmas� sto�un d��mesi gibi her biri ayr� microservice de ger�ekle�iyorsa biz bu durumlarda transactionlar� y�netmemiz laz�m.

- ACID Nedir?

De�i�ikliklerin veritaban�na nas�l uygulanaca��n� belirten prensiplerdir. Transactionlar'�n ACID (atomicity, consistency,isolation,durability) olmal�d�r.

	Atomicity: Ya hep, ya hi�
	Consistency: Datalar�n tutarl� olmas�. Veritaban�n� s�rekli valid tutar.
	Isolation: Transactionlar�n birbirinden ba��ms�z olmas�n� ifade eder.
	Durability: Datalar�n g�venli bir ortamda saklanmas�n� ifade eder.

- Orchestration-based saga

Microservice'ler aras�nda t�m transaction merkezi bir yerden y�netilir. (Saga State Machine)

	Uygulamas� daha zordur.. 
	4'den fazla microservice aras�nda bir distributed transaction y�netimi i�in uygun bir implementasyondur.
	Asynchronous messaging pattern kullanmak uygundur.
	Transaction y�netimi merkezi oldu�u i�in performance bottleneck fazlad�r.