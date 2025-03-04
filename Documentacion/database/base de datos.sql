
CREATE TABLE Cliente(
	Cliente_ID INT PRIMARY KEY IDENTITY,
	Nombre VARCHAR(50) NOT NULL,
	Apellido VARCHAR(50) NOT NULL,
	dui VARCHAR(10) NOT NULL
)


CREATE TABLE TarjetaCredito(
	TarjetaCredito_ID INT PRIMARY KEY IDENTITY,
	Cliente_ID INT FOREIGN KEY REFERENCES Cliente(Cliente_ID),
	Numero_Tarjeta VARCHAR(16) NOT NULL,
	Limite_Credito DECIMAL NOT NULL,
	Saldo_Utilizado DECIMAL NOT NULL,
	TasaInteresConfigurable DECIMAL NOT NULL,
	Fecha_Corte DATE NOT NULL,
	Fecha_Pago DATE NOT NULL,
	porcentaje_Saldo_min DECIMAL NOT NULL
	)


CREATE TABLE MovimientoTarjeta(
	Movimiento_Tarjeta_ID INT PRIMARY KEY IDENTITY,
	TarjetaCredito_ID INT FOREIGN KEY REFERENCES TarjetaCredito(TarjetaCredito_ID),
	Fecha_Movimiento DATETIME NOT NULL,
	Descripcion VARCHAR(MAX) NOT NULL,
	Monto DECIMAL NOT NULL,
	Tipo_Movimiento VARCHAR(25) NOT NULL
	)
