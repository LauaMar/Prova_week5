CREATE TABLE Prodotti (
ID int IDENTITY(1,1) NOT NULL,
CodiceProdotto nvarchar(10) NOT NULL UNIQUE,
Categoria nvarchar(20) NOT NULL,
Descrizione nvarchar(500) NOT NULL,
PrezzoUnitario numeric(10,2) NOT NULL,
QuantitaDisponibile int NOT NULL,
CONSTRAINT PK_Prodotti PRIMARY KEY(ID),
CONSTRAINT CK_Categoria CHECK (Categoria IN ('Alimentari','Cancelleria','Sanitari'))
)

