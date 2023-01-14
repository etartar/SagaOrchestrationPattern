.NET Core ile Microservice Choreography-Based Saga Pattern kodlamasý yapýlmýþtýr.

- Distributed Transaction nedir ?

Distributed Transaction senaryolarýnda, microservice'ler arasýnda data consistency(veri tutarlýðý) olayýný yönetmeyi imkan veren bir pattern'dýr. Her bir microservice bazý durumlarda shared bir veritabanýna baðlanabilir. Ama bizim genellikle istediðimiz her microservicein kendisine ait bir veritabaný olmasý. Birden fazla microservice içeren sistemde örneðin sipariþin oluþturulmasý stoðun düþmesi gibi her biri ayrý microservice de gerçekleþiyorsa biz bu durumlarda transactionlarý yönetmemiz lazým.

- ACID Nedir?

Deðiþikliklerin veritabanýna nasýl uygulanacaðýný belirten prensiplerdir. Transactionlar'ýn ACID (atomicity, consistency,isolation,durability) olmalýdýr.

	Atomicity: Ya hep, ya hiç
	Consistency: Datalarýn tutarlý olmasý. Veritabanýný sürekli valid tutar.
	Isolation: Transactionlarýn birbirinden baðýmsýz olmasýný ifade eder.
	Durability: Datalarýn güvenli bir ortamda saklanmasýný ifade eder.

- Orchestration-based saga

Microservice'ler arasýnda tüm transaction merkezi bir yerden yönetilir. (Saga State Machine)

	Uygulamasý daha zordur.. 
	4'den fazla microservice arasýnda bir distributed transaction yönetimi için uygun bir implementasyondur.
	Asynchronous messaging pattern kullanmak uygundur.
	Transaction yönetimi merkezi olduðu için performance bottleneck fazladýr.